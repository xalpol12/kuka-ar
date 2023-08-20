using System.Collections.Generic;
using Project.Scripts.EventSystem.Enums;
using Project.Scripts.EventSystem.Events;
using Project.Scripts.EventSystem.Services.ServerConfig;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.Scripts.EventSystem.Controllers
{
    public class UiController : MonoBehaviour
    {
        public int id;
        public float animSpeed;
        public GameObject menuUi;
        public GameObject moreOptions;
        public GameObject serverConfig;
        public GameObject focusMode;

        internal AnimationStates ServerConfigAnim;
        internal AnimationStates MenuAnim;
        internal AnimationStates MoreOptionsAnim;
        internal AnimationStates FocusModeAnim;
        internal List<string> NextAnim;
        private IpValidationService validationService;

        [SerializeField] private GameObject abortServerConfigArrow;
        [SerializeField] private GameObject focusModeToggle;
        private Toggle selectedMode;
        private int serverConfigDisplayState;
        private bool showMoreOptionsDialog;

        void Start()
        {
            validationService = IpValidationService.Instance;

            ServerConfigAnim = PlayerPrefs.GetInt("firstRun") == new PlayersPrefsStates().FirstRun ?
                AnimationStates.FadeIn : AnimationStates.StandBy;
            MenuAnim = PlayerPrefs.GetInt("firstRun") == new PlayersPrefsStates().FirstRun ?
                AnimationStates.StandBy : AnimationStates.FadeIn;
            MoreOptionsAnim = AnimationStates.StandBy;
            FocusModeAnim = AnimationStates.StandBy;

            NextAnim = new List<string>();
            selectedMode = focusModeToggle.GetComponent<Toggle>();
            if (PlayerPrefs.GetInt("firstRun") == new PlayersPrefsStates().FirstRun)
            {
                menuUi.SetActive(false);
                serverConfig.SetActive(true);
            }
            else
            {
                menuUi.SetActive(true);
                serverConfig.SetActive(false);

                menuUi.transform.Find("Canvas").GetComponent<CanvasGroup>().alpha = 1;
                serverConfig.transform.Find("Canvas").GetComponent<CanvasGroup>().alpha = 0;
            }

            moreOptions.SetActive(false);
            focusMode.SetActive(false);

            MenuEvents.Event.OnClickMoreOptions += ShowMoreOptions;
            ServerConfigEvents.Events.OnClickSaveServerConfig += SaveServerConfiguration;
            ServerConfigEvents.Events.OnClickBackToMenu += AbortServerReconfiguration;
            MoreOptionsEvents.Events.OnClickBack += GoToMainScreen;
            MoreOptionsEvents.Events.OnClickDisplayServer += ReconfigureServer;
            MoreOptionsEvents.Events.OnClickDisplayBrowser += SubmitAnIssue;
            FocusModeEvents.Events.OnClickDisplayMoreOptions += FocusModeHandler;
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
                PlayerPrefs.SetInt("firstRun", 1);
                PlayerPrefs.SetString("serverIp", HttpService.Instance.ConfiguredIp);
            }
        }

        private void GoToMainScreen(int uid)
        {
            if (id == uid)
            {
                MoreOptionsAnim = AnimationStates.FadeOut;
                if (selectedMode.isOn)
                {
                    NextAnim.Add("FocusModeIn");
                }
                else
                {
                    NextAnim.Add("MenuIn");
                }
            }
        }

        private void ReconfigureServer(int uid)
        {
            if (id == uid)
            {
                MoreOptionsAnim = AnimationStates.FadeOut;
                NextAnim.Add("ServerConfigScreenIn");
                abortServerConfigArrow.SetActive(true);
            }
        }

        private void SubmitAnIssue(int uid)
        {
            if (id == uid)
            {
                SceneManager.LoadScene("WebViewScene");
            }
        }

        private void AbortServerReconfiguration(int uid)
        {
            if (id == uid)
            {
                ServerConfigAnim = AnimationStates.FadeOut;
                NextAnim.Add("MenuIn");
            }
        }

        private void FocusModeHandler(int uid)
        {
            if (id == uid)
            {
                FocusModeAnim = AnimationStates.FadeOut;
                NextAnim.Add("MoreOptionsIn");
            }
        }
    }
}
