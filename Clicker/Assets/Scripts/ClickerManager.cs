using System.Collections.Generic;
using UnityEngine;

public class ClickerManager : MonoBehaviour
{
    private List<UpgradeConfig> allUpgrades;
    private List<UpgradeConfig> boughtUpgrades;

    void Awake()
    {
        main = this;
    }
    public static ClickerManager main;

    private BigNumber mainScore;

    private void Start()
    {
        mainScore = new();
    }

    public void RegisterClick(ClickerAction action)
    {
        if (action == ClickerAction.NumberGoUp)
        {
            mainScore.IncrementValue();
        }

        Debug.Log("Click registered");
    }

    public string GetScore()
    {
        return mainScore.GetUIValue();
    }
}

public enum ClickerAction
{
    None,
    NumberGoUp,
    BuyUpgrade
}
