using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulbState : PlayerBaseState
{
    public override PlayerStateManager playerStateManager { get; set; }


    public Vector3 expressionPosition = new Vector3(-0.15f, 0.01f, 0f);

    public bool isAttacking { get; private set; } = false;

    public override void EnterState(PlayerController player)
    {
        player.playerExpression.transform.position = player.transform.position + expressionPosition;

    }

    public override void FixedUpdate(PlayerController player)
    {
        
    }

    public override void OnCollisionEnter2D(PlayerController player, Collision2D collision)
    {
        
    }

    public override void Update(PlayerController player)
    {
        if (!isAttacking)
        {
            if (player.inputMovement > 0.1 || player.inputMovement < -0.1)
            {
                player.TransitionToState(player.springState);
            }

            if (Input.GetButtonDown(Globals.Input.Goop))
            {
                player.TransitionToState(player.goopState);
            }

            if (Input.GetButtonDown(Globals.Input.Jump))
            {
                player.TransitionToState(player.springState);
                player.springState.JumpCheck(player);
            }

            if (Input.GetButtonDown(Globals.Input.Glide) && player.inAir)
            {
                player.TransitionToState(player.glideState);
            }
        }
    }
}
