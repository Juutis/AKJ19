using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{

    [SerializeField]
    private float totalDistance = 1000.0f;

    private Vector3 origPosition;
    private Vector3 targetPosition;

    private bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(speed*Time.deltaTime, Space.World);
    }

    public void SetPosition(float t) {
        Init();
        transform.position = Vector3.Lerp(origPosition, targetPosition, t);
    }

    private void Init() {
        if (initialized) return;
        origPosition = transform.position;
        targetPosition = transform.position + Vector3.down * totalDistance;
    }
}
