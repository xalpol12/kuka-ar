using System.Collections.Generic;
using Project.Scripts.EventSystem.Enums;
using UnityEngine;

public class UiController : MonoBehaviour
{
    public int id;
    public float animSpeed;
    public GameObject menuUi;
    public GameObject moreOptions;
    public GameObject serverConfig;
    public GameObject webView;
    
    internal AnimationStates ServerConfigAnim;
    internal AnimationStates MenuAnim;
    internal AnimationStates MoreOptionsAnim;
    internal AnimationStates WebViewAnim;
    internal List<string> NextAnim;
    private IpValidationService validationService;
    
    [SerializeField] private GameObject abortServerConfigArrow;
    private bool showMoreOptionsDialog;

    void Start()
    {
        validationService = IpValidationService.Instance;
        
        ServerConfigAnim = AnimationStates.FadeIn;
        MenuAnim = AnimationStates.StandBy;
        MoreOptionsAnim = AnimationStates.StandBy;
        WebViewAnim = AnimationStates.StandBy;
        
        NextAnim = new List<string>();
        
        menuUi.SetActive(false);
        moreOptions.SetActive(false);
        serverConfig.SetActive(true);
        webView.SetActive(false);
        
        MenuEvents.Event.OnClickMoreOptions += ShowMoreOptions;
        ServerConfigEvents.Events.OnClickSaveServerConfig += SaveServerConfiguration;
        ServerConfigEvents.Events.OnClickBackToMenu += AbortServerReconfiguration;
        MoreOptionsEvents.Events.OnClickBack += GoToMainScreen;
        MoreOptionsEvents.Events.OnClickDisplayServer += ReconfigureServer;
        MoreOptionsEvents.Events.OnClickDisplayBrowser += SubmitAnIssue;
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
            MoreOptionsAnim = AnimationStates.FadeOut;
            NextAnim.Add("WebViewIn");
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
}
