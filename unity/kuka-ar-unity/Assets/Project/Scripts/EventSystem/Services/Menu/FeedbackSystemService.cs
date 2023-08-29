using Project.Scripts.EventSystem.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.EventSystem.Services.Menu
{
    public class FeedbackSystemService : MonoBehaviour
    {
        public static FeedbackSystemService Instance;
        public GameObject feedbackSystem;
        
        private SelectableStylingService stylingService;
        [SerializeField] private GameObject popupConfirm;
        [SerializeField] private GameObject popupError;
        private TMP_Text popupTitle;
        private TMP_Text popupHeader;
        private TMP_Text popupMessage;
        private Image popupIcon;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            feedbackSystem.SetActive(false);
            var confirmationPopup = feedbackSystem.transform.Find("Popup")
                .GetComponent<RectTransform>().gameObject;
            popupTitle = confirmationPopup.transform.Find("Title").GetComponent<TMP_Text>();
            popupHeader = confirmationPopup.transform.Find("Header").GetComponent<TMP_Text>();
            popupMessage = confirmationPopup.transform.Find("Message").GetComponent<TMP_Text>();
            popupIcon = confirmationPopup.transform.Find("Icon").GetComponent<Image>();
            
            popupConfirm.SetActive(false);
            popupError.SetActive(false);

        }

        internal void OpenDialog(string message, string header = "",
            string title = "", Sprite icon = null, PopupType type = PopupType.Confirm)
        {
            popupTitle.text = title;
            popupHeader.text = header;
            popupMessage.text = message;
            popupIcon.sprite = icon == null ? stylingService.DefaultSprite : icon;

            popupConfirm.SetActive(type == PopupType.Confirm);
            popupError.SetActive(type == PopupType.Error);
        }
    }
}
