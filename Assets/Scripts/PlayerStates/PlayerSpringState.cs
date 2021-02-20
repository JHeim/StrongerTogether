using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpringState : PlayerBaseState
{
    public override int stackIndex { get; set; } = 0;

    public bool jumpKeyHeld;
    public bool isJumping;

    public Vector3 expressionPosition = Vector3.zero;

    public override PlayerStateManager playerStateManager { get; set; }

    public override void EnterState(PlayerController player)
    {
        player.playerExpression.transform.position = player.transform.position + expressionPosition;
        player.rb.sharedMaterial = null;
        //player.springPlayer.SetActive(true);

        player.currentRenderer = player.springPlayer.GetComponent<SpriteRenderer>();
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
                playerStateManager.animator.SetBool(Globals.Animation.IsJumping, true);
            }

        }

    }

    public override void OnCollisionEnter2D(PlayerController player, Collision2D collision)
    {
        isJumping = false;
        player.isGrounded = true;
        playerStateManager.animator.SetBool(Globals.Animation.IsJumping, false);
    }

    // Check if colliding with anything to determine if in air.
    public override void Update(PlayerController player)
    {
        if (Input.GetButtonDown(Globals.Input.Goop))
        {
            //player.springPlayer.SetActive(false);
            player.TransitionToState(player.goopState);
        }

        if (Input.GetButtonDown(Globals.Input.Attack))
        {
            player.TransitionToState(player.bulbState);
        }

        if (Input.GetButtonDown(Globals.Input.Glide))
        {
            player.TransitionToState(player.glideState);
        }

        playerStateManager.animator.SetFloat(Globals.Animation.MoveX, player.inputMovement);

        JumpCheck(player);
    }

    public void JumpCheck(PlayerController player)
    {
        if (Input.GetButtonDown(Globals.Input.Jump))
        {
            jumpKeyHeld = true;
            if (player.isGrounded)
            {
                player.PlaySound(player.jumpSound);
                isJumping = true;
                playerStateManager.animator.SetBool(Globals.Animation.IsJumping, true);
                player.rb.AddForce(Vector2.up * player.jumpForce * player.rb.mass, ForceMode2D.Impulse);
                //player.rb.AddForce(Vector2.up * player.jumpForce);
            }
        }
        else if (Input.GetButtonUp(Globals.Input.Jump))
        {
            jumpKeyHeld = false;
        }
    }

    public override void OnDestroy(PlayerController player)
    {
        
    }

    public override void Start(PlayerController player)
    {
        
    }
}
