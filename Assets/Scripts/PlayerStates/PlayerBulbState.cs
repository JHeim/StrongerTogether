using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulbState : PlayerBaseState
{

    private bool _startedAttack = false;

    public override PlayerStateManager playerStateManager { get; set; }
    public override int stackIndex { get; set; } = 2;

    public Vector3 expressionPosition = new Vector3(-0.15f, 0.01f, 0f);


    //private float _gravityScale;

    public override void EnterState(PlayerController player)
    {
        player.playerExpression.transform.position = player.transform.position + expressionPosition;
        player.isAttacking = true;
        _startedAttack = false;
        
        //player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

        player.PlaySound(player.bulbClickAndPoofSound);
       

        
    }

    private void Attack()
    {

        
    }

    public override void FixedUpdate(PlayerController player)
    {

    }

    public override void OnCollisionEnter2D(PlayerController player, Collision2D collision)
    {
        
    }

    public void OnAttackAnimStart()
    {
    }

    public void OnAttackAnimEnd()
    {
        _startedAttack = false;
    }

    public override void Update(PlayerController player)
    {
        if (player.isAttacking && !_startedAttack)
        {
            _startedAttack = true;
            playerStateManager.animator.SetTrigger(Globals.Animation.IsAttacking);
        }

        if (!player.isAttacking)
        {
            //player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (player.inputMovement > 0.1 || player.inputMovement < -0.1)
            {
                player.TransitionToStateDelayed(player.springState);
            }

            if (Input.GetButtonDown(Globals.Input.Goop))
            {

                //player.rb.AddForce(new Vector2(0, 0.01f));
                player.TransitionToStateDelayed(player.goopState);
            }

            if (Input.GetButtonDown(Globals.Input.Jump))
            {
                //player.rb.AddForce(new Vector2(0, 0.01f));
                player.TransitionToStateDelayed(player.springState);
                player.springState.JumpCheck(player);
            }

            if (Input.GetButtonDown(Globals.Input.Glide) && player.inAir)
            {

                //player.rb.AddForce(new Vector2(0, 0.01f));
                player.TransitionToStateDelayed(player.glideState);
            }

            if (Input.GetButtonDown(Globals.Input.Attack))
            {
                player.isAttacking = true;
                player.PlaySound(player.bulbPoofSound);
                playerStateManager.animator.SetTrigger(Globals.Animation.IsAttacking);
            }
        }
    }

    public override void OnDestroy(PlayerController player)
    {
    }

    public override void Start(PlayerController player)
    {
    }
}
