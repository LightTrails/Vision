using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EffectSelector : MonoBehaviour
{
    public string SelectedEffect;

    private MainImage _mainImage;

    // Use this for initialization
    void Awake()
    {
        _mainImage = FindObjectOfType<MainImage>();
        var dropdown = GetComponent<Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(_mainImage.Effects.ToList());
        dropdown.onValueChanged.AddListener(SelectIndex);
    }

    public void SelectIndex(int index)
    {
        SelectedEffect = _mainImage.Effects[index];
        _mainImage.UpdateImage();
    }
}
