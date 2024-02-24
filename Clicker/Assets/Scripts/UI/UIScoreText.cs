using UnityEngine;
using UnityEngine.UI;

public class UIScoreText : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private RectTransform rtContainer;
    private float charSize = 40;
    private float padding = 20;

    private System.Numerics.BigInteger currentScore = 0;
    private System.Numerics.BigInteger targetScore;
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
            System.Numerics.BigInteger newScore = BigNumber.Lerp(currentScore, targetScore, lerpTimer / lerpDuration);
            if (lerpTimer / lerpDuration >= 1)
            {
                currentScore = targetScore;
                isLerping = false;
                lerpTimer = 0f;
            }
            SetScore(newScore);
        }
    }

    private void SetScore(System.Numerics.BigInteger newScore)
    {
        scoreText.text = $"{newScore:N0}";
        rtContainer.sizeDelta = new Vector2(
            scoreText.text.Length * charSize + padding * 2,
            rtContainer.sizeDelta.y
        );
    }

    public void UpdateScore(System.Numerics.BigInteger newScore)
    {
        targetScore = newScore;
        isLerping = true;
    }
    /*
    public void UpdateScore(string newScore)
    {
        scoreText.text = newScore;
        rtContainer.sizeDelta = new Vector2(
            scoreText.text.Length * charSize + padding * 2,
            rtContainer.sizeDelta.y
        );
    }*/
}
