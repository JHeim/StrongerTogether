using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{


    public float DefaultGravityScale = 0.8f;

    public bool isAttacking;

    public bool inAir;

    PlayerBaseState currentState;

    public bool isGrounded = true;
    public bool wasGrounded = true;

    public float groundCheckRadius = 0.2f;

    public float inputMovement;
    public bool inputJump;

    public AudioSource playerAudioSource;
    public GameObject groundCheck;

    public PlayerExpression playerExpression;

    [SerializeField]
    private Shader _whiteShader;
    private Shader _defaultShader;

    [SerializeField]
    private UnlockableTracker _unlockableTracker = new UnlockableTracker();

    public SpriteRenderer currentRenderer;

    public Animator changeCharacterAnimator;
    public ChangeCharacterFlash changeCharacterFlasher;

    [Header("Bulb Player Settings")]
    public GameObject bulbPlayer;
    public BulbAttack bulbAttack;
    public AudioClip bulbClick;
    public AudioClip bulbPoofSound;
    public AudioClip bulbClickAndPoofSound;

    [Header("Glide Player Settings")]
    public GameObject glidePlayer;
    public AudioClip glideTransformSound;
    public float glideSpeed = 2f;

    [Header("Goop Player Settings")]
    public GameObject goopPlayer;
    public float goopSpeed = 5f;
    public PhysicsMaterial2D goopMaterial;
    public AudioClip goopTransformSound;

    [Header("Spring Player Settings")]
    public GameObject springPlayer;
    public float springSpeed = 5f;
    public float jumpForce = 5f;
    public Vector2 counterJumpForce = new Vector2(0, -20);
    public AudioClip springTransformSound;
    public AudioClip jumpSound;

    public readonly PlayerBulbState bulbState = new PlayerBulbState();
    public readonly PlayerGlideState glideState = new PlayerGlideState();
    public readonly PlayerGoopState goopState = new PlayerGoopState();
    public readonly PlayerSpringState springState = new PlayerSpringState();

    public Rigidbody2D rb = new Rigidbody2D();

    private bool animFinished;

    private IEnumerator coroutine;

    private void Awake()
    {
        //_whiteShader = Shader.Find("Shaders/GUI Text Shader");
        _defaultShader = Shader.Find("Sprites/Default");

        RegisterPlayerStates();

        playerAudioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        Camera.main.transform.SetParent(transform);
    }

    public void RegisterPlayerStates()
    {
        glideState.playerStateManager = glidePlayer.GetComponent<PlayerStateManager>();
        goopState.playerStateManager = goopPlayer.GetComponent<PlayerStateManager>();
        springState.playerStateManager = springPlayer.GetComponent<PlayerStateManager>();
        bulbState.playerStateManager = bulbPlayer.GetComponent<PlayerStateManager>();

    }

    public void ShadeWhite(SpriteRenderer renderer)
    {
        renderer.material.shader = _whiteShader;
        renderer.color = Color.white;
    }

    public void ShadeNormal(SpriteRenderer renderer)
    {
        renderer.material.shader = _defaultShader;
        renderer.color = Color.white;
    }

    public void PlaySound(AudioClip sound)
    {
        playerAudioSource.clip = sound;
        playerAudioSource.PlayOneShot(sound);
    }

    // Start is called before the first frame update
    void Start()
    {

        TransitionToStateInstantly(springState);

        currentState.Start(this);
    }

    private void FixedUpdate()
    {
        wasGrounded = isGrounded;
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, groundCheckRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (!colliders[i].tag.Equals(Globals.Tags.Player))
            {
                isGrounded = true;
                if (!wasGrounded)
                {
                    // Land.
                }
            }
        }

        currentState.FixedUpdate(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var unlockerVolume = collision.gameObject.GetComponent<UnlockerScript>();

        if (unlockerVolume != null)
        {
            Debug.Log("Entered Unlocker Volume of " + unlockerVolume.name);
            Type unlockedStateType = unlockerVolume.GetUnlockable();
            _unlockableTracker.UnlockCharacter(unlockedStateType);
        }
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

    public void OnChangeCharacterPoint()
    {

    }

    public void OnChangeCharacterFinished()
    {
        animFinished = true;
    }

    public void TransitionToStateDelayed(PlayerBaseState state)
    {
        if (CheckIfCanTransitionToState(state))
        {
            rb.gravityScale = DefaultGravityScale;
            animFinished = false;
            DisableOldState(state);
        }
    }

    public bool CheckIfCanTransitionToState(PlayerBaseState state)
    {
        bool result = false;

        if (_unlockableTracker.IsCharacterUnlocked(state.GetType()))
        {
            result = true;
        }

        return result;
    }

    public void TransitionToStateInstantly(PlayerBaseState state)
    {
        rb.gravityScale = DefaultGravityScale;
        animFinished = false;
        if (currentState != null)
        {
            currentState.playerStateManager.DisableCharacter(currentState.playerStateManager.gameObject);
        }

        currentState = state;
        currentState.EnterState(this);

        currentState.playerStateManager.EnableCharacter(currentState.playerStateManager.gameObject);
    }

    private IEnumerator WaitAndRevert()
    {
        yield return new WaitForSeconds(.2f);
        ShadeNormal(currentState.playerStateManager.spriteRenderer);
        playerExpression.gameObject.SetActive(true);
    }

    private IEnumerator WaitAndDisable(PlayerBaseState newState)
    {
        yield return new WaitForSeconds(.1f);
        currentState.playerStateManager.DisableCharacter(currentState.playerStateManager.gameObject);
        EnableNewState(newState);

    }

    private void EnableNewState(PlayerBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
        //currentState.playerStateManager.gameObject.SetActive(true);
        currentState.playerStateManager.EnableCharacter(currentState.playerStateManager.gameObject);
        ShadeWhite(currentState.playerStateManager.spriteRenderer);

        coroutine = WaitAndRevert();
        StartCoroutine(coroutine);
    }

    private void DisableOldState(PlayerBaseState newState)
    {
        if (currentState != null)
        {

            playerExpression.gameObject.SetActive(false);
            ShadeWhite(currentState.playerStateManager.spriteRenderer);
            coroutine = WaitAndDisable(newState);
            StartCoroutine(coroutine);
        }
    }

    private void OnDestroy()
    {
        currentState.OnDestroy(this);
    }

}
