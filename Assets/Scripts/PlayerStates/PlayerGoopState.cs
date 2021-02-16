using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoopState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        player.rb.sharedMaterial = player.goopMaterial;
        player.goopPlayer.SetActive(true);
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
            player.goopPlayer.SetActive(false);
            player.TransitionToState(player.springState);
        }

        if (Input.GetButtonDown(Globals.Input.Jump))
        {

            player.goopPlayer.SetActive(false);
            player.TransitionToState(player.springState);
            player.springState.JumpCheck(player);
        }

        if (Input.GetButtonDown(Globals.Input.Glide) && player.inAir)
        {
            player.goopPlayer.SetActive(false);
            player.TransitionToState(player.glideState);
        }
    }
}