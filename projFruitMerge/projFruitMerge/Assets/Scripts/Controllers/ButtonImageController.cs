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
        ImageManager.SetImage(gameObject, PressedSprite);
        yield return new WaitForSeconds(TimePressing);
        ImageManager.SetImage(gameObject, NormalSprite);
    }
}
