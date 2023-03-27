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



    [SerializeField] private LayerMask dashLayerMask;

    // Start is called before the first frame update
    void Start()
    {  
        rb = GetComponent<Rigidbody2D>();
        animator= GetComponent<Animator>();
        spriteRenderer= GetComponent<SpriteRenderer>();
        healthText.text = playerHealth.ToString();
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
                invincible= false;
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

                rb.velocity= direction * moveSpeed;

                animator.SetBool("isMoving", true);
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


            //flip sprite based on mouse position
            Flip(lookDir);


              /*  float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle));
*/

                /*                if (lookDir.x > 0f && ParentBone.transform.localRotation.eulerAngles.y == 180)
                                {
                                    //transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                                    //holster.transform.localScale = new Vector3(Mathf.Abs(holster.transform.localScale.x), holster.transform.localScale.y, holster.transform.localScale.z);
                                    ParentBone.transform.eulerAngles = new Vector3(ParentBone.transform.localRotation.eulerAngles.x, 180f, ParentBone.transform.localRotation.eulerAngles.z);
                                }
                                else if (lookDir.x < 0f && ParentBone.transform.localRotation.eulerAngles.y != 180)
                                {
                                    //transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                                    // holster.transform.localScale = new Vector3(Mathf.Abs(holster.transform.localScale.x), holster.transform.localScale.y, holster.transform.localScale.z);
                                    ParentBone.transform.eulerAngles = new Vector3(ParentBone.transform.localRotation.eulerAngles.x, 0f, ParentBone.transform.localRotation.eulerAngles.z);

                                }*/

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
            ParentBone.transform.localScale = new Vector3(ParentBone.transform.localScale.x, ParentBone.transform.localScale.y*-1, ParentBone.transform.localScale.z);

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
        if (canMove & !stunned & canDash)
        {
            RaycastHit2D raycast = Physics2D.Raycast(transform.position, movementInput.normalized, DashAmount, dashLayerMask);
            if(raycast.collider==null)
            {
                rb.velocity = Vector2.zero;
                rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + movementInput.normalized * DashAmount);
                canDash = false;
                //change Quaternion identity to actual rotation 
                Instantiate(dashEffect, new Vector2(transform.position.x, transform.position.y) + movementInput.normalized * DashAmount, Quaternion.identity);
            }
            else
            {
                Debug.Log(raycast.collider);
            }
        }
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
        healthText.text = playerHealth.ToString();
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
    }

    public void OnEscape()
    {
        // TODO trigger pause screen

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
