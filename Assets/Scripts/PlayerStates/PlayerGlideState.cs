using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlideState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {

    }

    public override void FixedUpdate(PlayerController player)
    {
        
    }

    public override void OnCollisionEnter2D(PlayerController player, Collision2D collision)
    {
    }

    public override void Update(PlayerController player)
    {
        if (Input.GetButtonUp(Globals.Input.Glide))
        {
            player.TransitionToState(player.springState);
        }
    }
}
