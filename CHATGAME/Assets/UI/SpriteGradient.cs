using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteGradient : MonoBehaviour
{
    public Gradient gradient;

    [Range(0, 1)]
    public float t;

    public Image img;

    void Start()
    {
        img = transform.GetComponent<Image>();
    }

    void Update()
    {
        img.color = gradient.Evaluate(t);
    }
}
