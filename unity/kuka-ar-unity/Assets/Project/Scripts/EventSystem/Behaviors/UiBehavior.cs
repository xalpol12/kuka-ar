using Project.Scripts.EventSystem.Enums;
using UnityEngine;

public class UiBehavior : MonoBehaviour
{
    private UiController controller;
    private CanvasGroup serverCanvasGroup;
    private CanvasGroup menuCanvasGroup;
    private CanvasGroup moreOptionsCanvasGroup;
    void Start()
    {
        controller = GetComponent<UiController>();

        serverCanvasGroup = controller.serverConfig.transform.Find("Canvas").GetComponent<CanvasGroup>();
        menuCanvasGroup = controller.menuUi.transform.Find("Canvas").GetComponent<CanvasGroup>();
        moreOptionsCanvasGroup = controller.moreOptions.transform.Find("Canvas").GetComponent<CanvasGroup>();
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
    }

    private void FadeIn(CanvasGroup group)
    {
        var newAlpha = group.alpha + (Time.deltaTime * controller.AnimSpeed);
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
        var newAlpha = group.alpha - (Time.deltaTime * controller.AnimSpeed);
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
                    case "MenuIn":
                        controller.menuUi.SetActive(true);
                        controller.MenuAnim = AnimationStates.FadeIn;
                        break;
                    case "ServerConfigScreenIn":
                        controller.serverConfig.SetActive(true);
                        controller.ServerConfigAnim = AnimationStates.FadeIn;
                        break;
                    case "MoreOptionsIn":
                        controller.moreOptions.SetActive(true);
                        controller.MoreOptionsAnim = AnimationStates.FadeIn;                 
                        break;
                    case "Menu":
                        controller.MenuAnim = AnimationStates.FadeOut;
                        break;
                    case "ServerConfigScreen":
                        controller.ServerConfigAnim = AnimationStates.FadeOut;
                        break;
                    case "MoreOptions":
                        controller.MoreOptionsAnim = AnimationStates.FadeOut;                 
                        break;
                }
            }

            controller.NextAnim.Clear();
        }
    }
}
