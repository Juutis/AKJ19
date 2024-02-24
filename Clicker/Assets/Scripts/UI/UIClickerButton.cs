using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIClickerButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField]
    private string title;
    //[SerializeField]
    //private Text txtTitle;
    [SerializeField]
    private TextMeshProUGUI txtTitle;
    [SerializeField]
    private GameObject costContainer;
    [SerializeField]
    private TextMeshProUGUI txtCost;
    [SerializeField]
    private Image imgBg;

    [SerializeField]
    private Color disabledBgColor;
    [SerializeField]
    private Color disabledTextColor;

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

    [SerializeField]
    private ClickerAction clickerAction;

    [SerializeField]
    private UpgradeConfig upgradeConfig;
    public UpgradeConfig UpgradeConfig { get { return upgradeConfig; } }

    [SerializeField]
    private int cost = 0;

    public void Init(UpgradeConfig config)
    {
        upgradeConfig = config;
        clickerAction = ClickerAction.BuyUpgrade;
    }

    void Start()
    {
        normalTextColor = txtTitle.color;
        normalBgColor = imgBg.color;
        txtTitle.text = title;
        if (clickerAction == ClickerAction.BuyUpgrade)
        {
            costContainer.gameObject.SetActive(true);
            txtCost.text = $"{cost:N0}";
        }
        /*if (requirements not met) {
            txtTitle.color = disabledTextColor;
            txtCost.color = disabledTextColor;
            imgBg.color = disabledBgColor;
        }
        */
    }

    void Update()
    {
        if (clickerAction != ClickerAction.NumberGoUp || !ClickerManager.main.ClickHoldEnabled)
        {
            return;
        }
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
            //Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ClickerManager.main.RegisterClick(ClickerAction.NumberGoUp, Input.mousePosition);
        }
        else
        {
            Debug.Log("ClickerManager is missing from scene");
        }

        Invoke("Highlight", clickHightlightDuration);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Button Clicked!");
        Click();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("entered");
        txtTitle.color = hoverTextColor;
        imgBg.color = hoverBgColor;
        Highlight();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetHold();
        //Debug.Log("Exited");
        txtTitle.color = normalTextColor;
        imgBg.color = normalBgColor;
        Unhighlight();
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        isHeldDown = true;
        //Debug.Log(name + "Game Object Click in Progress");
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        ResetHold();
        //Debug.Log(name + "No longer being clicked");
    }
}
public delegate void ButtonClicked();