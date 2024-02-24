using UnityEngine;

public class ClickerManager : MonoBehaviour
{
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
        return mainScore.GetNumber();
    }
}

public enum ClickerAction
{
    None,
    NumberGoUp
}
