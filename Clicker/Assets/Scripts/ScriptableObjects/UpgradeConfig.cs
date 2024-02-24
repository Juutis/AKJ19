using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Configs/UpgradeConfig")]
public class UpgradeConfig : ScriptableObject
{
    public UpgradeType UpgradeType;
    public string UpgradeName;
    [TextArea]
    public string Description;
    [TextArea]
    public string LoreText;
    public Sprite Icon;
    [Header("Requirements")]
    public string moneyRequirement;
    public string scoreRequirement;
    public List<UpgradeConfig> requiredUpgrades;
    [Header("Upgrade effects")]
    public float clickAmountMultiplier;
    public int additionalClickersAdded;
    public int additionalClickMultiplier;
    public float clickHoldFrequency;
}

public enum UpgradeType
{
    ClickUpgrade
}