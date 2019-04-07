using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CircleController : MonoBehaviour
{
    public GameManager Controller { get; set; }

    public Color Color
    {
        get => GetComponent<SpriteRenderer>().color;
        set => GetComponent<SpriteRenderer>().color = value;
    }

    void Update()
    {
        transform.localScale += Vector3.one * Controller.GrowthSpeed * Time.deltaTime;
        if (transform.localScale.x < 0)
        {
            Destroy(gameObject);
            Controller.CircleDestroed();
        }
        if (transform.localScale.x > 5)
            Controller.GameOver();
    }

    public void OnMouseDown()
    {
        transform.localScale -= Vector3.one * Controller.Decrement;
        Controller.CircleHit();
    }
}
