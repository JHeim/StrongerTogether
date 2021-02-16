using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpringState : PlayerBaseState
{
    public bool jumpKeyHeld;
    public bool isJumping;


    public override void EnterState(PlayerController player)
    {
        player.rb.sharedMaterial = null;
        player.springPlayer.SetActive(true);
    }

    public override void FixedUpdate(PlayerController player)
    {
        //player.rb.MovePosition(
        //        player.transform.position +
        //        new Vector3(player.inputMovement, 0) * player.springSpeed * Time.fixedDeltaTime
        //        );

        player.rb.velocity = new Vector2(player.inputMovement * player.springSpeed, player.rb.velocity.y);

        if (isJumping)
        {
            if (!jumpKeyHeld && Vector2.Dot(player.rb.velocity, Vector2.up) > 0)
            {
                player.rb.AddForce(player.counterJumpForce * player.rb.mass);
            }
            //player.rb.AddForce(new Vector2(player.inputMovement * player.springSpeed, 0));

        }

    }

    public override void OnCollisionEnter2D(PlayerController player, Collision2D collision)
    {
        isJumping = false;
        player.isGrounded = true;
    }

    // Check if colliding with anything to determine if in air.
    public override void Update(PlayerController player)
    {
        if (Input.GetButtonDown(Globals.Input.Goop))
        {
            player.springPlayer.SetActive(false);
            player.TransitionToState(player.goopState);
        }

        JumpCheck(player);
    }

    public void JumpCheck(PlayerController player)
    {
        if (Input.GetButtonDown(Globals.Input.Jump))
        {
            jumpKeyHeld = true;
            //if (player.isGrounded)
            {
                isJumping = true;
                player.rb.AddForce(Vector2.up * player.jumpForce * player.rb.mass, ForceMode2D.Impulse);
                //player.rb.AddForce(Vector2.up * player.jumpForce);
            }
        }
        else if (Input.GetButtonUp(Globals.Input.Jump))
        {
            jumpKeyHeld = false;
        }
    }
}
