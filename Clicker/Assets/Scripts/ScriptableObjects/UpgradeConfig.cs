using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Configs/UpgradeConfig")]
public class UpgradeConfig : ScriptableObject
{
    public UpgradeType UpgradeType;
    [TextArea]
    public string Description;
    [TextArea]
    public string LoreText;
    public int moneyRequirement;
    public int heightRequirement;
    public List<UpgradeConfig> requiredUpgrades;
    public float clickAmountMultiplier;
    public int additionalClickersAdded;
    public int additionalClickMultiplier;
    public float clickHoldFrequency;
    public Sprite Icon;
}

public enum UpgradeType
{
    ClickUpgrade
}