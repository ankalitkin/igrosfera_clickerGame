using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _saturation = 0.9f;
    [SerializeField, Range(0, 1)] private float _vibrance = 0.9f;
    [SerializeField, Range(0, 1)] private float _offset = 0;
    [SerializeField, Range(0, 360)] private int _nuberOfColors = 12;

    public Color NewColor()
    {
        float hue = (float)Random.Range(0, _nuberOfColors) / _nuberOfColors + _offset;
        return Color.HSVToRGB(hue, _saturation, _vibrance);
    }
}
