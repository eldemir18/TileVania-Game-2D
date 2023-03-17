using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{


    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    
    [Space]

    [SerializeField] GameObject pauseMenuCanvas;

    [Space]

    [SerializeField] AudioClip deathSFX;

    bool IsGamePaused = false;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    Camera mainCamera;

    float volume;
    float playerGravityScale;
    bool isAlive = true;
    bool doubleJump = false;

    void Start()
    {
        mainCamera = Camera.main;
        volume =  PlayerPrefs.GetFloat("EffectVolume",0.5f);

        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();

        playerGravityScale = myRigidbody.gravityScale;
    } 

    void Update()
    {
        if(!isAlive){ return;}

        ResetRoll();
        Run();
        ClimbLadder();
        FlipSprite();
        Die();
    }

    void ResetRoll()
    {
        if((myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Ladder")) || 
            myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) && 
            myAnimator.GetBool("isRolling"))
        {
            myAnimator.SetBool("isRolling", false);
            doubleJump = false;
        }
    }

    void OnMove(InputValue value)
    {
        if(!isAlive){ return;}

        moveInput = value.Get<Vector2>();
    }

    void OnPause(InputValue value)
    {
        PauseOrResume();
    }

    public void PauseOrResume()
    {
        IsGamePaused = !IsGamePaused;
        pauseMenuCanvas.SetActive(IsGamePaused);
        Time.timeScale = IsGamePaused ? 0f : 1f;
    }

    void OnJump(InputValue value)
    {
        if(!isAlive){ return;}
    
        if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Ladder")) || 
           myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpSpeed);
            doubleJump = true;  
        }
        else if(doubleJump || !myAnimator.GetBool("isRolling"))
        {
            myAnimator.SetBool("isRolling", true);
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpSpeed);
            doubleJump = false;
        }
    }

    void Run()
    {
        myRigidbody.velocity = new Vector2(moveInput.x * runSpeed,  myRigidbody.velocity.y);

        if(!myAnimator.GetBool("isRolling"))
        {
            myAnimator.SetBool("isRunning", IsPlayerHasHorizontalSpeed());
        }
    }

    void ClimbLadder()
    {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) || 
           myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myRigidbody.gravityScale = 0;

            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
            
            myAnimator.SetBool("isClimbing", IsPlayerHasVerticalSpeed());
        }
        else
        {
            myRigidbody.gravityScale = playerGravityScale;
            
            myAnimator.SetBool("isClimbing", false);
        }
    }

    
    void Die()
    {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")) ||
           myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            AudioSource.PlayClipAtPoint(deathSFX, mainCamera.transform.position, volume);
            myRigidbody.velocity = new Vector2(0f, jumpSpeed);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    void FlipSprite()
    {
        if(IsPlayerHasHorizontalSpeed())
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    bool IsPlayerHasHorizontalSpeed()
    {
        return Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
    }

    bool IsPlayerHasVerticalSpeed()
    {
        return Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
    }

    public void DisableMovement()
    {
        isAlive = false;
    }
}
