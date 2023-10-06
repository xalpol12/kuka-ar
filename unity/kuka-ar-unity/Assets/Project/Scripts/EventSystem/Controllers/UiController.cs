using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Connectivity.Http;
using Project.Scripts.Connectivity.Http.Requests;
using Project.Scripts.Connectivity.WebSocket;
using Project.Scripts.EventSystem.Enums;
using Project.Scripts.EventSystem.Events;
using Project.Scripts.EventSystem.Services.Menu;
using Project.Scripts.EventSystem.Services.ServerConfig;
using Project.Scripts.ImageSystem;
using TMPro;
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

        [NonSerialized] public GameObject TopMenuUi;
        
        [NonSerialized] public AnimationStates ServerConfigAnim;
        [NonSerialized] public AnimationStates MenuAnim;
        [NonSerialized] public AnimationStates MoreOptionsAnim;
        [NonSerialized] public AnimationStates FocusModeAnim;
        [NonSerialized] public List<AnimationFilter> NextAnim;
    
        [SerializeField] private GameObject abortServerConfigArrow;
        [SerializeField] private GameObject focusModeToggle;
        [SerializeField] private TMP_Text noInternetText;
        private IpValidationService validationService;
        private SelectableStylingService stylingService;
        private Toggle selectedMode;
        private int serverConfigDisplayState;
        private bool isAfterBugReport;
        private bool isQuitting;
        
        private bool dataNeedsToBeFetched;

        private void Start()
        {
            validationService = IpValidationService.Instance;
            stylingService = SelectableStylingService.Instance;
            
            TopMenuUi = menuUi.transform.Find("Canvas").transform.Find("TopMenu").GetComponent<Transform>().gameObject;
            
            NextAnim = new List<AnimationFilter>();
            selectedMode = focusModeToggle.GetComponent<Toggle>();
            isQuitting = false;
        
            SetFadeController();
            if (PlayerPrefs.GetInt("firstRun") == PlayersPrefsStates.FirstRun)
            {
                SetPrefabsActiveState(false, true, false);
            }
            else if (isAfterBugReport)
            {
                SetPrefabsActiveState(false,false,true);

                dataNeedsToBeFetched = true;
            }
            else
            {
                SetPrefabsActiveState(true, false, false);

                menuUi.transform.Find("Canvas").GetComponent<CanvasGroup>().alpha = 1;
                serverConfig.transform.Find("Canvas").GetComponent<CanvasGroup>().alpha = 0;

                dataNeedsToBeFetched = true;
            }

            LoadAddressFromPlayerPrefsIfPresent();

            MenuEvents.Event.OnClickMoreOptions += ShowMoreOptions;
            MenuEvents.Event.OnClickReloadServerData += RequestData;
            ServerConfigEvents.Events.OnClickSaveServerConfig += SaveServerConfiguration;
            ServerConfigEvents.Events.OnClickBackToMenu += AbortServerReconfiguration;
            MoreOptionsEvents.Events.OnClickBack += GoToMainScreen;
            MoreOptionsEvents.Events.OnClickDisplayServer += ReconfigureServer;
            MoreOptionsEvents.Events.OnClickDisplayBrowser += SubmitAnIssue;
            FocusModeEvents.Events.OnClickDisplayMoreOptions += FocusModeHandler;
        }

        private void LoadAddressFromPlayerPrefsIfPresent()
        {
            if (string.IsNullOrWhiteSpace(PlayerPrefs.GetString("serverIp"))) return;
            var cachedAddress = PlayerPrefs.GetString("serverIp");
            HttpClientWrapper.Instance.BaseAddress = cachedAddress;
            WebSocketClient.Instance.ConnectToWebsocket($"ws://{cachedAddress}:8080/kuka-variables");
        }

        private void RequestData(int uid)
        {
            if (id != uid) return;
            ServerInvoker.Invoker.GetFullData();
        }

        private void OnEnable()
        {
            isAfterBugReport = PlayerPrefs.GetString("isAfterBugReport") == true.ToString();
        }

        private void OnDisable()
        {
            if (isQuitting) return;
            PlayerPrefs.SetString("isAfterBugReport", isAfterBugReport.ToString());
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.SetString("isAfterBugReport", false.ToString());
            isQuitting = true;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                PlayerPrefs.SetString("isAfterBugReport", false.ToString());
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                PlayerPrefs.SetString("isAfterBugReport", false.ToString());
            }
        }

        private void ShowMoreOptions(int uid)
        {
            if (id != uid) return;
            MenuAnim = AnimationStates.FadeOut;
            NextAnim.Add(AnimationFilter.MoreOptionsIn);
        }

        private void SaveServerConfiguration(int uid)
        {
            if (id != uid || !validationService.ValidationResult) return;
            ServerConfigAnim = AnimationStates.FadeOut;
            NextAnim.Add(AnimationFilter.MenuIn);

            if (PlayerPrefs.GetInt("firstRun") == PlayersPrefsStates.FirstRun)
            {
                PlayerPrefs.SetInt("firstRun", PlayersPrefsStates.NthRun);
            }
            
            WebSocketClient.Instance.ConnectToWebsocket(
                $"ws://{HttpClientWrapper.Instance.BaseAddress}:8080/kuka-variables");
        }

        private void GoToMainScreen(int uid)
        {
            if (id != uid) return;
            MoreOptionsAnim = AnimationStates.FadeOut;
            NextAnim.Add(selectedMode.isOn ? AnimationFilter.FocusModeIn : AnimationFilter.MenuIn);
        }

        private void ReconfigureServer(int uid)
        {
            if (id != uid) return;
            MoreOptionsAnim = AnimationStates.FadeOut;
            NextAnim.Add(AnimationFilter.ServerConfigScreenIn);
            abortServerConfigArrow.SetActive(true);
        }

        private void SubmitAnIssue(int uid)
        {
            var internet = Application.internetReachability == NetworkReachability.NotReachable;
            if (id == uid && !internet)
            {
                SceneManager.LoadScene("WebViewScene");
                isAfterBugReport = true;
                return;
            }
            noInternetText.color = internet ? Color.red : Color.clear;
            StartCoroutine(stylingService.FadeOutText(noInternetText));
        }

        private void AbortServerReconfiguration(int uid)
        {
            if (id != uid) return;
            ServerConfigAnim = AnimationStates.FadeOut;
            NextAnim.Add(AnimationFilter.MoreOptionsIn);
        }

        private void FocusModeHandler(int uid)
        {
            if (id != uid) return;
            FocusModeAnim = AnimationStates.FadeOut;
            NextAnim.Add(AnimationFilter.MoreOptionsIn);
        }

        private void SetPrefabsActiveState(bool menu, bool server, bool options)
        {
            menuUi.SetActive(menu);
            TopMenuUi.SetActive(menu);
            serverConfig.SetActive(server);
            moreOptions.SetActive(options);
            focusMode.SetActive(false);
        }

        private void SetFadeController()
        {
            if (PlayerPrefs.GetString("isAfterBugReport") == true.ToString())
            {
                ServerConfigAnim = AnimationStates.StandBy;
                MenuAnim = AnimationStates.StandBy;
                MoreOptionsAnim = AnimationStates.FadeIn;
                FocusModeAnim = AnimationStates.StandBy;
                return;
            }
            ServerConfigAnim = PlayerPrefs.GetInt("firstRun") == PlayersPrefsStates.FirstRun ?
                AnimationStates.FadeIn : AnimationStates.StandBy;
            MenuAnim = PlayerPrefs.GetInt("firstRun") == PlayersPrefsStates.FirstRun ?
                AnimationStates.StandBy : AnimationStates.FadeIn;
            MoreOptionsAnim = AnimationStates.StandBy;
            FocusModeAnim = AnimationStates.StandBy;
        }

        private void Update()
        {
            if (!dataNeedsToBeFetched) return;
            InvokeFetchingRequiredData();
            dataNeedsToBeFetched = false;
        }

        private void InvokeFetchingRequiredData()
        {
            ServerInvoker.Invoker.GetFullData();
            MutableImageRecognizer.Instance.LoadNewTargets();
        }
    }
}
