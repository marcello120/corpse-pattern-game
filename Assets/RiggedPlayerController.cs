using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RiggedPlayerController : PlayerController
{
    public GameObject ParentBone;
    public float DashAmount = 1f;
    public bool canDash = true;
    public float dashCooldown = 3f;
    public float dashTimer = 0f;

    public GameObject dashEffect;

    public ParticleSystem dust;

    public AudioSource walkSound;

    public Collider2D collider;

    public int level = 1;

    public bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject deathMenuUI;
    [SerializeField] private Animator pauseMenuAnimator;


    [SerializeField] private LayerMask dashLayerMask;
    [SerializeField] private LayerMask enemyLayerMask;

    public Animator effectsAnimator;

    public PerfectDashDecider perfectDashDecider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthText.text = playerHealth.ToString();
        collider = GetComponent<Collider2D>();
        
    }

    private void FixedUpdate()
    {
        //toggle dash
        if (!canDash)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer > dashCooldown)
            {
                dashTimer = 0;
                canDash = true;
            }
        }

        //Debug.DrawRay(transform.position, lookDir);

        //toggle inivicibility
        if (invincible)
        {
            ininvTimer += Time.deltaTime;
            if (ininvTimer > invicnicbilityTime)
            {
                ininvTimer = 0;
                invincible = false;
            }
        }
        //toggle stunned
        if (stunned)
        {
            stuntimer += Time.deltaTime;
            if (stuntimer > stunTime)
            {
                stuntimer = 0;
                stunned = false;
            }
        }

        //move the character
        if (canMove && !stunned)
        {
            // If movement input is not 0, try to move
            if (movementInput != Vector2.zero)
            {
                Vector3 direction = movementInput.normalized;
                
                //roate perfect dash checker towards input dir
                if (perfectDashDecider != null)
                {
                    // Calculate the angle in radians.
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                    // Set the rotation to the calculated angle.
                    perfectDashDecider.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                }

                rb.velocity = direction * moveSpeed;

                if (!dust.isPlaying)
                {
                    dust.Play();
                }
                if (!walkSound.isPlaying)
                {
                    walkSound.Play();
                }

                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
                dust.Stop();
                walkSound.Stop();

            }

            //update mouse postion
            mousePosition = Input.mousePosition;

            Vector3 lookDir = (mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;

            animator.SetFloat("MouseY", lookDir.y);
            animator.SetFloat("MouseX", lookDir.x);


            //flip sprite based on mouse position
            Flip(lookDir);



        }
    }

    private void Flip(Vector3 lookDir)
    {
        if (lookDir.x > 0f & ParentBone.transform.localScale.y < 0)
        {
            //transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            //holster.transform.localScale = new Vector3(Mathf.Abs(holster.transform.localScale.x), holster.transform.localScale.y, holster.transform.localScale.z);
            ParentBone.transform.localScale = new Vector3(ParentBone.transform.localScale.x, ParentBone.transform.localScale.y * -1, ParentBone.transform.localScale.z);
        }
        else if (lookDir.x < 0f & ParentBone.transform.localScale.y > 0)
        {
            //transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            // holster.transform.localScale = new Vector3(Mathf.Abs(holster.transform.localScale.x), holster.transform.localScale.y, holster.transform.localScale.z);
            ParentBone.transform.localScale = new Vector3(ParentBone.transform.localScale.x, ParentBone.transform.localScale.y * -1, ParentBone.transform.localScale.z);

        }
    }





    public void EndAtttack()
    {
        canMove = true;
        swordAttack.StopAttack();

    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnJump()
    {
        Debug.Log("JUMP");
        //check if we can move
        if (canMove & !stunned & canDash)
        {
            animator.SetTrigger("Dash");
            rb.velocity = Vector2.zero;
            Vector2 dashDirection = movementInput.normalized; 
            //check for wall
            RaycastHit2D raycast = Physics2D.Raycast(transform.position, movementInput.normalized, DashAmount, dashLayerMask);
            Vector2 dashPosition;
            if (raycast.collider == null)
            {
                //no wall - dash full amount
                dashPosition = new Vector2(transform.position.x, transform.position.y) + dashDirection * DashAmount;
                Quaternion dashRotation = Quaternion.FromToRotation(Vector2.right, dashDirection);
                Instantiate(dashEffect, dashPosition, dashRotation);

                //PERFECT DASH
                if (perfectDashDecider != null && perfectDashDecider.isPerfect().Count > 0)
                {
                    Debug.Log("PERFECT DODGE ON ");
                    invincible = true;
                    effectsAnimator.Play("effect_flash");
                    //set all
                    foreach (GameObject stuntarget in perfectDashDecider.isPerfect())
                    { 
                        if (stuntarget.GetComponent<Enemy>() != null)
                        {
                            Enemy enemy = stuntarget.GetComponent<Enemy>();
                            enemy.stun();
                            perfectDashDecider.playNice();
                        }
                        else if (stuntarget.GetComponentInParent<Enemy>() != null)
                        {
                            Enemy enemy = stuntarget.GetComponentInParent<Enemy>();
                            enemy.stun();
                            perfectDashDecider.playNice();
                        }
                    }
                }

            }
            else
            {
                //wall - dash only until wall
                Debug.Log(raycast.collider);
                ColliderDistance2D distanceToObstacle = Physics2D.Distance(collider, raycast.collider);
                dashPosition = new Vector2(transform.position.x, transform.position.y) + dashDirection * distanceToObstacle.distance * 0.9f;
            }
            rb.MovePosition(dashPosition);
            canDash = false;
        }
    }

    void OnFire()
    {
        if (canAttack)
        {
            //animator.SetTrigger("swordAttack");
            holster.Attack();
            if(canMove && !stunned)
            {
                mousePosition = Input.mousePosition;
                Vector3 lookDir = (mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
                rb.velocity = lookDir * 0.5f;

            }
        }

    }

    public void takeDamage(float damage, Enemy enemy)
    {
        if (!canAttack && !canMove)
        {
            return;
        }
        if (invincible)
        {
            return;
        }
        animator.SetTrigger("Hit");
        takeDamageSound.Play();
        canMove = false;
        Vector2 knockback = (transform.position - enemy.gameObject.transform.position).normalized * 300;
        rb.AddForce(knockback);
        playerHealth -= damage;
        healthText.text = playerHealth.ToString();

        CameraShake.Instance.Shake(3, 0.2f);

        canMove = true;
        if (playerHealth <= 0)
        {
            Die();
        }
        else
        {
            invincible = true;
            stunned = true;
        }
    }

    public void Die()
    {
        canAttack = false;
        holster.canAttack = false;
        canMove = false;
        animator.SetTrigger("Death");
    }


    public void Defeat()
    {
        Destroy(gameObject);
        deathMenuUI.SetActive(true);
    }

    public void levelUp()
    {
        level++;
        Debug.Log("LeveledUp " + level);
    }

    private Coroutine animationCoroutine;
    private Coroutine closeAnimationCoroutine;
    private bool isAnimating = false;
    private bool canToggle = true;

    public void OnEscape()
    {
        if (gameIsPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        gameIsPaused = true;

        // Enable the GameObject before starting animation
        pauseMenuUI.SetActive(true);

        if (!isAnimating)
        {
            isAnimating = true;
            pauseMenuAnimator.Play("Pause_Menu_Animation");
            StartCoroutine(WaitForAnimationFinish("Pause_Menu_Animation"));
        }
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        gameIsPaused = false;

        if (!isAnimating)
        {
            isAnimating = true;
            pauseMenuAnimator.Play("Close_Pause_Menu");
            StartCoroutine(WaitForAnimationFinish("Close_Pause_Menu"));
        }
    }

    IEnumerator WaitForAnimationFinish(string animationName)
    {
        while (!pauseMenuAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName) ||
               pauseMenuAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
        {
            yield return null;
        }

        isAnimating = false;

        // Deactivate UI only if it was a close animation
        if (!gameIsPaused && animationName == "Close_Pause_Menu")
        {
            pauseMenuUI.SetActive(false);
        }
    }


    public void heal(float inHealth)
    {
        playerHealth += inHealth;
        if (playerHealth > maxHealth)
        {
            playerHealth= maxHealth;
        }
        healthText.text= playerHealth.ToString();
    }

    public void increaseDashAmount(float dashIncreaseAmount)
    {
        DashAmount += dashIncreaseAmount;
        perfectDashDecider.updateDashAmount();
    }

    public void inreaseReach(float reachIncrease)
    {
        holster.increaseReach(reachIncrease);
    }

    public void inreaseWeaponSize(float sizeIncrease)
    {
        holster.increaseSize(sizeIncrease);
    }


}
