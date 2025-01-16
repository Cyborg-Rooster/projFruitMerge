using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ButtonImageController : MonoBehaviour 
{
    [SerializeField] Sprite NormalSprite;
    [SerializeField] Sprite PressedSprite;

    [SerializeField] float TimePressing;

    public void ChangeSprite()
    {
        StartCoroutine(ChangeSpriteForTime());
    }

    IEnumerator ChangeSpriteForTime()
    {
        UIManager.SetImage(gameObject, PressedSprite);
        yield return new WaitForSeconds(TimePressing);
        UIManager.SetImage(gameObject, NormalSprite);
    }
}
