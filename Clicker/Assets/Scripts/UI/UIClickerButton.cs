using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIClickerButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    //[SerializeField]
    //private Text txtTitle;
    [SerializeField]
    private TextMeshProUGUI txtTitle;
    [SerializeField]
    private GameObject costContainer;
    [SerializeField]
    private TextMeshProUGUI txtCost;
    [SerializeField]
    private UIScoreText scoreReq;
    [SerializeField]
    private Image imgBg;

    [SerializeField]
    private Color bgColor;
    [SerializeField]
    private Color textColor = Color.white;
    [SerializeField]
    private Color disabledBgColor;
    [SerializeField]
    private Color disabledTextColor;

    [SerializeField]
    private Color hoverTextColor;
    [SerializeField]
    private Color clickTextColor;

    [SerializeField]
    private Color hoverBgColor;
    [SerializeField]
    private Color clickBgColor;

    [SerializeField]
    private Image imgStar;

    private bool isDisabled = false;
    public bool IsDisabled { get { return isDisabled; } }

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

    private bool isHidden = false;
    public bool IsHidden { get { return isHidden; } }

    public void InitUpgradeButton(UpgradeConfig config)
    {
        Disable();
        upgradeConfig = config;
        clickerAction = ClickerAction.BuyUpgrade;

        costContainer.gameObject.SetActive(true);
        txtCost.text = $"{config.moneyRequirement:N0}";
        txtTitle.text = config.UpgradeName.ToUpper();
        RectTransform rt = imgStar.rectTransform;
        float xOffset = txtCost.text.Length * txtCost.fontSize / 2f;
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x + Mathf.Clamp(xOffset, txtCost.fontSize, 999f), rt.anchoredPosition.y);
        long req;
        long.TryParse(config.scoreRequirement, out req);
        scoreReq.SetScore(req);
    }

    public void Disable()
    {
        imgBg.color = disabledBgColor;
        txtTitle.color = disabledTextColor;
        isDisabled = true;
    }

    public void Enable()
    {
        imgBg.color = bgColor;
        txtTitle.color = textColor;
        isDisabled = false;
    }

    public void Hide()
    {
        isHidden = true;
        gameObject.SetActive(false);
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
        if (clickerAction == ClickerAction.BuyUpgrade)
        {
            UIManager.main.ShowHoverBox(upgradeConfig.Description, upgradeConfig.LoreText);
        }
        if (isHidden)
        {
            UIManager.main.HideHoverBox();
        }
        if (isDisabled)
        {
            return;
        }
        txtTitle.color = hoverTextColor;
        imgBg.color = hoverBgColor;
    }

    private void HighlightClick()
    {
        if (isDisabled)
        {
            return;
        }
        txtTitle.color = clickTextColor;
        imgBg.color = clickBgColor;
    }

    private void Unhighlight()
    {
        if (isDisabled)
        {
            return;
        }
        txtTitle.color = textColor;
        imgBg.color = bgColor;
    }

    private void Click()
    {
        if (isDisabled)
        {
            return;
        }
        HighlightClick();
        if (ClickerManager.main)
        {
            //Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (clickerAction == ClickerAction.NumberGoUp)
            {
                ClickerManager.main.RegisterClick(clickerAction, Input.mousePosition);
            }
            else if (clickerAction == ClickerAction.BuyUpgrade)
            {
                bool successfullyBought = ClickerManager.main.RegisterClick(clickerAction, Input.mousePosition, new() { ResourceName = upgradeConfig.UpgradeName });
                if (!successfullyBought) {
                    return;
                }
                Hide();
                UIManager.main.HideHoverBox();
                SoundManager.main.PlaySound(GameSoundType.Upgrade);
            }
        }
        else
        {
            Debug.LogWarning("ClickerManager is missing from scene");
        }

        Invoke("Highlight", clickHightlightDuration);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Click();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Highlight();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetHold();
        Unhighlight();
        UIManager.main.HideHoverBox();
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        isHeldDown = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        ResetHold();
    }
}
public delegate void ButtonClicked();