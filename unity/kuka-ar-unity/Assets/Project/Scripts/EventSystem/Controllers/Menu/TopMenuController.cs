using Project.Scripts.Connectivity.Models.KRLValues;
using Project.Scripts.TrackedRobots;
using TMPro;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class TopMenuController : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Tracked robots handler reference game object")]
        private GameObject robotModel;

        private TrackedRobotsHandler trackedRobot;
        private TMP_Text toolText;
        private TMP_Text baseText;

        private void Awake()
        {
            trackedRobot = robotModel.GetComponent<TrackedRobotsHandler>();
        }

        private void Start()
        {
            var topMenu = GetComponent<Transform>().gameObject;
            var constantTopPanel = topMenu.GetComponent<RectTransform>()
                .Find("ConstantInfoPanel").GetComponent<RectTransform>().gameObject;
            
            toolText = constantTopPanel.transform.Find("ToolBean").GetComponent<RectTransform>().gameObject
                .transform.Find("ToolBackground").GetComponent<RectTransform>().gameObject
                .transform.Find("Tool").GetComponent<TMP_Text>();
            baseText = constantTopPanel.transform.Find("BaseBean").GetComponent<RectTransform>().gameObject
                .transform.Find("BaseBackground").GetComponent<RectTransform>().gameObject
                .transform.Find("Base").GetComponent<TMP_Text>();
            
            trackedRobot.ActiveBaseUpdated += OnBaseValueChange;
            trackedRobot.ActiveToolUpdated += OnToolValueChange;
        }

        private void OnBaseValueChange(object _, KRLInt e)
        {
            baseText.text = "Base\n" + e.Value;
        }

        private void OnToolValueChange(object _, KRLInt e)
        {
            toolText.text = "Tool\n" + e.Value;
        }
    }
}
