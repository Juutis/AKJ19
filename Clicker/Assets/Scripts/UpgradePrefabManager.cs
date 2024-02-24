using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradePrefabManager : MonoBehaviour
{
    [SerializeField]
    private List<NameGameobjectPair> upgradePrefabs;

    void Awake()
    {
        main = this;
    }
    public static UpgradePrefabManager main;

    public void ShowUpgrade(UpgradePrefab upgrade)
    {
        GameObject gameObject = upgradePrefabs.FirstOrDefault(x => x.Name == upgrade).GameObject;
        if (gameObject != null)
        {
            gameObject.SetActive(true);
        }
    }
}

[Serializable]
public class NameGameobjectPair
{
    public UpgradePrefab Name;
    public GameObject GameObject;
}

public enum UpgradePrefab
{
    Balloon,
    RocketEngines,
    Dome
}