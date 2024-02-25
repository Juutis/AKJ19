using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private UIStarCounter uiMoneyText;
    [SerializeField]
    private UIHoverBox uiHoverBox;

    [SerializeField]
    private UIPoppingText uiPoppingTextPrefab;
    [SerializeField]
    private Transform uiPoppingTextContainer;
    [SerializeField]
    private Transform buttonContainer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

    public void ShowPoppingText(string message, Vector3 position)
    {
        UIPoppingText uiPoppingText = Instantiate(uiPoppingTextPrefab, uiPoppingTextContainer);
        uiPoppingText.Show(position, message);
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
