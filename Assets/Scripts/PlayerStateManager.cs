using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerStateManager : MonoBehaviour
{
    public PlayerController player;
    public Collider2D[] myColliders;

    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public void DisableCharacter(GameObject character)
    {
        character.SetActive(false);
    }

    public void EnableCharacter(GameObject character)
    {
        character.SetActive(true);
    }


    private void Awake()
    {
        myColliders = GetComponents<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
