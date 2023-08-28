using System.Collections;
using System.Collections.Generic;
using Project.Scripts.AnchorSystem;
using Project.Scripts.Connectivity.Http;
using Project.Scripts.Connectivity.Http.Requests;
using Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Project.Scripts.ImageSystem
{
    public class MutableImageRecognizer : MonoBehaviour
    {
        public static MutableImageRecognizer Instance;
        
        [SerializeField] private XRReferenceImageLibrary runtimeImageLibrary;
        [SerializeField] private GameObject arPrefab;
        
        private AnchorManager anchorManager;
        private ARTrackedImageManager imageManager;
        private Dictionary<string, ARTrackedImage> trackedImages;

        private HttpClientWrapper httpClientWrapper;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            anchorManager = gameObject.GetComponent<AnchorManager>();
        }

        private void Start()
        {
            trackedImages = new Dictionary<string, ARTrackedImage>();
            
            httpClientWrapper = HttpClientWrapper.Instance;

            imageManager = gameObject.AddComponent<ARTrackedImageManager>();
            ConfigureMutableLibrary();
            imageManager.trackedImagesChanged += OnChange;
            
            DebugLogger.Instance.AddLog("Device supports mutable tracked image library: " + 
                                        imageManager.subsystem.subsystemDescriptor.supportsMutableLibrary + "; ");
        }

        private void ConfigureMutableLibrary()
        {
            #if !UNITY_EDITOR || !UNITY_EDITOR_WIN
            imageManager.referenceLibrary = imageManager.CreateRuntimeLibrary(runtimeImageLibrary);
            imageManager.requestedMaxNumberOfMovingImages = 5; //TODO: change later
            imageManager.trackedImagePrefab = arPrefab;
            imageManager.enabled = true;
            #endif
        }

        private void OnChange(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (var newImage in eventArgs.added)
            {
                trackedImages.Add(newImage.referenceImage.name, newImage);
                StartCoroutine(anchorManager.StartNewAnchorTracking(newImage));
                DebugLogger.Instance.AddLog($"Current tracked images count: {trackedImages.Count.ToString()}; ");
            }
        }

        public void LoadNewTargets()
        {
            StartCoroutine(LoadTargetsFromServer());
        }

        private IEnumerator LoadTargetsFromServer()
        {
            var newTargetsTask = httpClientWrapper.ExecuteRequest(new GetTargetImagesRequest());

            while (!newTargetsTask.IsCompleted)
            {
                yield return null;
            }
            
            // SetNewTargets(newTargetsTask);
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
                DebugLogger.Instance.AddLog("Waiting for image to add... ;");
                yield return null;
            }
            DebugLogger.Instance.AddLog($"New image {image.Key} added; ");
        }
    }
}
