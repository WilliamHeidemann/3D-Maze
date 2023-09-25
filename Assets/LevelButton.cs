using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    private Image _image;
    
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        _image.color = ColorPicker.complementaryColor;
    }
}
