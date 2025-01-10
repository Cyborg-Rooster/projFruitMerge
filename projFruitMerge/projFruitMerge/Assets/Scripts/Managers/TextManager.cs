using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

static class TextManager
{
    public static void SetText(GameObject gameObject, string text)
    {
        gameObject.GetComponent<Text>().text = text;
    }
}

