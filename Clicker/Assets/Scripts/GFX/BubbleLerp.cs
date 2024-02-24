using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;

public class BubbleLerp : MonoBehaviour
{
    private Material mat;
    private float spawned;
    private float lerpDuration = 10.0f;
    private float lerpStart = 3.0f;
    private Color origColor;
    private Color targetColor;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        spawned = Time.time;
        origColor = new Color(mat.color.r, mat.color.g, mat.color.b, 1.0f);
        targetColor = new Color(mat.color.r, mat.color.g, mat.color.b, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - spawned < lerpStart) {
            mat.SetColor("_BaseColor", origColor);
        } else if (Time.time - spawned < lerpDuration + lerpStart) {
            var t = (Time.time - spawned - lerpStart) / lerpDuration;
            mat.SetColor("_BaseColor", Color.Lerp(origColor, targetColor, t));
        } else {
            mat.SetColor("_BaseColor", targetColor);
        }
    }
}
