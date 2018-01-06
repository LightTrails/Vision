using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ImageSelector : MonoBehaviour
{
    public Texture2D SelectedTexture;

    private MainImage _mainImage;

    // Use this for initialization
    void Awake()
    {
        _mainImage = FindObjectOfType<MainImage>();
        var dropdown = GetComponent<Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(_mainImage.Textures.Select(x => x.name).ToList());
        dropdown.onValueChanged.AddListener(SelectIndex);
    }

    public void SelectIndex(int index)
    {
        SelectedTexture = _mainImage.Textures[index];
        _mainImage.UpdateImage();
    }
}
