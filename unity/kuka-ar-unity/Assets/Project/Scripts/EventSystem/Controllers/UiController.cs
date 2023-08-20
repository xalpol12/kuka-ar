using System.Collections.Generic;
using Project.Scripts.EventSystem.Enums;
using Project.Scripts.EventSystem.Events;
using Project.Scripts.EventSystem.Services.ServerConfig;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers
{
    public class UiController : MonoBehaviour
    {
        public int id;
        public GameObject menuUi;
        public GameObject moreOptions;
        public GameObject serverConfig;
        public float animSpeed;
        internal AnimationStates ServerConfigAnim;
        internal AnimationStates MenuAnim;
        internal AnimationStates MoreOptionsAnim;
        internal List<string> NextAnim;
        private IpValidationService validationService;
        private bool showMoreOptionsDialog;

        void Start()
        {
            validationService = IpValidationService.Instance;
        
            ServerConfigAnim = AnimationStates.FadeIn;
            MenuAnim = AnimationStates.StandBy;
            MoreOptionsAnim = AnimationStates.StandBy;
            NextAnim = new List<string>();
        
            menuUi.SetActive(false);
            moreOptions.SetActive(false);
            serverConfig.SetActive(true);
        
            MenuEvents.Event.OnClickMoreOptions += ShowMoreOptions;
            ServerConfigEvents.Events.OnClickSaveServerConfig += SaveServerConfiguration;
            MoreOptionsEvents.Events.onClickBack += GoToMainScreen;
        }

        private void ShowMoreOptions(int uid)
        {
            if (id == uid)
            {
                MenuAnim = AnimationStates.FadeOut;
                NextAnim.Add("MoreOptionsIn");
            }
        }

        private void SaveServerConfiguration(int uid)
        {
        
            if (id == uid && validationService.ValidationResult)
            {
                ServerConfigAnim = AnimationStates.FadeOut;
                NextAnim.Add("MenuIn");
            }
        }

        private void GoToMainScreen(int uid)
        {
            if (id == uid)
            {
                MoreOptionsAnim = AnimationStates.FadeOut;
                NextAnim.Add("MenuIn");
            }
        }
    }
}
