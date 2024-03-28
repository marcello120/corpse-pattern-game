using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [Header("Fields")]
    public Rigidbody2D rb;
    public Animator animator;
    public AudioSource hitSound;


    [Header("Stats")]
    public float speed = 10;
    public float weaponAttackPower = 10;
    public float weaponKnockback = 10;

    public float baseWeaponReach = 3.35f;
    public float currentWeaponReach = 0f;
    public float reach;
    public Vector3 baseSize;
    public float size = 1;

    [Header("Debug")]
    public bool attackQueued = false;
    public float timeSinceAttack = 0;
    public bool canAttack = true;
    public bool attackState = false;

    [Header("Set this")]
    public WeaponSwing swing;
    public GameObject weaponBody;



    public void Init()
    {
        rb = weaponBody.GetComponent<Rigidbody2D>();
        animator = weaponBody.GetComponent<Animator>();
        currentWeaponReach = baseWeaponReach;
        baseSize = transform.localScale;
        increaseReach(0f);
        increaseSize(0f);
    }
    public virtual void Attack()
    {
        throw new System.NotImplementedException();
    }

    public virtual void increaseReach(float reachIncreae)
    {
        currentWeaponReach *= (1+ reachIncreae);
        transform.localPosition = new Vector3(currentWeaponReach, transform.localPosition.y, weaponBody.transform.localPosition.z);
    }

    public virtual void increaseSize(float sizeIncrease)
    {
        size *= (1 + sizeIncrease);
        transform.localScale = baseSize * size;
    }

    public virtual bool isAttacking()
    {
        return attackState;
    }

    public virtual void HeavyAttack()
    {
    }
}
