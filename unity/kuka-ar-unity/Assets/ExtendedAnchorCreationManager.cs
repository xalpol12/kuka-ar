using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARTrackedImageManager))]
public class ExtendedAnchorCreationManager : MonoBehaviour
{
    [Space]
    [SerializeField] private TextMeshProUGUI text;
    [Space]
    [SerializeField] private Vector3 positionShift;
    [SerializeField] private Vector3 rotationShift;
    
    private ARAnchorManager anchorManager;
    private ARTrackedImageManager imageManager;
    private List<ARAnchor> anchors;
    private ARTrackedImage refImage;
    private void Awake()
    {
        anchorManager = gameObject.GetComponent<ARAnchorManager>();
        imageManager = gameObject.GetComponent<ARTrackedImageManager>();
    }

    void Start()
    {
        imageManager.trackedImagesChanged += OnChange;
        anchors = new List<ARAnchor>();
    }

    private void OnChange(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var added in eventArgs.added)
        {
            refImage = added;
            StartCoroutine(CreateAnchor());
            text.text += "Image found";
        }
    }

    IEnumerator CreateAnchor()
    {
        bool isTracked = false;
        while (!isTracked)
        {
            if (refImage.trackingState == TrackingState.Tracking)
            {
                var position = refImage.transform.localPosition + positionShift;
                var rotation = refImage.transform.localRotation * Quaternion.Euler(rotationShift);
                var anchor = anchorManager.AddAnchor(new Pose(position, rotation));
                anchors.Add(anchor);
                text.text += "Anchor created\n" + "Position: " + anchor.transform.position;
                isTracked = true;
            }
            yield return null;
        }
    }
}
