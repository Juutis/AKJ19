using TMPro;
using UnityEngine;

public class UIHoverBox : MonoBehaviour
{
    [SerializeField]
    private GameObject container;
    [SerializeField]
    private TextMeshProUGUI txtDescription;
    [SerializeField]
    private TextMeshProUGUI txtFlavor;
    [SerializeField]
    private RectTransform rt;

    private float padding = 20f;
    private float minHeight = 80f;
    private float maxHeight = 240f;

    public void Hide()
    {
        container.SetActive(false);
    }

    public void Show(string description, string flavor)
    {
        txtDescription.text = description;
        Vector2 preferred = txtDescription.GetPreferredValues(description, rt.sizeDelta.x - padding * 2, maxHeight);
        //Rect rect = txtDescription.rectTransform.rect;
        //rt.sizeDelta = new Vector2(rect.width, rect.height + padding * 2);
        float textBoxHeight = Mathf.Clamp(preferred.y + padding * 3, minHeight, maxHeight);
        txtFlavor.text = $"~{flavor}~";
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, textBoxHeight);
        container.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(Input.mousePosition.x - padding, Input.mousePosition.y);
    }
}
