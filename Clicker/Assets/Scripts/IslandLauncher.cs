using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandLauncher : MonoBehaviour
{
    private bool launched = false;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (launched) {
            transform.Translate(Vector3.up * Time.deltaTime * 10.0f, Space.World);
            transform.Rotate(new Vector3(1.0f, 1.0f, 1.0f), Time.deltaTime * 180.0f);
        }
    }

    public void Launch() {
        launched = true;
    }
}
