using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    Animator Animator;
    private void Start()
    {
        Animator = GetComponent<Animator>();

        Animator.speed = 0;
    }

    public IEnumerator FadeIn()
    {
        Animator.speed = 1;
        Animator.Play("Base Layer.FadeIn", 0);

        yield return new WaitForSeconds(0.571f);

        Animator.speed = 0;
    }

    public IEnumerator FadeOut()
    {
        Animator.speed = 1;
        Animator.Play("Base Layer.FadeOut", 0);

        yield return new WaitForSeconds(0.571f);

        Animator.speed = 0;
        gameObject.SetActive(false);
    }
}
