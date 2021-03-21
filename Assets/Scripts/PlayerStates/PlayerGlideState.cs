using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlideState : PlayerBaseState
{
    public override int stackIndex { get; set; } = 3;
    public override PlayerStateManager playerStateManager { get; set; }

    public Vector3 expressionPosition = new Vector3(-0.12f, 0.13f, 0f);

    public override void EnterState(PlayerController player)
    {
        player.rb.gravityScale = 0.4f;
        player.playerExpression.transform.position = player.transform.position + expressionPosition;
        player.PlaySound(player.glideTransformSound);
    }

    public override void FixedUpdate(PlayerController player)
    {
        if (!player.isGrounded)
        {
            player.rb.velocity = new Vector2(player.inputMovement * player.glideSpeed, player.rb.velocity.y);
        }
    }

    public override void OnCollisionEnter2D(PlayerController player, Collision2D collision)
    {
    }

    public override void OnDestroy(PlayerController player)
    {
        
    }

    public override void Start(PlayerController player)
    {
        
    }

    public override void Update(PlayerController player)
    {
        if (Input.GetButtonDown(Globals.Input.Glide))
        {
            player.TransitionToStateDelayed(player.springState);
        }

        if (Input.GetButtonDown(Globals.Input.Attack))
        {
            player.TransitionToStateDelayed(player.bulbState);
        }

        if (Input.GetButtonDown(Globals.Input.Goop))
        {
            player.TransitionToStateDelayed(player.goopState);
        }
    }


}
