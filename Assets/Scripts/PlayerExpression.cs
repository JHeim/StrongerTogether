using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerExpression : MonoBehaviour
{
    public PlayerController player;
    public SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.inputMovement < 0)
        {
            player.currentRenderer.flipX = true;
            spriteRenderer.flipX = true;
        }
        else if (player.inputMovement > 0)
        {
            player.currentRenderer.flipX = false;
            spriteRenderer.flipX = false;
        }
    }
}
