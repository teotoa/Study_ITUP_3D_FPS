using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GuageColor : MonoBehaviour
{
    Image image;


    void Start()
    {
        image = GetComponent<Image>();
    }


    void Update()
    {
        image.color = Color.HSVToRGB(image.fillAmount / 3, 1, 1);
    }
}
