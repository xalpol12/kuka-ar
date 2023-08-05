using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARTrackedImageManager))]
public class AnchorCreationManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

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

    // Update is called once per frame
    void Update()
    {

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
                var position = refImage.transform.position;
                var rotation = refImage.transform.rotation;
                var anchor = anchorManager.AddAnchor(new Pose(position, rotation));
                anchors.Add(anchor);
                text.text += "Anchor created\n" + "Position: " + anchor.transform.position;
                isTracked = true;
            }
            yield return null;
        }
    }
}
