using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    public float playerHealth = 30;
    public float maxHealth = 30;
    //public float currentMaxHealt = 5;

    public float moveSpeed = 1f;
    public float baseMoveSpeed = 1f;
    public float collisionOffset = 0.001f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;

    public Vector2 movementInput;
    public Rigidbody2D rb;
    public List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    public Animator animator;

    public bool canMove = true;

    public bool canAttack = true;

    public float invicnicbilityTime = 0.3f;

    public bool invincible = false;

    public float stunTime = 0.2f;
    public bool stunned = false;

    public float ininvTimer = 0;
    public float stuntimer = 0;

    public Vector3 mousePosition = new Vector3(0, 0, 0);

    public Holster holster;


    public AudioSource takeDamageSound;

    public bool flipped;

    public Collider2D collider;


    //public bool gameIsPaused = false;
    //public GameObject pauseMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator= GetComponent<Animator>();
    }

    private void FixedUpdate()
    {

        //Debug.DrawRay(transform.position, lookDir);

        //toggle inivicibility
        if(invincible)
        {
            ininvTimer += Time.deltaTime;
            if (ininvTimer > invicnicbilityTime)
            {
                ininvTimer = 0;
                setNotInvincible();
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

                bool success = TryMove(movementInput);

                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }

                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
                animator.SetBool("isMoving", success);
            }
            else
            {
                animator.SetBool("isMoving", false);

            }

            //update mouse postion
            mousePosition = Input.mousePosition;

            Vector3 lookDir = (mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;

            animator.SetFloat("MouseY", lookDir.y);
            animator.SetFloat("MouseX", lookDir.x);



                if (lookDir.x < 0f && transform.localScale.x > 0f)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                }
                else if (lookDir.x > 0f && transform.localScale.x < 0f)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

                }
            

        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            // Check for potential collisions
            int count = rb.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            // Can't move if there's no direction to move in
            return false;
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

    void OnFire()
    {
        if (canAttack)
        {
            //animator.SetTrigger("swordAttack");
            holster.Attack();
        }

    }

    public void takeDamage(float damage, Enemy enemy)
    {
        if(!canAttack && !canMove)
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
        Vector2 knockback = (transform.position - enemy.gameObject.transform.position).normalized * 500;
        rb.AddForce(knockback);
        playerHealth -= damage;
        canMove = true;
        if (playerHealth <= 0)
        {
            Die();
        }
        else
        {
            setInvincible();
            stunned = true;
        }
    }

    public void setInvincible()
    {
        invincible = true;
        collider.enabled= false;
    }
    public void setNotInvincible()
    {
        invincible = false;
        collider.enabled = true;

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
    }

    public void OnEscape()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    

}
