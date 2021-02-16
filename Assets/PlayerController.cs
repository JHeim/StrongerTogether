using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public bool inAir;

    PlayerBaseState currentState;

    public bool isGrounded = true;

    public float inputMovement;
    public bool inputJump;

    [Header("Glide Player Settings")]
    public GameObject glidePlayer;

    [Header("Goop Player Settings")]
    public GameObject goopPlayer;
    public float goopSpeed = 5f;
    public PhysicsMaterial2D goopMaterial;

    [Header("Spring Player Settings")]
    public GameObject springPlayer;
    public float springSpeed = 5f;
    public float jumpForce = 5f;
    public Vector2 counterJumpForce = new Vector2(0, -20);

    public readonly PlayerGlideState glideState = new PlayerGlideState();
    public readonly PlayerGoopState goopState = new PlayerGoopState();
    public readonly PlayerSpringState springState = new PlayerSpringState();

    public Rigidbody2D rb = new Rigidbody2D();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        TransitionToState(springState);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter2D(this, collision);
    }

    // Update is called once per frame
    void Update()
    {
        inputMovement = Input.GetAxis(Globals.Input.Horizontal);
        inputJump = Input.GetButtonDown(Globals.Input.Jump);

        currentState.Update(this);
    }

    public void TransitionToState(PlayerBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
