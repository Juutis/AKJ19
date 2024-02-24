using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIClickerButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField]
    private Text txtTitle;
    [SerializeField]
    private Image imgBg;

    [SerializeField]
    private Color hoverTextColor;
    [SerializeField]
    private Color clickTextColor;
    private Color normalTextColor;

    [SerializeField]
    private Color hoverBgColor;
    [SerializeField]
    private Color clickBgColor;
    private Color normalBgColor;

    private bool isHeldDown = false;
    private bool isContinuouslyHeld = false;
    private float continuousHoldTimer = 0f;
    [SerializeField]
    private float continuousHoldBoundary = 0.5f;

    [SerializeField]
    private float continuousClickInterval = 0.2f;
    private float continuousClickTimer = 0f;

    private float clickHightlightDuration = 0.1f;

    void Start()
    {
        normalTextColor = txtTitle.color;
        normalBgColor = imgBg.color;
    }

    void Update()
    {
        if (isHeldDown && !isContinuouslyHeld)
        {
            continuousHoldTimer += Time.deltaTime;
            if (continuousHoldTimer > continuousHoldBoundary)
            {
                isContinuouslyHeld = true;
            }
        }
        if (isContinuouslyHeld)
        {
            continuousClickTimer += Time.deltaTime;
            if (continuousClickTimer > continuousClickInterval)
            {
                continuousClickTimer = 0f;
                Click();
            }
        }
    }

    private void ResetHold()
    {
        isHeldDown = false;
        continuousHoldTimer = 0f;
        continuousClickTimer = 0f;
        isContinuouslyHeld = false;
    }

    private void Highlight()
    {
        txtTitle.color = hoverTextColor;
        imgBg.color = hoverBgColor;
    }

    private void HighlightClick()
    {
        txtTitle.color = clickTextColor;
        imgBg.color = clickBgColor;
    }

    private void Unhighlight()
    {
        txtTitle.color = normalTextColor;
        imgBg.color = normalBgColor;
    }

    private void Click()
    {
        HighlightClick();
        if (ClickerManager.main)
        {
            ClickerManager.main.RegisterClick();
        }
        else
        {
            Debug.Log("ClickerManager is missing from scene");
        }
        Invoke("Highlight", clickHightlightDuration);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Button Clicked!");
        Click();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("entered");
        txtTitle.color = hoverTextColor;
        imgBg.color = hoverBgColor;
        Highlight();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetHold();
        Debug.Log("Exited");
        txtTitle.color = normalTextColor;
        imgBg.color = normalBgColor;
        Unhighlight();
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        isHeldDown = true;
        Debug.Log(name + "Game Object Click in Progress");
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        ResetHold();
        Debug.Log(name + "No longer being clicked");
    }
}
public delegate void ButtonClicked();