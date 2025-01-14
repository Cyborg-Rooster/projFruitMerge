using System.Collections;
using UnityEngine;

public class ToggleController : MonoBehaviour
{

    public bool On;

    Animator Animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Animator = GetComponent<Animator>();
        Animator.speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (On) StartCoroutine(TurnOff());
        else StartCoroutine(TurnOn());
    }

    IEnumerator TurnOn()
    {
        Animator.speed = 1;
        Animator.Play("Base Layer.ToggleOn", 0);

        yield return new WaitForSeconds(0.3f);

        On = true;
        Animator.speed = 0;    
    }

    IEnumerator TurnOff()
    {
        Animator.speed = 1;
        Animator.Play("Base Layer.ToggleOff", 0);

        yield return new WaitForSeconds(0.3f);

        On = false;
        Animator.speed = 0;
    }
}
