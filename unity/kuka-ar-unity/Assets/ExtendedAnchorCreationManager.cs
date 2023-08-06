using System.Collections;
using System.Collections.Generic;
using Connectivity.Models;
using Project.Scripts.Connectivity.Models;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARTrackedImageManager))]
public class ExtendedAnchorCreationManager : MonoBehaviour
{
    private ARAnchorManager anchorManager;
    private ARTrackedImageManager imageManager;
    private Dictionary<string, ARAnchor> anchors;
    private Dictionary<string, ARTrackedImage> images;
    private Dictionary<string, RobotData> robotData;
    [SerializeField] TextMeshProUGUI debugLog;

    private void Awake()
    {
        anchorManager = gameObject.GetComponent<ARAnchorManager>();
        imageManager = gameObject.GetComponent<ARTrackedImageManager>();
    }

    void Start()
    {
        imageManager.trackedImagesChanged += OnChange;
        anchors = new Dictionary<string, ARAnchor>();
        images = new Dictionary<string, ARTrackedImage>();
        robotData = new Dictionary<string, RobotData>();
        var pos = new Vector3(0, -0.1f, 0);
        var rot = Vector3.zero;
        var robot = new RobotData();
        robot.Name = "Kuka";
        robot.PositionShift = pos;
        robot.RotationShift = rot;
        robotData.Add("192.168.1.50", robot);
    }

    private void OnChange(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var added in eventArgs.added)
        {
            images.Add(added.referenceImage.name, added);
            debugLog.text = "Picture: " + added.referenceImage.name + "\n";
            StartCoroutine(CreateAnchor(images[added.referenceImage.name]));
        }

        //foreach (var updated in eventArgs.updated)
        //{
        //    StartCoroutine(UpdateAnchor(images[updated.referenceImage.name]));
        //}
    }

    IEnumerator CreateAnchor(ARTrackedImage image)
    {
        debugLog.text += "Searching for reference points...\n";
        var data = robotData[image.referenceImage.name];
        bool isTracked = false;
        while (!isTracked)
        {
            yield return null;
            if (image.trackingState != TrackingState.Tracking) continue;
            var position = image.transform.position + data.PositionShift;
            var rotation = image.transform.rotation * Quaternion.Euler(data.RotationShift);
            var anchor = anchorManager.AddAnchor(new Pose(position, rotation));
            anchors.Add(image.referenceImage.name, anchor);
            isTracked = true;
        }
        debugLog.text += "Object placed\n";
        debugLog.text += anchors[image.referenceImage.name].transform.position;
    }

    IEnumerator UpdateAnchor(ARTrackedImage image)
    {
        var data = robotData[image.referenceImage.name];
        yield return null;
        if (image.trackingState == TrackingState.Tracking)
        {
            var anchor = anchors[image.referenceImage.name];
            var position = image.transform.position + data.PositionShift;
            var rotation = image.transform.rotation * Quaternion.Euler(data.RotationShift);
            anchor.transform.position = position;
            anchor.transform.rotation = rotation;
        }
    }
}
