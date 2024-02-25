using TMPro;
using UnityEngine;

public class UIStarCounter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private RectTransform rtContainer;
    private float charSize = 40;
    private float padding = 30;

    private long currentScore = 0;
    private long targetScore;
    // Start is called before the first frame update

    private bool isLerping = false;
    private float lerpTimer = 0f;
    [SerializeField]
    private float lerpDuration = 0.2f;

    void Update()
    {
        if (isLerping)
        {
            lerpTimer += Time.deltaTime;
            long newScore = BigNumber.Lerp(currentScore, targetScore, lerpTimer / lerpDuration);
            if (lerpTimer / lerpDuration >= 1)
            {
                currentScore = targetScore;
                isLerping = false;
                lerpTimer = 0f;
            }
            SetScore(newScore);
        }
    }

    private void SetScore(long newScore)
    {
        scoreText.text = $"{newScore:N0}";
    }

    public void UpdateScore(long newScore)
    {
        if (newScore > currentScore) {
            UIManager.main.ShowPoppingStarText($"+ {newScore - currentScore}");
        }
        targetScore = newScore;
        isLerping = true;
    }

}
