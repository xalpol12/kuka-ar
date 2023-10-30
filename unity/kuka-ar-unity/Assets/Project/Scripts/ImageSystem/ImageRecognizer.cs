using System.Collections.Generic;
using Project.Scripts.AnchorSystem;
using Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

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
            imageManager.trackedImagesChanged += OnChange;
        }

        private void OnChange(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (var newImage in eventArgs.added)
            {
                trackedImages.Add(newImage.referenceImage.name, newImage);
                StartCoroutine(anchorManager.StartNewAnchorTracking(newImage));
            }
        }
    }
}
