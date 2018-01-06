using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Histogram : MonoBehaviour
{
    public int Width = 256;
    public int Height = 256;

    public void SetHistogram(byte[] bytes)
    {
        var grayScale = new Texture2D(Width, Height, TextureFormat.Alpha8, false);

        float[] histogram = new float[256];

        for (int i = 0; i < histogram.Length; i++)
        {
            histogram[i] = 0;
        }

        for (int i = 0; i < bytes.Length; i++)
        {
            histogram[bytes[i]]++;
        }

        var max = histogram.Max();

        for (int i = 0; i < histogram.Length; i++)
        {
            histogram[i] = histogram[i] / max;
        }

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                grayScale.SetPixel(i, j, new Color()
                {
                    a = j < (histogram[i] * Height) ? 1.0f : 0.0f
                });
            }
        }

        var rawImage = GetComponent<RawImage>();
        rawImage.texture = grayScale;
        grayScale.Apply();
        rawImage.SetNativeSize();
    }
}
