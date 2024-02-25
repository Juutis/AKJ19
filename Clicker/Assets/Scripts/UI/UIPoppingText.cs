using TMPro;
using UnityEngine;

public class UIPoppingText : MonoBehaviour
{

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private TextMeshProUGUI txtMessage;

    private float yOffset = 20f;
    private float randomXFactor = 20f;
    private float randomYFactor = 5f;
    PoppingTextPool pool;

    public void Init(PoppingTextPool pool) {
        this.pool = pool;
    }

    public void Show(Vector3 position, string message, Color color)
    {
        float randomX = Random.Range(-randomXFactor, randomXFactor);
        float randomY = Random.Range(randomYFactor, randomYFactor * 2);
        transform.position = new Vector3(position.x + randomX, position.y + yOffset + randomY, position.z);
        txtMessage.text = message;
        txtMessage.color = color;
        animator.SetTrigger("Show");
    }

    public void ShowFinished()
    {
        //Destroy(gameObject);
        pool.ReturnObject(this);
    }
}

