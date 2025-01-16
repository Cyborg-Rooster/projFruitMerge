using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

static class UIManager
{
    public static void SetText(GameObject gameObject, object text)
    {
        gameObject.GetComponent<Text>().text = text.ToString();
    }

    public static void SetImage(GameObject gameObject, Sprite sprite)
    {
        var i = gameObject.GetComponent<Image>();

        i.sprite = sprite;
        i.SetNativeSize();
    }

    public static void SetButtonEnable(GameObject gameObject, bool enable)
    {
        gameObject.GetComponent<Button>().enabled = enable;
    }
}

