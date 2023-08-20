using Project.Scripts.EventSystem.Enums;
using UnityEngine;

public class UiBehavior : MonoBehaviour
{
    private UiController controller;
    private CanvasGroup serverCanvasGroup;
    private CanvasGroup menuCanvasGroup;
    private CanvasGroup moreOptionsCanvasGroup;
    private CanvasGroup focusModeCanvasGroup;
    private GameObject serverBackArrow;
    void Start()
    {
        controller = GetComponent<UiController>();

        serverCanvasGroup = controller.serverConfig.transform.Find("Canvas").GetComponent<CanvasGroup>();
        menuCanvasGroup = controller.menuUi.transform.Find("Canvas").GetComponent<CanvasGroup>();
        moreOptionsCanvasGroup = controller.moreOptions.transform.Find("Canvas").GetComponent<CanvasGroup>();
        focusModeCanvasGroup = controller.focusMode.transform.Find("Canvas").GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (controller.ServerConfigAnim == AnimationStates.FadeIn)
        {
            FadeIn(serverCanvasGroup);
        }
        else if(controller.ServerConfigAnim == AnimationStates.FadeOut)
        {
            FadeOut(serverCanvasGroup);
        }
        
        if (controller.MenuAnim == AnimationStates.FadeIn)
        {
            FadeIn(menuCanvasGroup);
        }
        else if(controller.MenuAnim == AnimationStates.FadeOut)
        {
            FadeOut(menuCanvasGroup);
        }
        
        if (controller.MoreOptionsAnim == AnimationStates.FadeIn)
        {
            FadeIn(moreOptionsCanvasGroup);
        }
        else if(controller.MoreOptionsAnim == AnimationStates.FadeOut)
        {
            FadeOut(moreOptionsCanvasGroup);
        }

        if (controller.FocusModeAnim == AnimationStates.FadeIn)
        {
            FadeIn(focusModeCanvasGroup);
        }
        else if (controller.FocusModeAnim == AnimationStates.FadeOut)
        {
            FadeOut(focusModeCanvasGroup);
        }
    }

    private void FadeIn(CanvasGroup group)
    {
        var newAlpha = group.alpha + (Time.deltaTime * controller.animSpeed);
        if (newAlpha > 1)
        {
            newAlpha = 1;
            MakeStandBy(group.gameObject.transform.parent.name);
            AnimQueryResolver();
        }

        group.alpha = newAlpha;
    }

    private void FadeOut(CanvasGroup group)
    {
        var newAlpha = group.alpha - (Time.deltaTime * controller.animSpeed);
        if (newAlpha < 0)
        {
            newAlpha = 0;
            MakeStandBy(group.gameObject.transform.parent.name, false);
            AnimQueryResolver();
        }

        group.alpha = newAlpha;
    }

    private void MakeStandBy(string callerName, bool active = true)
    {
        switch (callerName)
        {
            case "Menu":
                controller.MenuAnim = AnimationStates.StandBy;
                controller.menuUi.SetActive(active);
                break;
            case "ServerConfigScreen":
                controller.ServerConfigAnim = AnimationStates.StandBy;
                controller.serverConfig.SetActive(active);
                break;
            case "MoreOptions":
                controller.MoreOptionsAnim = AnimationStates.StandBy;
                controller.moreOptions.SetActive(active);
                break;
            case "FocusMode":
                controller.FocusModeAnim = AnimationStates.StandBy;
                controller.focusMode.SetActive(active);
                break;
        }
    }

    private void AnimQueryResolver()
    {
        if (controller.NextAnim.Count > 0)
        {
            foreach (var s in controller.NextAnim)
            {
                switch (s)
                {
                    case AnimationFilter.MenuIn:
                        controller.menuUi.SetActive(true);
                        controller.MenuAnim = AnimationStates.FadeIn;
                        break;
                    case AnimationFilter.ServerConfigScreenIn:
                        controller.serverConfig.SetActive(true);
                        controller.ServerConfigAnim = AnimationStates.FadeIn;
                        break;
                    case AnimationFilter.MoreOptionsIn:
                        controller.moreOptions.SetActive(true);
                        controller.MoreOptionsAnim = AnimationStates.FadeIn;                 
                        break;
                    case AnimationFilter.FocusModeIn:
                        controller.focusMode.SetActive(true);
                        controller.FocusModeAnim = AnimationStates.FadeIn;
                        break;
                    case AnimationFilter.Menu:
                        controller.MenuAnim = AnimationStates.FadeOut;
                        break;
                    case AnimationFilter.ServerConfigScreen:
                        controller.ServerConfigAnim = AnimationStates.FadeOut;
                        break;
                    case AnimationFilter.MoreOptions:
                        controller.MoreOptionsAnim = AnimationStates.FadeOut;                 
                        break;
                    case AnimationFilter.FocusMode:
                        controller.FocusModeAnim = AnimationStates.FadeOut;
                        break;
                }
            }

            controller.NextAnim.Clear();
        }
    }
}
