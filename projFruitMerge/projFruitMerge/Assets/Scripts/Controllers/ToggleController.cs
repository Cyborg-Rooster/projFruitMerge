using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ToggleController : MonoBehaviour
{
    [SerializeField] UnityEvent onCustomEvent;

    public bool On;

    Animator Animator;
    AudioSource AudioSource;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Animator = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();
        Animator.speed = 0;
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
        On = true;
        Animator.speed = 1;
        Animator.Play("Base Layer.ToggleOn", 0);
        AudioSource.Play();

        yield return new WaitForSeconds(0.3f);

        Animator.speed = 0;   
        
        TriggerEvent();
    }

    IEnumerator TurnOff()
    {
        On = false;
        Animator.speed = 1;
        Animator.Play("Base Layer.ToggleOff", 0);
        AudioSource.Play();

        yield return new WaitForSeconds(0.3f);

        Animator.speed = 0;

        TriggerEvent();
    }
}
