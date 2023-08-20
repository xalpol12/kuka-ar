using System.Collections;
using System.Collections.Generic;
using Project.Scripts.AnchorSystem;
using Project.Scripts.Connectivity.RestAPI;
using Project.Scripts.Connectivity.RestAPI.Commands;
using Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Project.Scripts.ImageSystem
{
    [RequireComponent(typeof(ARTrackedImageManager))]
    public class MutableImageRecognizer : MonoBehaviour
    {
        private AnchorManager anchorManager;
        private ARTrackedImageManager imageManager;
        private Dictionary<string, ARTrackedImage> trackedImages;

        private RestClient restClient;

        private void Awake()
        {
            anchorManager = gameObject.GetComponent<AnchorManager>();
            imageManager = gameObject.GetComponent<ARTrackedImageManager>();
        }

        private void Start()
        {
            trackedImages = new Dictionary<string, ARTrackedImage>();
            restClient = RestClient.Instance;
            
            ConfigureMutableLibrary();
            imageManager.trackedImagesChanged += OnChange;
            
            DebugLogger.Instance().AddLog(
                "Device supports mutable image library: " +
                imageManager.subsystem.subsystemDescriptor.supportsMutableLibrary + "; ");
        }

        private void ConfigureMutableLibrary()
        {
            var constantImageLib = imageManager.GetComponent<XRReferenceImageLibrary>();
            imageManager.referenceLibrary = imageManager.CreateRuntimeLibrary(constantImageLib);
            imageManager.requestedMaxNumberOfMovingImages = 5; //TODO: change later
            imageManager.trackedImagePrefab = imageManager.GetComponent<GameObject>(); //TODO: check if prefab works
            imageManager.enabled = true;
        }

        private void OnChange(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (var newImage in eventArgs.added)
            {
                trackedImages.Add(newImage.referenceImage.name, newImage);
                StartCoroutine(anchorManager.StartNewAnchorTracking(newImage));
                DebugLogger.Instance().AddLog($"Current tracked images count: {trackedImages.Count.ToString()}; ");
            }
        }

        public IEnumerator LoadTargetsFromServer()
        {
            var newTargetsTask = restClient.ExecuteCommand(new GetTargetImagesCommand());
            
            while (!newTargetsTask.IsCompleted)
            {
                yield return null;
            }
            
            SetNewTargets(newTargetsTask.Result);
        }

        private void SetNewTargets(Dictionary<string, byte[]> targets)
        {
            var textureDict = new Dictionary<string, Texture2D>();

            foreach (var entry in targets)
            {
                Texture2D texture = new Texture2D(512, 512);
                texture.LoadImage(entry.Value);
                texture.Apply();
                textureDict.Add(entry.Key, texture);
            }

            foreach (var entry in textureDict)
            {
                StartCoroutine(AddImageToTrackingLibrary(entry));
            }
        }

        private IEnumerator AddImageToTrackingLibrary(KeyValuePair<string, Texture2D> image)
        {
            yield return null;

            var mutableLib = imageManager.referenceLibrary as MutableRuntimeReferenceImageLibrary;

            var jobHandler = mutableLib.ScheduleAddImageWithValidationJob(image.Value, image.Key, 0.1f);
            
            while (jobHandler.status == AddReferenceImageJobStatus.Pending)
            {
                DebugLogger.Instance().AddLog("Waiting for image to add... ;");
                yield return null;
            }
            DebugLogger.Instance().AddLog("New image added; ");
        }
    }
}
