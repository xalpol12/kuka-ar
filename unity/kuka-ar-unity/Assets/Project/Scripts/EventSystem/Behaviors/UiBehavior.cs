using System.Collections;
using Project.Scripts.EventSystem.Enums;
using UnityEngine;

namespace Project.Scripts.EventSystem.Behaviors
{
    private UiController controller;
    private CanvasGroup serverCanvasGroup;
    private CanvasGroup menuCanvasGroup;
    private CanvasGroup moreOptionsCanvasGroup;
    private CanvasGroup focusModeCanvasGroup;
    private GameObject serverBackArrow;
    private void Start()
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

    private void Update()
    {
        if (controller.ServerConfigAnim == AnimationStates.FadeIn)
        {
            StartCoroutine(FadeIn(serverCanvasGroup));
        }

        private void FadeIn(CanvasGroup group)
        {
            StartCoroutine(FadeOut(serverCanvasGroup));
        }
        
        if (controller.MenuAnim == AnimationStates.FadeIn)
        {
            StartCoroutine(FadeIn(menuCanvasGroup));
        }
        else if(controller.MenuAnim == AnimationStates.FadeOut)
        {
            StartCoroutine(FadeOut(menuCanvasGroup));
        }
        
        if (controller.MoreOptionsAnim == AnimationStates.FadeIn)
        {
            StartCoroutine(FadeIn(moreOptionsCanvasGroup));
        }
        else if(controller.MoreOptionsAnim == AnimationStates.FadeOut)
        {
            StartCoroutine(FadeOut(moreOptionsCanvasGroup));
        }

        if (controller.FocusModeAnim == AnimationStates.FadeIn)
        {
            StartCoroutine(FadeIn(focusModeCanvasGroup));
        }
        else if (controller.FocusModeAnim == AnimationStates.FadeOut)
        {
            StartCoroutine(FadeOut(focusModeCanvasGroup));
        }
        
        if (Input.GetKey(KeyCode.Escape))
        {
            if (controller.moreOptions.activeSelf)
            {
                controller.MoreOptionsAnim = AnimationStates.FadeOut;
                controller.NextAnim.Add(AnimationFilter.MenuIn);
                StartCoroutine(FadeOut(moreOptionsCanvasGroup));
            } else if (controller.serverConfig.activeSelf)
            {
                controller.ServerConfigAnim = AnimationStates.FadeOut;
                controller.NextAnim.Add(AnimationFilter.MoreOptionsIn);
                StartCoroutine(FadeOut(serverCanvasGroup));
            }
            else if (controller.focusMode.activeSelf)
            {
                controller.MenuAnim = AnimationStates.FadeOut;
                controller.NextAnim.Add(AnimationFilter.MoreOptionsIn);
                StartCoroutine(FadeOut(focusModeCanvasGroup));
            }
        }
    }

    private IEnumerator FadeIn(CanvasGroup group)
    {
        var newAlpha = group.alpha + (Time.deltaTime * controller.animSpeed);
        if (newAlpha > 1)
        {
            group.alpha = 1;
            MakeStandBy(group.gameObject.transform.parent.name);
            AnimQueryResolver();
            yield break;
        }

        group.alpha = newAlpha;
        yield return null;
    }

    private IEnumerator FadeOut(CanvasGroup group)
    {
        var newAlpha = group.alpha - (Time.deltaTime * controller.animSpeed);
        if (newAlpha < 0)
        {
            group.alpha = 0;
            MakeStandBy(group.gameObject.transform.parent.name, false);
            AnimQueryResolver();
            yield break;
        }

        group.alpha = newAlpha;
        yield return null;
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

        private void AnimQueryResolver()
        {
            if (controller.NextAnim.Count > 0)
            {
                foreach (var s in controller.NextAnim)
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

                controller.NextAnim.Clear();
            }
        }
    }
}
