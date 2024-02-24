using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    [field: SerializeField]
    public int InitialClickHoldFrequency { get; private set; }
    [field: SerializeField]
    public int InitialClickAmount { get; private set; }
    [field: SerializeField]
    public string NoDomeMaxScore { get; private set; }
    [field: SerializeField]
    public int StarValue { get; private set; }
}
