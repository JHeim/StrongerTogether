using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public PlayerBaseState[] characterStack  = new PlayerBaseState[4];

    public void StackCharacter(PlayerBaseState character)
    {
        if (character.stackIndex > 0)
        {
            for (int i = character.stackIndex; i > 0; i--)
            {
                characterStack[i] = characterStack[i - 1];
                characterStack[i].stackIndex = i;
            }
            character.stackIndex = 0;
            characterStack[0] = character;
        }
    }

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

    private Shader _whiteShader;
    private Shader _defaultShader;

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

    private void Awake()
    {
        _whiteShader = Shader.Find("Shaders/GUI Text Shader");
        _defaultShader = Shader.Find("Sprites/Default");

        characterStack[0] = springState;
        characterStack[1] = goopState;
        characterStack[2] = bulbState;
        characterStack[3] = glideState;

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

        TransitionToState(springState);

        //changeCharacterFlasher.OnFlashFinished.AddListener(OnChangeCharacterFinished);

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

    public void TransitionToState(PlayerBaseState state)
    {
        if (state.playerStateManager.gameObject.activeSelf)
        {
            rb.gravityScale = DefaultGravityScale;
            animFinished = false;
            //ShadeWhite(currentRenderer);

            //changeCharacterFlasher.animator.SetTrigger(Globals.Animation.Flash);
            if (currentState != null)
            {

                //currentState.playerStateManager.gameObject.SetActive(false);
                //currentState.playerStateManager.transform.position = new Vector3(transform.position.x - currentState.stackIndex, transform.position.y, transform.position.z);
                currentState.playerStateManager.DisableCharacter(currentState.playerStateManager.gameObject);
            }

            // Move the character to the top of the stack.
            StackCharacter(state);

            // Update the positions of all characters.
            foreach (var character in characterStack)
            {
                character.playerStateManager.gameObject.transform.position = new Vector3(transform.position.x - character.stackIndex, transform.position.y, transform.position.z);
            }

            currentState = state;
            currentState.EnterState(this);
            //currentState.playerStateManager.gameObject.SetActive(true);
            currentState.playerStateManager.EnableCharacter(currentState.playerStateManager.gameObject);
            //ShadeNormal(currentRenderer);
        }
    }

    private void OnDestroy()
    {
        currentState.OnDestroy(this);
    }

    private IEnumerator WaitForAnim(PlayerBaseState state)
    {
        yield return animFinished;
    }
}
