using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoopState : PlayerBaseState
{
    public override int stackIndex { get; set; } = 1;
    public override PlayerStateManager playerStateManager { get; set; }

    public Vector3 expressionPosition = new Vector3(0.07f, -0.3f, 0f);
    public override void EnterState(PlayerController player)
    {
        player.playerExpression.transform.position = player.transform.position + expressionPosition;
        player.PlaySound(player.goopTransformSound);
        //player.rb.sharedMaterial = player.goopMaterial;
        //player.goopPlayer.SetActive(true);
    }

    public override void FixedUpdate(PlayerController player)
    {
        player.rb.velocity = new Vector2(player.inputMovement * player.goopSpeed, player.rb.velocity.y);

    }

    public override void OnCollisionEnter2D(PlayerController player, Collision2D collision)
    {

    }

    public override void Update(PlayerController player)
    {
        if (Input.GetButtonDown(Globals.Input.Goop))
        {
            //player.goopPlayer.SetActive(false);
            player.TransitionToStateDelayed(player.springState);
        }

        if (Input.GetButtonDown(Globals.Input.Jump))
        {

            //player.goopPlayer.SetActive(false);
            player.TransitionToStateDelayed(player.springState);
            player.springState.JumpCheck(player);
        }

        if (Input.GetButtonDown(Globals.Input.Attack))
        {
            player.TransitionToStateDelayed(player.bulbState);
        }

        if (Input.GetButtonDown(Globals.Input.Glide) && player.inAir)
        {
            //player.goopPlayer.SetActive(false);
            player.TransitionToStateDelayed(player.glideState);
        }
    }

    public override void OnDestroy(PlayerController player)
    {

    }

    public override void Start(PlayerController player)
    {

    }
}
