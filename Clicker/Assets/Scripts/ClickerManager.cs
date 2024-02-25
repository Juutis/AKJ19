using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ClickerManager : MonoBehaviour
{
    [SerializeField]
    private GameConfig gameConfig;
    [SerializeField]
    private List<UpgradeConfig> allUpgrades = new();
    private List<UpgradeConfig> boughtUpgrades = new();
    private List<UIClickerButton> upgradeButtons = new();

    void Awake()
    {
        main = this;
    }
    public static ClickerManager main;

    private BigNumber mainScore;
    private BigNumber clickPower = new(123);
    private BigNumber money;
    private BigNumber noDomeMaxScore;
    private BigNumber passiveScoreIncrease = new(0);

    private int additionalClickers = 0;
    private float clickFrequency = 1;
    private float lastClick = 0;
    private bool hasDome = false;
    private bool clickHoldEnabled = false;
    private float starFrequency = 0.1f;
    private float lastStar = 10f;

    public bool ClickHoldEnabled { get { return clickHoldEnabled; } }
    private int starValue;

    [SerializeField]
    private UIClickerButton clickerButtonPrefab;

    private float upgradeCheckTimer = 0f;
    private float upgradeCheckInterval = 1f;
    private Vector3 prevClickPos = Vector3.zero;

    private void Start()
    {
        mainScore = new(0);
        money = new(gameConfig.InitialMoney);
        clickPower.value = gameConfig.InitialClickAmount;

        clickFrequency = gameConfig.InitialClickHoldFrequency;
        noDomeMaxScore = new(gameConfig.NoDomeMaxScore);
        starValue = gameConfig.StarValue;
        UIManager.main.UpdateMoney(money.value);
    }

    private void Update()
    {
        if (!hasDome && mainScore.CompareTo(noDomeMaxScore) >= 0)
        {
            mainScore.value = noDomeMaxScore.value;
        }

        if (Time.time - lastClick >= (1f / clickFrequency) && additionalClickers > 0)
        {
            System.Numerics.BigInteger increment = mainScore.IncrementValue(clickPower, additionalClickers);
            lastClick = Time.time;
            UpdateScore();
            UIManager.main.ShowPoppingText($"+{increment:N0}", prevClickPos);
        }

        if (passiveScoreIncrease.CompareTo(0) > 0)
        {
            BigNumber scorePerFrame = BigNumber.Multiply(passiveScoreIncrease, Time.deltaTime);
            mainScore.Increase(scorePerFrame);
        }

        upgradeCheckTimer += Time.deltaTime;
        if (upgradeCheckTimer > upgradeCheckInterval)
        {
            upgradeCheckTimer = 0f;
            CheckUpgrades();
        }

        if (starFrequency != 0 && Time.time - lastStar > (1 / starFrequency))
        {
            GetStar();
            lastStar = Time.time;
        }
    }

    private void UpdateScore()
    {
        UIManager.main.UpdateScore(mainScore.value);
        GraphicsManager.Main.SetHeight((double)mainScore.value);
    }

    public void RegisterClick(ClickerAction action, Vector3 position, ClickData data = null)
    {
        if (action == ClickerAction.NumberGoUp)
        {
            System.Numerics.BigInteger increment = mainScore.IncrementValue(clickPower);
            UIManager.main.ShowPoppingText($"+{increment:N0}", position);
            UpdateScore();
            prevClickPos = position;
        }
        if (action == ClickerAction.BuyUpgrade)
        {
            BuyUpgrade(data);
            return;
        }
    }

    private void BuyUpgrade(ClickData data)
    {
        if (data == null || string.IsNullOrWhiteSpace(data.ResourceName))
        {
            Debug.LogError("Trying to buy an upgrade without ClickData!");
            return;
        }

        UpgradeConfig upgrade = allUpgrades.FirstOrDefault(x => x.UpgradeName == data.ResourceName);

        if (upgrade == null)
        {
            Debug.LogError("Trying to buy an upgrade that doesn't exist in ClickerManager allUpgrades!");
            return;
        }

        if (money.CompareTo(upgrade.moneyRequirement) < 0)
        {
            Debug.LogWarning("Trying to buy an upgrade that is too expensive! Should be blocked by UI");
            return;
        }

        money.Decrease(upgrade.moneyRequirement);
        UIManager.main.UpdateMoney(money.value);
        boughtUpgrades.Add(upgrade);

        additionalClickers += upgrade.additionalClickersAdded;
        if (upgrade.additionalClickMultiplier > 0)
        {
            additionalClickers *= Mathf.Max(upgrade.additionalClickMultiplier, 1);
        }
        clickFrequency = Mathf.Max(upgrade.clickHoldFrequency, clickFrequency);
        if (upgrade.clickAmountMultiplier > 0)
        {
            clickPower.Multiply(upgrade.clickAmountMultiplier);
        }
        clickPower.Increase(upgrade.clickValueAddition);
        hasDome |= upgrade.isDome;
        clickHoldEnabled |= upgrade.clickHoldEnabled;
        passiveScoreIncrease.Increase(upgrade.passiveScoreIncrease);
        starValue += upgrade.starCaughtValueAddition;
        starFrequency += upgrade.starCaughtFrequencyAddition;
    }

    public string GetScore()
    {
        return mainScore.GetUIValue();
    }

    private void CheckUpgrades()
    {
        IEnumerable<UpgradeConfig> newUpgrades = allUpgrades
            .Where(x => !boughtUpgrades.Select(x => x.UpgradeName).Contains(x.UpgradeName))
            .Where(x => !upgradeButtons.Any(y => y.UpgradeConfig == x))
            // .Where(x => mainScore.CompareTo(x.scoreRequirement) >= 0)
            .Where(x => x.requiredUpgrades == null ||
                x.requiredUpgrades.Select(y => y.UpgradeName).All(y => boughtUpgrades.Select(z => z.UpgradeName).Contains(y))
            )
            .OrderBy(x => x.scoreRequirement);
        foreach (UpgradeConfig config in newUpgrades)
        {
            UIClickerButton newButton = Instantiate(clickerButtonPrefab);
            newButton.InitUpgradeButton(config);
            UIManager.main.AddButton(newButton);
            upgradeButtons.Add(newButton);
        }
        foreach (UIClickerButton button in upgradeButtons)
        {
            if (button.IsHidden)
            {
                continue;
            }
            if (button.IsDisabled && money.CompareTo(button.UpgradeConfig.moneyRequirement) >= 0 && mainScore.CompareTo(button.UpgradeConfig.scoreRequirement) >= 0)
            {
                button.Enable();
            }
            else if (!button.IsDisabled && money.CompareTo(button.UpgradeConfig.moneyRequirement) == -1 && mainScore.CompareTo(button.UpgradeConfig.scoreRequirement) == -1)
            {
                button.Disable();
            }
        }
    }

    public IEnumerable<UpgradeConfig> VisibleUpgrades()
    {
        return allUpgrades
            .Where(x => !boughtUpgrades.Select(x => x.UpgradeName).Contains(x.UpgradeName))
            .Where(x => mainScore.CompareTo(x.scoreRequirement) >= 0)
            .Where(x =>
                x.requiredUpgrades.Select(y => y.UpgradeName).All(y => boughtUpgrades.Select(z => z.UpgradeName).Contains(y))
            );
    }

    public IEnumerable<UpgradeConfig> BuyableUpgrades()
    {
        return VisibleUpgrades()
            .Where(x => money.CompareTo(x.moneyRequirement) >= 0);
    }

    public void GetStar()
    {
        money.Increase(starValue);
        UIManager.main.UpdateMoney(money.value);
    }
}

public enum ClickerAction
{
    None,
    NumberGoUp,
    BuyUpgrade
}

public class ClickData
{
    public string ResourceName { get; set; }
}
