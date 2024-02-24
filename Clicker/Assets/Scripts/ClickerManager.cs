using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClickerManager : MonoBehaviour
{
    [SerializeField]
    private GameConfig gameConfig;
    [SerializeField]
    private List<UpgradeConfig> allUpgrades;
    private List<UpgradeConfig> boughtUpgrades;

    void Awake()
    {
        main = this;
    }
    public static ClickerManager main;

    private BigNumber mainScore;
    private BigNumber clickPower = new(123);
    private BigNumber money;
    private BigNumber domeHeight;
    private BigNumber height = new(0);
    private BigNumber speed = new(0);

    private int additionalClickers = 0;
    private float clickFrequency = 1;
    private float lastClick = 0;
    private bool hasDome = false;
    private bool clickHoldEnabled = false;
    private int starValue;
   

    private void Start()
    {
        mainScore = new(0);
        clickPower.value = gameConfig.InitialClickAmount;
        clickFrequency = gameConfig.InitialClickHoldFrequency;
        domeHeight.Set(gameConfig.NoDomeMaxHeight);
        starValue = gameConfig.StarValue;
    }

    private void Update()
    {
        if (Time.time - lastClick >= (1f / clickFrequency))
        {
            mainScore.IncrementValue(clickPower, additionalClickers);
        }

        if (hasDome || height.CompareTo(domeHeight) < 0)
        {
            BigNumber speedPerFrame = BigNumber.Multiply(speed, Time.deltaTime);
            height.Increase(speedPerFrame);
        }
    }

    public void RegisterClick(ClickerAction action, ClickData data = null)
    {
        if (action == ClickerAction.NumberGoUp)
        {
            mainScore.IncrementValue(clickPower);
        }
        if (action == ClickerAction.BuyUpgrade)
        {
            BuyUpgrade(data);
            return;
        }

        Debug.Log("Click registered");
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
        boughtUpgrades.Add(upgrade);

        additionalClickers += upgrade.additionalClickersAdded;
        additionalClickers *= Mathf.Max(upgrade.additionalClickMultiplier, 1);
        clickFrequency = Mathf.Max(upgrade.clickHoldFrequency, clickFrequency);
        clickPower.Multiply(upgrade.clickAmountMultiplier);
        hasDome |= upgrade.isDome;
        clickHoldEnabled |= upgrade.clickHoldEnabled;
    }

    public string GetScore()
    {
        return mainScore.GetUIValue();
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
