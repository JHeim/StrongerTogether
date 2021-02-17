using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
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

    [Header("Glide Player Settings")]
    //public GameObject glidePlayer;
    public AudioClip glideTransformSound;

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

    public readonly PlayerGlideState glideState = new PlayerGlideState();
    public readonly PlayerGoopState goopState = new PlayerGoopState();
    public readonly PlayerSpringState springState = new PlayerSpringState();

    public Rigidbody2D rb = new Rigidbody2D();

    private bool animFinished;

    private void Awake()
    {
        _whiteShader = Shader.Find("Shaders/GUI Text Shader");
        _defaultShader = Shader.Find("Sprites/Default");

        RegisterPlayerStates();

        playerAudioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        TransitionToState(springState);
    }

    public void RegisterPlayerStates()
    {
        //glideState.playerStateManager = glidePlayer.GetComponent<PlayerStateManager>();
        goopState.playerStateManager = goopPlayer.GetComponent<PlayerStateManager>();
        springState.playerStateManager = springPlayer.GetComponent<PlayerStateManager>();

        
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
        changeCharacterFlasher.OnFlashFinished.AddListener(OnChangeCharacterFinished);
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
        animFinished = false;
        //ShadeWhite(currentRenderer);

        //changeCharacterFlasher.animator.SetTrigger(Globals.Animation.Flash);
        if (currentState!= null)
        {

            currentState.playerStateManager.gameObject.SetActive(false);
        }
        currentState = state;
        currentState.EnterState(this);
        currentState.playerStateManager.gameObject.SetActive(true);

        //ShadeNormal(currentRenderer);
    }

    private IEnumerator WaitForAnim(PlayerBaseState state)
    {
        yield return animFinished;
    }
}
