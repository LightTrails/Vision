using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class MainImage : MonoBehaviour
{
    public Texture2D[] Textures;
    public string[] Effects = { "Normal", "GrayScale", "GaussianBlur", "Otzu", "OtzuRevert" };

    public Material GrayScale;
    public Material Color;

    [DllImport("ImageSegmentation")]
    private static extern IntPtr otsuBinarization(byte[] data, int width, int height, int blur = 5, bool revert = false);

    [DllImport("ImageSegmentation")]
    private static extern IntPtr gaussianBlur(byte[] data, int width, int height, int blur = 5);

    [DllImport("ImageSegmentation")]
    private static extern IntPtr grayScale(byte[] data, int width, int height);

    private ImageSelector _imageSelector;
    private EffectSelector _effectSelector;
    private Histogram _histogram;
    private BlurSlider _blurSlider;

    private void Start()
    {
        _imageSelector.SelectIndex(0);
        _effectSelector.SelectIndex(0);
    }

    private void Awake()
    {
        _imageSelector = FindObjectOfType<ImageSelector>();
        _effectSelector = FindObjectOfType<EffectSelector>();
        _histogram = FindObjectOfType<Histogram>();
        _blurSlider = FindObjectOfType<BlurSlider>();
    }

    public void UpdateImage()
    {
        if (_imageSelector == null || _imageSelector.SelectedTexture == null)
        {
            return;
        }

        Texture2D texture = _imageSelector.SelectedTexture;

        if (_effectSelector.SelectedEffect == Effects[0])
        {
            SetNormal(texture);
        }
        else if (_effectSelector.SelectedEffect == Effects[1])
        {
            SetGrayScale(texture);
        }
        else if (_effectSelector.SelectedEffect == Effects[2])
        {
            SetGaussian(texture);
        }
        else if (_effectSelector.SelectedEffect == Effects[3])
        {
            SetOtzu(texture, false);
        }
        else if (_effectSelector.SelectedEffect == Effects[4])
        {
            SetOtzu(texture, true);
        }
    }

    public void SetNormal(Texture2D texture)
    {
        var rawImage = GetComponent<RawImage>();
        rawImage.material = Color;
        rawImage.texture = texture;
        texture.Apply();

        _histogram.SetHistogram(new byte[0]);

        rawImage.SetNativeSize();
    }

    public void SetGrayScale(Texture2D texture)
    {
        var rawTexture = texture.GetRawTextureData();
        IntPtr pointer = grayScale(rawTexture, texture.width, texture.height);
        var result = ExtractData(pointer, texture.width, texture.height);
        _histogram.SetHistogram(result);
        SetImage(texture, result);
    }

    public void SetGaussian(Texture2D texture)
    {
        var rawTexture = texture.GetRawTextureData();
        IntPtr pointer = gaussianBlur(rawTexture, texture.width, texture.height, _blurSlider.SliderValue);
        var result = ExtractData(pointer, texture.width, texture.height);
        _histogram.SetHistogram(result);
        SetImage(texture, result);
    }

    public void SetOtzu(Texture2D texture, bool revert)
    {
        var rawTexture = texture.GetRawTextureData();
        IntPtr pointer = otsuBinarization(rawTexture, texture.width, texture.height, _blurSlider.SliderValue, revert);
        var result = ExtractData(pointer, texture.width, texture.height);
        _histogram.SetHistogram(result);
        SetImage(texture, result);
    }

    private void SetImage(Texture2D texture, byte[] result)
    {
        var grayScale = new Texture2D(texture.width, texture.height, TextureFormat.Alpha8, false);
        grayScale.LoadRawTextureData(result);

        var rawImage = GetComponent<RawImage>();
        rawImage.material = GrayScale;
        rawImage.texture = grayScale;
        grayScale.Apply();
        rawImage.SetNativeSize();
    }

    private unsafe byte[] ExtractData(IntPtr pointer, int width, int height)
    {
        int size = width * height;
        byte[] managedArray = new byte[size];
        Marshal.Copy(pointer, managedArray, 0, size);

        return managedArray;
    }
}
