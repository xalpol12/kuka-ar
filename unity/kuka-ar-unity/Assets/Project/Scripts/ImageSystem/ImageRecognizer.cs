using System.Collections.Generic;
using Project.Scripts.AnchorSystem;
using Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Project.Scripts.ImageSystem
{
    [RequireComponent(typeof(ARTrackedImageManager))]
    public class ImageRecognizer : MonoBehaviour
    {
        private AnchorManager anchorManager;
        private ARTrackedImageManager imageManager;
        private Dictionary<string, ARTrackedImage> trackedImages;

        private void Awake()
        {
            anchorManager = gameObject.GetComponent<AnchorManager>();
            imageManager = gameObject.GetComponent<ARTrackedImageManager>();
        }

        private void Start()
        {
            trackedImages = new Dictionary<string, ARTrackedImage>();
            
            ConfigureMutableLibrary();
            
            imageManager.trackedImagesChanged += OnChange;
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
    }
}
