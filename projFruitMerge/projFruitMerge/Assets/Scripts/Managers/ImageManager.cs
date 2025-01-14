using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

public static class ImageManager
{
    public static void SetImage(GameObject gameObject, Sprite sprite)
    {
        var i = gameObject.GetComponent<Image>();

        i.sprite = sprite;
        i.SetNativeSize();
    }
}
