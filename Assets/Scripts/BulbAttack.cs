using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulbAttack : MonoBehaviour
{
    public PlayerController player;
    public UnityEvent OnAttackStart { get; private set; } = new UnityEvent();
    public UnityEvent OnAttackEnd { get; private set; } = new UnityEvent();

    public void AttackStart()
    {
        OnAttackStart?.Invoke();
    }

    public void AttackEnd()
    {
        player.isAttacking = false;
        OnAttackEnd?.Invoke();
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
