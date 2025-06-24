using UnityEngine;

public class GUIManager : MonoBehaviour
{
    // [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject panel;
    // [SerializeField] private float animationSpeed = 5f;
    // [SerializeField] private RectTransform panelRect;

    // private Vector2 hiddenPosition;
    // private Vector2 visiblePosition;
    private bool isPanelActive = false;
    private bool activated = false;
    public Animator panelAnimator;

    private void Start()
    {
        panel.SetActive(false);
        isPanelActive = false;
    }

    // private void Awake()
    // {
        // hiddenPosition = new Vector2(0, -panelRect.rect.height);
        // visiblePosition = Vector2.zero;

        // panelRect.anchoredPosition = hiddenPosition;
    // }

    public void PanelActivate(bool flag)
    {
        Activate();
        //
        // Vector2 targetPosition = isPanelActive ? visiblePosition : hiddenPosition;
        // panelRect.anchoredPosition = Vector2.Lerp(
        //     panelRect.anchoredPosition,
        //     targetPosition,
        //     Time.unscaledDeltaTime * animationSpeed
        // );
        if (flag)
        {
            panelAnimator.SetBool("slide", true);
        }
        else
        {
            panelAnimator.SetBool("slide", false);
        }
        //
    }

    private void Activate()
    {
        isPanelActive = !isPanelActive;
        if (isPanelActive)
        {
            panel.SetActive(true);
        }
        Time.timeScale = isPanelActive ? 0 : 1;
    }
}