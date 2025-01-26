using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ButtonController : MonoBehaviour 
{
    [SerializeField] Sprite NormalSprite;
    [SerializeField] Sprite PressedSprite;

    [SerializeField] float TimePressing;

    AudioSource ButtonSound;

    private void Start()
    {
        ButtonSound = GetComponent<AudioSource>();
        ButtonSound.volume = 0.5f;
    }

    public void ChangeSprite()
    {
        ButtonSound.Play();
        StartCoroutine(ChangeSpriteForTime());
    }

    IEnumerator ChangeSpriteForTime()
    {
        UIManager.SetImage(gameObject, PressedSprite);
        yield return new WaitForSeconds(TimePressing);
        UIManager.SetImage(gameObject, NormalSprite);
    }
}
