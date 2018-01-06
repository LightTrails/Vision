using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlurSlider : MonoBehaviour
{
    private MainImage _mainImage;

    public int SliderValue;

    void Awake()
    {
        _mainImage = FindObjectOfType<MainImage>();
        var slider = GetComponent<Slider>();
        SliderValue = (int)slider.value;
        slider.onValueChanged.AddListener(SliderChanged);
    }

    private void SliderChanged(float newValue)
    {
        int newIntValue = (int)newValue;

        if (newIntValue % 2 == 0)
        {
            newIntValue++;
        }

        SliderValue = newIntValue;
        _mainImage.UpdateImage();
    }
}
