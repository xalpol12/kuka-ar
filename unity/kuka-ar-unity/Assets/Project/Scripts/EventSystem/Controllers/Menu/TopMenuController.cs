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
        private TMP_Text tool;
        private TMP_Text world;

        private void Awake()
        {
            trackedRobot = robotModel.GetComponent<TrackedRobotsHandler>();
        }

        private void Start()
        {
            var topMenu = GetComponent<Transform>().gameObject;
            var constantTopPanel = topMenu.GetComponent<RectTransform>()
                .Find("ConstantInfoPanel").GetComponent<RectTransform>().gameObject;
            
            tool = constantTopPanel.transform.Find("ToolBean").GetComponent<RectTransform>().gameObject
                .transform.Find("Tool").GetComponent<TMP_Text>();
            world = constantTopPanel.transform.Find("WorldBean").GetComponent<RectTransform>().gameObject
                .transform.Find("World").GetComponent<TMP_Text>();
            
            trackedRobot.ActiveBaseUpdated += OnBaseValueChange;
            trackedRobot.ActiveToolUpdated += OnToolValueChange;
        }

        private void OnBaseValueChange(object _, KRLInt e)
        {
            world.text = "World\n" + e.Value;
        }

        private void OnToolValueChange(object _, KRLInt e)
        {
            tool.text = "Tool\n" + e.Value;
        }
    }
}
