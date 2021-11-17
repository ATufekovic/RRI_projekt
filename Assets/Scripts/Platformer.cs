using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class Platformer : MonoBehaviour
{
    Rigidbody2D rigidbody2DPlayer;
    public float movementSpeed = 2.0f;
    public float jumpingPower = 2.0f;
    public float airControlModifier = 0.5f;
    public float maxAirSpeed = 2.0f;
    public Text deathsText;
    //public float wallJumpPower = 1.0f;

    private float x;
    private int deaths = 0;
    private KeyCode jumpKeyCode;

    private bool isGrounded = false;
    private bool isTouchingLeftWall = false;
    private bool isTouchingRightWall = false;
    private bool deathFlag = false;

    //colliders for checking ground/death_ground
    private Vector2 capsuleSize;
    private Collider2D colliderGround;
    private Collider2D colliderWallLeft;
    private Collider2D colliderWallRight;
    private Collider2D colliderCrushed;

    private Animator playerAnimator;

    public Transform isTouchingGroundedChecker;
    public Transform isTouchingWallLeftChecker;
    public Transform isTouchingWallRightChecker;
    public Transform isCrushedChecker;
    public Transform spawnPoint;
    public float checkGroundRadius = 0.1f;
    public float checkGroundBottomLength = 0.25f;
    public LayerMask groundLayer;
    public LayerMask deathLayer;

    public AudioClip audioJump;
    public AudioClip audioLand;
    public AudioClip audioDeath;
    private AudioSource audioSource;
    [Range(0.0f, 1.0f)]
    public float audioVolume = 1.0f;
    private int audioVolumeGlobal;
    /// <summary>
    /// Used to check if the player has jumped to play landing audio only once
    /// </summary>
    private bool hasJumped = false;

    void Start()
    {
        audioVolumeGlobal = PlayerPrefs.GetInt("VolumeEffects", 75);
        audioSource = gameObject.GetComponent<AudioSource>();
        capsuleSize = new Vector2(checkGroundBottomLength, checkGroundRadius);
        rigidbody2DPlayer = GetComponent<Rigidbody2D>();
        //load the jump key from playerprefs
        jumpKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", KeyCode.Space.ToString()));
        playerAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckIfTouchingDeathGround();
        CheckIfTouchingValidGround();
        PlaySounds();
        Jump();
        UpdateAnimations();
        StartCoroutine(CheckForEndScene());
    }

    private IEnumerator CheckForEndScene()
    {
        if (SceneManager.GetActiveScene().name == "EndScene")
        {
            int totalDeaths = PlayerPrefs.GetInt("Deaths", 0);
            if (totalDeaths == 0)
            {
                deathsText.text = "You have never died during the game, congratulations original!";
            }
            else if (totalDeaths > 0 && totalDeaths <= 5)
            {
                deathsText.text = "You have died " + totalDeaths.ToString() + " times, you sacrificed some toys to get here but good job otherwise!";
            }
            else if (totalDeaths > 5 && totalDeaths <= 20)
            {
                deathsText.text = "You have died " + totalDeaths.ToString() + " times, that's a lot of broken toys that replaced you...";
            }
            else if (totalDeaths > 20)
            {
                deathsText.text = "Oof, you have died " + totalDeaths.ToString() + " times, you could have probably done better and broke less toys...";
            }
        }
        yield return new WaitForSeconds(1.0f);
    }

    private void PlaySounds()
    {
        //if a player walks off an edge play the landing sound
        if (rigidbody2DPlayer.velocity.y < 0.5)
            StartCoroutine(TagPlayerMidAirForLandingSound());

        if ((isGrounded) && hasJumped)
            StartCoroutine(PlaySoundIfLanding());
    }
    private IEnumerator PlaySoundIfLanding()
    {
        if ((isGrounded) && hasJumped)
        {
            audioSource.PlayOneShot(audioLand, audioVolume * (audioVolumeGlobal / 100.0f));
            hasJumped = false;
        }
        yield return new WaitForSeconds(0.03f);
    }
    private IEnumerator TagPlayerMidAirForLandingSound()
    {
        if (rigidbody2DPlayer.velocity.y < -2)
        {
            hasJumped = true;
        }
        yield return new WaitForSeconds(0.2f);
    }

    private void UpdateAnimations()
    {
        if (rigidbody2DPlayer.velocity.x > 0.1f || rigidbody2DPlayer.velocity.x < -0.1f)
        {
            playerAnimator.SetBool("move", true);
        }
        else
        {
            playerAnimator.SetBool("move", false);
        }
    }

    private void CheckIfTouchingDeathGround()
    {
        colliderGround = Physics2D.OverlapCapsule(isTouchingGroundedChecker.position, capsuleSize, CapsuleDirection2D.Horizontal, 0, deathLayer);
        colliderWallLeft = Physics2D.OverlapCircle(isTouchingWallLeftChecker.position, checkGroundRadius, deathLayer);
        colliderWallRight = Physics2D.OverlapCircle(isTouchingWallRightChecker.position, checkGroundRadius, deathLayer);

        colliderCrushed = Physics2D.OverlapCircle(isCrushedChecker.position, checkGroundRadius, groundLayer);

        if ((colliderGround != null) || (colliderWallLeft != null) || (colliderWallRight != null) || (colliderCrushed != null))
        {
            //Debug.Log(rigidbody2DPlayer.transform.position.ToString());
            Vector2 temp = spawnPoint.transform.position;
            rigidbody2DPlayer.transform.position = temp;
            rigidbody2DPlayer.velocity = Vector2.zero;
            deathFlag = true;
        }
        else if (deathFlag)
        {
            deathFlag = false;
            deaths++;
            PlayerPrefs.SetInt("Deaths", PlayerPrefs.GetInt("Deaths", 0) + 1);
            //Debug.Log("Deaths: " + deaths.ToString());
            deathsText.text = Convert.ToString(deaths, 2);
            audioSource.PlayOneShot(audioDeath, audioVolume * (audioVolumeGlobal / 100.0f));
            //Debug.Log(rigidbody2DPlayer.position.ToString());
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(jumpKeyCode))
        {
            if (isGrounded)
            {
                //Debug.Log("Jump up");
                rigidbody2DPlayer.velocity = new Vector2(rigidbody2DPlayer.velocity.x, jumpingPower);
                audioSource.PlayOneShot(audioJump, audioVolume * (audioVolumeGlobal / 100.0f));
                hasJumped = true;
            }
            else if (!isGrounded && isTouchingLeftWall)
            {
                //Debug.Log("WallJump to right");
                rigidbody2DPlayer.velocity = new Vector2(movementSpeed * 2, jumpingPower);
                audioSource.PlayOneShot(audioJump, audioVolume * (audioVolumeGlobal / 100.0f));
                hasJumped = true;
            }
            else if (!isGrounded && isTouchingRightWall)
            {
                //Debug.Log("WallJump to left");
                rigidbody2DPlayer.velocity = new Vector2(-movementSpeed * 2, jumpingPower);
                audioSource.PlayOneShot(audioJump, audioVolume * (audioVolumeGlobal / 100.0f));
                hasJumped = true;
            }
        }
    }

    private void Move()
    {
        x = Input.GetAxisRaw("Horizontal");
        if (isGrounded)
        {
            rigidbody2DPlayer.velocity = new Vector2(movementSpeed * x, rigidbody2DPlayer.velocity.y);
        }
        else
        {
            float targetSpeed = x * movementSpeed * airControlModifier;
            float targetSpeedChange = targetSpeed - rigidbody2DPlayer.velocity.x;
            float speedChange = Mathf.Clamp(targetSpeedChange, -maxAirSpeed, maxAirSpeed);
            rigidbody2DPlayer.velocity = new Vector2(rigidbody2DPlayer.velocity.x + speedChange, rigidbody2DPlayer.velocity.y);
        }
    }

    private void CheckIfTouchingValidGround()
    {
        colliderGround = Physics2D.OverlapCapsule(isTouchingGroundedChecker.position, capsuleSize, CapsuleDirection2D.Horizontal, 0, groundLayer);
        colliderWallLeft = Physics2D.OverlapCircle(isTouchingWallLeftChecker.position, checkGroundRadius, groundLayer);
        colliderWallRight = Physics2D.OverlapCircle(isTouchingWallRightChecker.position, checkGroundRadius, groundLayer);
        if (colliderGround != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (colliderWallLeft != null)
        {
            isTouchingLeftWall = true;
        }
        else
        {
            isTouchingLeftWall = false;
        }

        if (colliderWallRight != null)
        {
            isTouchingRightWall = true;
        }
        else
        {
            isTouchingRightWall = false;
        }
    }


}
