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

    private Color _activeColor = new Color(255, 255, 255);
    //private Color _inActiveColor = new Color(77, 77, 77, 100);
    // It's a multiplier???? 
    private Color _inActiveColor = new Color(0.3f, 0.3f, 0.3f);

    public void DisableCharacter(GameObject character)
    {
        character.SetActive(false);

        // Change character position.
        //float positionOffset = Random.Range(1, 6);
        //character.transform.position = new Vector3(player.transform.position.x - stackIndex, player.transform.position.y, player.transform.position.z);

        //// Disable colliders.
        //foreach (var collider in myColliders)
        //{
        //    collider.enabled = false;
        //}

        //// Change order in layer.
        //spriteRenderer.sortingOrder = 1;

        //// Change coloring.
        //spriteRenderer.color = _inActiveColor;
    }

    public void EnableCharacter(GameObject character)
    {
        character.SetActive(true);

        //// Move character to front.
        //character.transform.position = player.transform.position;

        //// Enable Colliders.
        //foreach (var collider in myColliders)
        //{
        //    collider.enabled = true;
        //}

        //// Revert sorting.
        //spriteRenderer.sortingOrder = 2;

        //// Revert coloring.
        //spriteRenderer.color = _activeColor;
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
