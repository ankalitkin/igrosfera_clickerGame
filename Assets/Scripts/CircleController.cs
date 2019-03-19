using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CircleController : MonoBehaviour
{
    public GameManager Сontroller { get; set; }

    public Color Color
    {
        get => GetComponent<SpriteRenderer>().color;
        set => GetComponent<SpriteRenderer>().color = value;
    }

    void Update()
    {
        transform.localScale += Vector3.one * Сontroller.GrowthSpeed * Time.deltaTime;
        if (transform.localScale.x < 0)
        {
            Destroy(gameObject);
            Сontroller.Score += 10;
        }
        if (transform.localScale.x > 5)
            Сontroller.GameOver();
    }

    public void OnMouseDown()
    {
        transform.localScale -= Vector3.one * Сontroller.Decrement;
        Сontroller.Score++;
    }
}
