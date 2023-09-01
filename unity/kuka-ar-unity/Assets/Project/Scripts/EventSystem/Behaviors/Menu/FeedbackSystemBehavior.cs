using Project.Scripts.EventSystem.Controllers.Menu;
using Project.Scripts.EventSystem.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.EventSystem.Behaviors.Menu
{
    public class FeedbackSystemBehavior : MonoBehaviour
    {
        private FeedbackSystemController feedbackController;
        private TMP_Text titleText;
        private TMP_Text headerText;
        private TMP_Text messageText;
        private Image icon;
        private void Start()
        {
            feedbackController = GetComponent<FeedbackSystemController>();

            var window = feedbackController.PopupDialog.transform;
            titleText = window.Find("Title").GetComponent<TMP_Text>();
            headerText = window.Find("Header").GetComponent<TMP_Text>();
            messageText = window.Find("Message").GetComponent<TMP_Text>();
            icon = window.Find("Icon").GetComponent<Image>();
        }

        private void Update()
        {
            if (feedbackController.FeedbackAnim == AnimationStates.FadeIn)
            {
                var data = feedbackController.PopupDialog.Content;
                titleText.text = data.Title;
                headerText.text = data.Header;
                messageText.text = data.Message;
                icon.sprite = data.Icon;

                feedbackController.FeedbackAnim = AnimationStates.StandBy;
            }
        }
    }
}
