using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARTrackedImageManager))]
public class AnchorWithChildManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [Space]
    [SerializeField] private GameObject childPrefab;

    private ARAnchorManager anchorManager;
    private ARTrackedImageManager imageManager;
    private List<ARAnchor> anchors;
    private ARTrackedImage refImage;
    private ARAnchor createdAnchor;
    private GameObject child;
    
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
                var position = refImage.transform.position;
                var rotation = refImage.transform.rotation * Quaternion.Euler(0, 0, 90);
                var anchor = anchorManager.AddAnchor(new Pose(position, rotation));
                anchors.Add(anchor);
                text.text += "Anchor created\n" + "Position: " + anchor.transform.localPosition;
                isTracked = true;
                child = Instantiate(childPrefab, anchor.transform);
                float x = 5;
                float y = 2.5f;
                float z = 1.5f;
                child.transform.localPosition = new Vector3(y,x , z);
                text.text += "Child created\n" + "Position: " + child.transform.localPosition;
            }
            yield return null;
        }
    }
}
