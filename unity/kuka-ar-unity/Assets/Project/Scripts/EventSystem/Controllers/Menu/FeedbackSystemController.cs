using Project.Scripts.Connectivity.Extensions;
using Project.Scripts.EventSystem.Enums;
using Project.Scripts.EventSystem.Events;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class FeedbackSystemController : MonoBehaviour
    {
        public int id;
        
        internal Popup PopupDialog;
        internal AnimationStates FeedbackAnim;
        private void Start()
        {
            PopupDialog = Popup.Window;
            FeedbackAnim = AnimationStates.StandBy;
            FeedbackSystemEvents.Events.OnClickHidePopup += HidePopup;
        }

        private void HidePopup(int uid)
        {
            Debug.Log("id"+ id + "   UID" + uid);
            if (id == uid)
            {
                Debug.Log("click");
                StartCoroutine(PopupDialog.ScaleDown());
            }
        }
    }
}
