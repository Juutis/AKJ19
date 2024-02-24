using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    void Awake()
    {
        main = this;
    }
    public static UIManager main;

    [SerializeField]
    private UIScoreText uIScoreText;

    [SerializeField]
    private UIPoppingText uiPoppingTextPrefab;
    [SerializeField]
    private Transform uiPoppingTextContainer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowPoppingText(string message, Vector3 position)
    {
        UIPoppingText uiPoppingText = Instantiate(uiPoppingTextPrefab, uiPoppingTextContainer);
        uiPoppingText.Show(position, message);
    }

    public void UpdateScore(System.Numerics.BigInteger newScore)
    {
        uIScoreText.UpdateScore(newScore);
    }
}
