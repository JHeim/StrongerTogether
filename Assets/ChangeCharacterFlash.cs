using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class ChangeCharacterFlash : MonoBehaviour
{
    public Animator animator;

    public UnityEvent OnFlashStarted { get; set; } = new UnityEvent();
    public UnityEvent OnChangeCharacterPoint { get; set; } = new UnityEvent();
    public UnityEvent OnFlashFinished { get; set; } = new UnityEvent();

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FlashStarted()
    {
        OnFlashStarted?.Invoke();
    }

    public void ChangeCharacterPoint()
    {
        OnChangeCharacterPoint?.Invoke();
    }


    public void FlashFinished()
    {
        OnFlashFinished?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
