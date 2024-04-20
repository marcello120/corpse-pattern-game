using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class RiggedPlayerController : PlayerController
{
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

    public SlowStatusEffect slowStatusEffect;
    public StunStatusEffect stunStatusEffect;

    public List<PowerUp> powerUps;

    public PatternGrid patternGrid;

    public enum Utility
    {
        MASS_SLOW,
        PULL,
        RANGED_SHOT,
        CORPSE_MOVE,
        PUSH_FORWARD,
        EXPLODE,
        TRAP,
        GRAPPLING_HOOK,
    }

    [Header("Util")]


    public Utility selectedUtility;
    [SerializeField] private LayerMask interactLayerMask;

    public MuliTimer utilTimer;

    public float utilHoldTime;

    public GameObject pullLine;
    public Projectile projectile;

    //default -100
    public int storedCorpse = -100;
    public GameObject corpse;

    public GameObject trap;

    public MuliHook mulihook;


    public GameObject playerBody;

    public MuliTimer chargeTimer;

    public bool isChargingAttack;

    public GameObject chargeCompleteEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = playerBody.GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        UpdateHearts();
        if (baseMoveSpeed == 0)
        {
            baseMoveSpeed = moveSpeed;
        }
        
    }

    private void FixedUpdate()
    {

        if (!utilTimer.isDone())
        {
            utilTimer.update(Time.deltaTime);
        }
        if (isChargingAttack)
        {
            if (!chargeTimer.isDone())
            {
                chargeTimer.update(Time.deltaTime);
                holster.Charge(Time.deltaTime);
                if (chargeTimer.isDone())
                {
                    Instantiate(chargeCompleteEffect, transform);
                }

            }
        }

        //toggle dash
        if (!canDash)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer > dashCooldown)
            {
                dashTimer = 0;
                canDash = true;
                playEffectDashReload();
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

    private void playEffectDashReload()
    {
        effectsAnimator.Play("effect_wheel");
        //insatatiate vfx
    }

    //private void playEffectInvincEnd()
    //{
    //    effectsAnimator.Play("effect_wheel");
    //    //insatatiate vfx
    //}

    private void playEffectSlowCircle()
    {
        effectsAnimator.Play("effect_vortex");
    }
    private void playEffectPerfectDash()
    {
        effectsAnimator.Play("effect_flash");
    }
    private void playEffectExplode()
    {
        effectsAnimator.Play("effect_explode");
    }

    private void Flip(Vector3 lookDir)
    {
        Vector3 bodyHolderScale = playerBody.transform.parent.localScale;
        if (lookDir.x > 0f & bodyHolderScale.x < 0)
        {
            //transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            //holster.transform.localScale = new Vector3(Mathf.Abs(holster.transform.localScale.x), holster.transform.localScale.y, holster.transform.localScale.z);
            playerBody.transform.parent.localScale = new Vector3(bodyHolderScale.x * -1, bodyHolderScale.y, bodyHolderScale.z);
        }
        else if (lookDir.x < 0f & bodyHolderScale.x > 0)
        {
            //transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            // holster.transform.localScale = new Vector3(Mathf.Abs(holster.transform.localScale.x), holster.transform.localScale.y, holster.transform.localScale.z);
            playerBody.transform.parent.localScale = new Vector3(bodyHolderScale.x * -1, bodyHolderScale.y, bodyHolderScale.z);

        }
    }


    public void Stun(Enemy enemy)
    {
        enemy.addStatusEffect(Instantiate(stunStatusEffect));

    }

    public void Slow(Enemy enemy)
    {
        enemy.addStatusEffect(Instantiate(slowStatusEffect));

    }


    public void EndAtttack()
    {
        canMove = true;
        swordAttack.StopAttack();

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("LOFASZ fire " + context);

        if (context.performed)
        {
            movementInput = context.ReadValue<Vector2>();
        }
        if (context.canceled)
        {
            movementInput = Vector2.zero;
        }
    }

    public void setUtility(Utility ability)
    {
        selectedUtility = ability;
    }
    public void OnUtility(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            utilHoldTime= Time.time;
        }
        if (context.canceled)
        {
            utilHoldTime = 0;
        }

        if (!utilTimer.isDone())
        {
            return;
        }

        if(selectedUtility == Utility.MASS_SLOW && context.canceled)
        {
            Debug.Log("MASS_SLOW");
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 2f);
            playEffectSlowCircle();
            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.tag == "Enemy" && hitCollider.gameObject.GetComponent<Enemy>() != null)
                {
                    Enemy en = hitCollider.gameObject.GetComponent<Enemy>();
                    Slow(en);
                }

            }
            utilTimer.reset();
            return;
        }
        if (selectedUtility == Utility.PULL && context.canceled)
        {
            Vector3 lookDir = (mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
            Vector3 lookDir2D = new Vector3(lookDir.x, lookDir.y);
            float castOffset = 0.3f;
            float castDistance = 2f;


            Debug.Log("PULL");
            RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position + castOffset * lookDir2D, lookDir2D,castDistance, interactLayerMask);
            Debug.DrawRay(transform.position + castOffset * lookDir2D, lookDir2D*castDistance, Color.yellow, 20);

            //If something was hit.
            if (hit.collider != null)
            {
                Enemy hitEnemy = hit.collider.GetComponent<Enemy>();
                if(hitEnemy != null)
                {
                    Debug.Log("Enemy In Range!");
                    Debug.Log("PULL  " + hit.collider.name);
                    LineRenderer line = Instantiate(pullLine, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
                    //line.transform.position = transform.position;
                    line.SetPosition(0,transform.position);
                    line.SetPosition(1,hitEnemy.transform.position);
                    Destroy(line, 0.15f);
                    hitEnemy.forceMoveToPosition(transform.position + castOffset * lookDir2D, 1f);
                    utilTimer.reset();
                    return;
                }

            }
            LineRenderer backupline = Instantiate(pullLine, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
            //line.transform.position = transform.position;
            backupline.SetPosition(0, transform.position);
            Vector3 newPos = transform.position + castOffset * lookDir2D;
            backupline.SetPosition(1, newPos + castDistance* lookDir2D);
            Destroy(backupline, 0.15f);
            utilTimer.reset();
            return;
        }
        if (selectedUtility == Utility.PUSH_FORWARD && context.canceled)
        {
            Vector3 lookDir = (mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
            Vector3 lookDir2D = new Vector3(lookDir.x, lookDir.y);
            float pushOffset = 0.01f;
            float pushReach = 1f;
            float pushDist = 2f;


            Debug.Log("PUSH");
            RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position + pushOffset * lookDir2D, lookDir2D, pushReach, interactLayerMask);
            Debug.DrawRay(transform.position + pushOffset * lookDir2D, lookDir2D * pushReach, Color.yellow, 20);

            //If something was hit.
            if (hit.collider != null)
            {
                Enemy hitEnemy = hit.collider.GetComponent<Enemy>();
                if (hitEnemy != null)
                {
                    Debug.Log("Enemy In Range!");
                    Debug.Log("PUSH  " + hit.collider.name);
                    LineRenderer line = Instantiate(pullLine, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
                    //line.transform.position = transform.position;
                    line.SetPosition(0, transform.position);
                    line.SetPosition(1, hitEnemy.transform.position);
                    Destroy(line, 0.2f);
                    hitEnemy.forceMoveToPosition(transform.position + pushDist * lookDir2D, 1f);
                    utilTimer.reset();
                    return;
                }

            }
            LineRenderer backupline = Instantiate(pullLine, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
            //line.transform.position = transform.position;
            backupline.SetPosition(0, transform.position);
            Vector3 newPos = transform.position + pushOffset * lookDir2D;
            backupline.SetPosition(1, newPos + pushReach * lookDir2D);
            Destroy(backupline, 0.2f);
            utilTimer.reset();
            return;
        }
        if (selectedUtility == Utility.RANGED_SHOT && context.canceled)
        {
            Debug.Log("RANGED_SHOT");
            Vector3 lookDir = (mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
            Vector3 lookDir2D = new Vector3(lookDir.x, lookDir.y);
            Projectile spawenProj = Instantiate(projectile, transform.position, Quaternion.identity);
            spawenProj.byPlayer = true;
            spawenProj.isParried = true;
            spawenProj.Setup(lookDir2D, 1,10);
            utilTimer.reset();

        }
        if (selectedUtility == Utility.CORPSE_MOVE && context.canceled)
        {
            if(storedCorpse == -100)
            {
                Vector2Int grdiPos = GameManager.Instance.worldPosToGridPos(transform.position, GameManager.Instance.grid);
                storedCorpse = GameManager.Instance.removeCoprseAndReturnID(grdiPos);
                if(storedCorpse == 0)
                {
                    storedCorpse = -100;
                    return;
                }
                GameManager.Instance.removeCorpseAtWorldPos(transform.position);

            }
            else
            {
                Vector3 place =  GameManager.Instance.AddWorldPosToGridAndReturnAdjustedPos(transform.position, storedCorpse, 0).corpseWorldPos;
                GameObject newCorpse = Instantiate(corpse, place, Quaternion.identity);
                newCorpse.GetComponent<SpriteRenderer>().sprite = CorpseStore.Instance.configs[storedCorpse];
                storedCorpse = -100;
            }
            utilTimer.reset();
        }
        if (selectedUtility == Utility.EXPLODE && context.canceled)
        {
            float expRadius = 1f;
            float knockbackPower = 1;
            playEffectExplode();

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, expRadius);
            //foreach collider in hitColliders
            foreach (Collider2D coll in hitColliders)
            {
                //enemy
                if (coll.gameObject.GetComponent<Enemy>() != null)
                {
                    Vector2 direction = transform.position - coll.gameObject.transform.position;

                    Vector2 knockback = (direction.normalized * knockbackPower * -1);

                    coll.gameObject.GetComponent<Enemy>().getHit(2f, knockback);
                }
                //player
                takeDamage(1f, null);
            }
        }
        if (selectedUtility == Utility.TRAP && context.canceled)
        {
            Instantiate(trap,transform.position,Quaternion.identity);
            utilTimer.reset();
        }
        if (selectedUtility == Utility.GRAPPLING_HOOK && context.canceled)
        {
            if(mulihook == null)
            {
                return;
            }

            if(mulihook.state == MuliHook.HookState.Ready)
            {
                mulihook.shoot();
            }
            else
            {
                mulihook.retreat();
            }

        }





    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("JUMP");
        //check if we can move
        if (canMove && !stunned && canDash && context.performed)
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
                    playEffectPerfectDash();
                    //set all
                    foreach (GameObject stuntarget in perfectDashDecider.isPerfect())
                    { 
                        if (stuntarget.GetComponent<Enemy>() != null)
                        {
                            Enemy enemy = stuntarget.GetComponent<Enemy>();
                            //enemy.stun();
                            Stun(enemy);
                            perfectDashDecider.playNice();
                        }
                        else if (stuntarget.GetComponentInParent<Enemy>() != null)
                        {
                            Enemy enemy = stuntarget.GetComponentInParent<Enemy>();
                            //enemy.stun();
                            Stun(enemy);
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



    public void OnFire(InputAction.CallbackContext context)
    {
        if (gameIsPaused)
        {
            return;
        }

        if (canAttack)
        {
            if(context.canceled)
            {
                if(isChargingAttack && chargeTimer.isDone())
                {
                    holster.HeavyAttack();

                }
                else
                {
                    holster.Attack();

                }
                chargeTimer.reset();
                isChargingAttack = false;

                //animator.SetTrigger("swordAttack");
                if (canMove && !stunned)
                {
                    mousePosition = Input.mousePosition;
                    Vector3 lookDir = (mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
                    rb.velocity = lookDir * 0.5f;

                }

            }
            if (context.started)
            {
                isChargingAttack= true;
            }
           
        }

    }

    public void OnTab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("TAB pressed");
            patternGrid.toggleBig();
        }
    }
    
    public void Slow()
    {
        moveSpeed *= 0.1f;
        Debug.Log("PLAYER SLOWED");
    }

    public void UnSlow()
    {
        moveSpeed = baseMoveSpeed;
        Debug.Log("PLAYER UN SLOWED");
    }

    public void takeDamage(float damage, GameObject enemy)
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
        Vector2 knockback;
        if (enemy != null)
        {
            knockback = (transform.position - enemy.gameObject.transform.position).normalized * 200;
        }
        else
        {
            // Generate a random knockback direction if the enemy is null
            float randomAngle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
            knockback = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * 200;
        }
        rb.AddForce(knockback);
        playerHealth -= damage;
        UpdateHearts();

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

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Resume();
        }
    }

    public void Resume()
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


    public void addPowerUp(PowerUp powerUp)
    {
        powerUps.Add(powerUp);
    }


    public void heal(float inHealth)
    {
        playerHealth += inHealth;
        if (playerHealth > maxHealth)
        {
            playerHealth= maxHealth;
            
        }
        UpdateHearts();
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

    public void decreaseAbilityCooldown(float abilityCooldownDecreaseAmount)
    {
        dashCooldown -= abilityCooldownDecreaseAmount;
    }

    public void increaseEffectsDuration(float durationIncreaseAmount)
    {
        stunStatusEffect.duration += durationIncreaseAmount;
        slowStatusEffect.duration += durationIncreaseAmount;
    }


    //------------Heart Container------------
    [SerializeField] private UnityEngine.UI.Image[] hearts;

    public void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth)
            {
                hearts[i].gameObject.SetActive(true);
                hearts[i].color = Color.white;
            }
            else
            {
                if (i < maxHealth)
                {
                    hearts[i].gameObject.SetActive(true);
                    hearts[i].color = Color.black;
                }
                else
                {
                    hearts[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
