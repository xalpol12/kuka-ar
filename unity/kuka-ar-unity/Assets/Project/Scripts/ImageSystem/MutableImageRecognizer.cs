using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.AnchorSystem;
using Project.Scripts.Connectivity.Http;
using Project.Scripts.Connectivity.Http.Requests;
using Project.Scripts.TrackedRobots;
using Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Project.Scripts.ImageSystem
{
    public class MutableImageRecognizer : MonoBehaviour
    {
        public static MutableImageRecognizer Instance;

        private const int MaxNumberOfMovingImages = 5;

        [SerializeField] private XRReferenceImageLibrary runtimeImageLibrary;
        [SerializeField] private GameObject imageRecognisedPrefab;
        [SerializeField] private TrackedRobotsHandler trackedRobotsHandler;

        private AnchorManager anchorManager;
        private ARTrackedImageManager imageManager;
        private Dictionary<string, Texture2D> downloadedTextures;

        private HttpClientWrapper httpClientWrapper;

        private void Awake()
        {
            Instance = this;
            anchorManager = gameObject.GetComponent<AnchorManager>();
        }

        private void Start()
        {
            httpClientWrapper = HttpClientWrapper.Instance;

            downloadedTextures = new();

            trackedRobotsHandler.FirstSelectionOfRobot += (_, _) => { TurnOnImageDetection(); };

            trackedRobotsHandler.RobotConnectionReset += (_, _) =>
            {
                DebugLogger.Instance.AddLog(
                    "Received invoke from RobotConnectionReset event; "); // TODO: Reset on 'Reload' btn click
                ResetImageLibraryToCleanState();
            };

            imageManager = gameObject.AddComponent<ARTrackedImageManager>();
            ConfigureMutableLibrary();
            imageManager.trackedImagesChanged += OnChange;

            DebugLogger.Instance.AddLog(imageManager.subsystem.subsystemDescriptor.supportsMutableLibrary
                ? "Device supports mutable tracked image library; "
                : "Device does not support mutable tracked image library; ");
        }

        private void ConfigureMutableLibrary()
        {
            imageManager.referenceLibrary = imageManager.CreateRuntimeLibrary(runtimeImageLibrary);
            imageManager.requestedMaxNumberOfMovingImages = MaxNumberOfMovingImages;
            imageManager.trackedImagePrefab = imageRecognisedPrefab;
            imageManager.enabled = false;
        }

        private void OnChange(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (var newImage in eventArgs.added)
            {
                if (trackedRobotsHandler.SelectedRobotIP != newImage.referenceImage.name) return;
                StartCoroutine(anchorManager.StartNewAnchorTracking(newImage));
            }
        }

        public void LoadNewTargets()
        {
            StartCoroutine(LoadTargetsFromServer());
            StartCoroutine(anchorManager.LoadRequiredData());
        }

        private IEnumerator LoadTargetsFromServer()
        {
            var newTargetsTask = httpClientWrapper.ExecuteRequest(new GetTargetImagesRequest());

            yield return new WaitUntil(() => newTargetsTask.IsCompleted);

            SetNewTargets(newTargetsTask.Result);
        }

        private void SetNewTargets(Dictionary<string, byte[]> targets)
        {
            foreach (var entry in targets)
            {
                if (downloadedTextures.ContainsKey(entry.Key))
                {
                    targets.Remove(entry.Key);
                }
                else
                {
                    var texture = new Texture2D(512, 512);
                    texture.LoadImage(entry.Value);
                    texture.Apply();
                    downloadedTextures.Add(entry.Key, texture);
                }
            }

            InitializeAddingImagesToTrackingLibrary();
        }

        private void InitializeAddingImagesToTrackingLibrary()
        {
            foreach (var entry in downloadedTextures)
            {
                StartCoroutine(AddImageToTrackingLibrary(entry));
            }
        }

        private IEnumerator AddImageToTrackingLibrary(KeyValuePair<string, Texture2D> image)
        {
            var mutableLib = imageManager.referenceLibrary as MutableRuntimeReferenceImageLibrary;
            var jobHandler = mutableLib.ScheduleAddImageWithValidationJob(image.Value, image.Key, 0.1f);

            yield return new WaitWhile(() =>
                jobHandler.status == AddReferenceImageJobStatus.Pending);
        }

        private void TurnOnImageDetection()
        {
            if (!imageManager.isActiveAndEnabled)
            {
                imageManager.enabled = true;
                DebugLogger.Instance.AddLog("Image recognition has been enabled; ");
            }
        }

        private void ResetImageLibraryToCleanState()
        {
            imageManager.referenceLibrary = runtimeImageLibrary;
            imageManager.requestedMaxNumberOfMovingImages = MaxNumberOfMovingImages;
            InitializeAddingImagesToTrackingLibrary();
        }

    }
}
