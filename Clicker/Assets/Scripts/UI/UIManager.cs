using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    void Awake()
    {
        main = this;
    }
    public static UIManager main;

    [SerializeField]
    private UIScoreText uiScoreText;
    [SerializeField]
    private GameObject theEndScreen;
    [SerializeField]
    private UIStarCounter uiMoneyText;
    [SerializeField]
    private UIHoverBox uiHoverBox;

    [SerializeField]
    private RectTransform gameRenderBox;

    [SerializeField]
    private UIPoppingText uiPoppingTextPrefab;
    [SerializeField]
    private PoppingTextPool scorePopPool;
    [SerializeField]
    private PoppingTextPool starPopPool;
    [SerializeField]
    private Transform uiPoppingTextContainer;
    [SerializeField]
    private Transform buttonContainer;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Version 1.0");
    }

    public void End() {
        theEndScreen.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSpawnStar) {
            starTimer += Time.deltaTime;
            if (starTimer > starMinInterval) {
                canSpawnStar = true;
            }
        }
    }

    public void ShowHoverBox(string description, string flavor)
    {
        uiHoverBox.Show(description, flavor);
    }

    public void HideHoverBox()
    {
        uiHoverBox.Hide();
    }

    public void AddButton(UIClickerButton button)
    {
        button.transform.SetParent(buttonContainer, false);

        List<UIClickerButton> buttons = new();

        foreach (Transform transform in buttonContainer)
        {
            if (transform.TryGetComponent<UIClickerButton>(out UIClickerButton otherButton))
            {
                buttons.Add(otherButton);
            }
        }

        buttons = buttons.OrderBy(x => new BigNumber(x.UpgradeConfig.scoreRequirement)).ToList();

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].transform.SetSiblingIndex(i);
        }
    }

    [SerializeField]
    private Color starColor;
    [SerializeField]
    private Color scoreColor;

    [SerializeField]
    private float starMinInterval = 0.02f;
    private float starTimer = 0f;
    private bool canSpawnStar = true;

    public void ShowPoppingStarText(string message)
    {
        if (canSpawnStar) {
            starTimer = 0f;
            canSpawnStar = false;
        } else {
            return;
        }
        //UIPoppingText uiPoppingText = Instantiate(uiPoppingTextPrefab, uiPoppingTextContainer);
        UIPoppingText uiPoppingText = starPopPool.GetObject();
        Vector3[] corners = new Vector3[4];
        gameRenderBox.GetWorldCorners(corners);
        Vector3 randomPos = new Vector2(
            Random.Range(corners[0].x, corners[2].x),
            Random.Range(corners[0].y, corners[1].y)
        );

        uiPoppingText.Show(randomPos, message, starColor);
    }

    public void ShowPoppingText(string message, Vector3 position)
    {
        //UIPoppingText uiPoppingText = Instantiate(uiPoppingTextPrefab, uiPoppingTextContainer);
        UIPoppingText uiPoppingText = scorePopPool.GetObject();
        uiPoppingText.Show(position, message, scoreColor);
    }

    public void UpdateMoney(long newMoney)
    {
        uiMoneyText.UpdateScore(newMoney);
    }

    public void UpdateScore(long newScore)
    {
        uiScoreText.UpdateScore(newScore);
    }
}
