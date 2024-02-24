using UnityEngine;

public class ClickerManager : MonoBehaviour
{
    void Awake()
    {
        main = this;
    }
    public static ClickerManager main;

    public void RegisterClick()
    {
        Debug.Log("Click registered");
    }
}
