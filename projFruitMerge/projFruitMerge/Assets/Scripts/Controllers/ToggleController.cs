using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ToggleController : MonoBehaviour
{
    [SerializeField] UnityEvent onCustomEvent;

    public bool On;

    Animator Animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Turn();
    }

    public void Turn()
    {
        Animator = GetComponent<Animator>();
        Animator.speed = 0;

        if (On) StartCoroutine(TurnOff());
        else StartCoroutine(TurnOn());
    }

    public void TriggerEvent()
    {
        onCustomEvent?.Invoke();
    }

    IEnumerator TurnOn()
    {
        Animator.speed = 1;
        Animator.Play("Base Layer.ToggleOn", 0);

        yield return new WaitForSeconds(0.3f);

        On = true;
        Animator.speed = 0;   
        
        TriggerEvent();
    }

    IEnumerator TurnOff()
    {
        Animator.speed = 1;
        Animator.Play("Base Layer.ToggleOff", 0);

        yield return new WaitForSeconds(0.3f);

        On = false;
        Animator.speed = 0;

        TriggerEvent();
    }
}
