using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeBoss : Enemy
{
    public SnakeBossSegment segment;

    public List<SnakeBossSegment> segmentList = new List<SnakeBossSegment>();

    public int initialSize = 5;

    public float segmentDistance = 0.5f;

    public float frequency = 5f; // Speed of sine movement
    public float magnitude = 0.01f; //  Size of sine movement

    public Vector3 dirToPlayer;

    public GameObject roamTarget;


    public float sinceCharge = 0f;
    public float chargeLimit = 30f;
    public float chargeTime = 1f;
    public float chargeTimer = 0f;

    public bool charging = false;

    public float chargeDistance = 3f;

    Vector3 savedPlayerPos;

    public bool originalSnake;

    public GameObject selfObject;

    public float segmentHealth = 10f;

    public float headHealth = 20f;

    public StatusEffect stunStatusEffect;

    public GameObject smallSnake;

    public List<SnakeBoss> othersnakes;

    public void Start()
    {
        base.Init();
        setState(State.Moving);
        target = GameObject.FindGameObjectWithTag("Player").transform;

        if (originalSnake)
        {
            for (int i = 0; i < initialSize; i++)
            {
                SnakeBossSegment newSegment = Instantiate(segment);
                newSegment.setup(this, segmentHealth);

                if (segmentList.Count == 0)
                {
                    newSegment.transform.position = transform.position - transform.right * segmentDistance;
                }
                else
                {
                    newSegment.transform.position = segmentList[segmentList.Count - 1].transform.position - transform.right * segmentDistance;

                }
                segmentList.Add(newSegment);
            }
        }

        health = segmentList.Sum(seg=>seg.health) + headHealth;

        InvokeRepeating(nameof(checkForSnakes), 0f, 10f);


    }


    public void checkForSnakes()
    {
        othersnakes.Clear();
        othersnakes.AddRange(GameObject.FindObjectsOfType(typeof(SnakeBoss)));

    }


    public void SplitInit(List<SnakeBossSegment> newSegmentList)
    {

        //TODO: new snake does not move or damage for x secs.
        addStatusEffect(Instantiate(stunStatusEffect)); //maybe this?


        originalSnake = false;
        segmentList = new List<SnakeBossSegment>();
        for (int i = 0; i < newSegmentList.Count; i++)
        {
            //SnakeBossSegment newSegment = newSegmentList[i];
            GameObject gameObject = Instantiate(newSegmentList[i].gameObject);
            SnakeBossSegment newSegment = gameObject.GetComponent<SnakeBossSegment>();
            newSegment.enabled = true;
            gameObject.GetComponent<Collider2D>().enabled = true;
            gameObject.GetComponent<Animator>().enabled = true;
            newSegment.snakeBoss = this;

            if (segmentList.Count == 0)
            {
                newSegment.transform.position = transform.position - transform.right * segmentDistance;
            }
            else
            {
                newSegment.transform.position = segmentList[segmentList.Count - 1].transform.position - transform.right * segmentDistance;

            }
            segmentList.Add(newSegment);
        }
    }

    public void segmentDestroyed(SnakeBossSegment snakeBossSegment)
    {
        int index = segmentList.IndexOf(snakeBossSegment);
        if (index == -1)
        {
            return;
            //Debug.LogError("What the seven fucks is this>L");
        } else
        if (index == 0)
        {
            if(segmentList.Count == 1)
            {
                Death();
            }
            segmentList.RemoveAt(0);
            snakeBossSegment.Kill(smallSnake);
            return;
        } else
        if (index == segmentList.Count - 1)
        {
            snakeBossSegment.Kill(smallSnake);
            segmentList.RemoveAt(segmentList.Count - 1);
            return;
        }
        Split(index);
    }


    public void Split(int index)
    {

        //TODO: spwan Split effect


        List<SnakeBossSegment> newSegmentList = new List<SnakeBossSegment>();

        SnakeBoss newSnake = Instantiate(selfObject, segmentList[index].transform.position, Quaternion.identity).GetComponent<SnakeBoss>();


        int i = segmentList.Count - 1;
        while (i >= index)
        {
            if (i != index)
            {
                newSegmentList.Add(segmentList[i]);
            }
            GameObject go = segmentList[i].gameObject;
            segmentList.RemoveAt(i);
            Destroy(go);
            i--;
        }

        newSnake.SplitInit(newSegmentList);

    }

    public override void moveInDirection(Vector3 direction)
    {

        Vector3 force = direction * movemetSpeed * 1f * Time.fixedDeltaTime;
        rb.AddForce(force);

    }

    private Quaternion rotateToDir(Vector3 dir, Quaternion currentRot)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        return Quaternion.Slerp(currentRot, rotation, 1.5f * Time.deltaTime);
    }

    public override void Death()
    {
        for (int i = 0; i < segmentList.Count; i++)
        {
            Destroy(segmentList[i].gameObject);
        }
        base.Death();
    }

    public void FixedUpdate()
    {
        commonUpdate();

        if(dirToPlayer.x < 0 && spriteRenderer.flipY == false && !charging)
        {
            spriteRenderer.flipY = true;
        }
        if (dirToPlayer.x > 0 && spriteRenderer.flipY == true && !charging)
        {
            spriteRenderer.flipY = false;
        }

        if (target == null)
        {
            return;
        }

        if (charging)
        {
            chargeTimer += Time.deltaTime;
            if (chargeTimer > chargeTime)
            {
                chargeTimer = 0f;
                charging = false;
                movemetSpeed *= 0.25f;
                sinceCharge = 0;
            }
        }
        else
        {
            sinceCharge += Time.deltaTime;
        }

        //charge if player is far or at an odd angle with perventage chance
        if (Vector3.Distance(target.position, transform.position) > chargeDistance && !charging && sinceCharge > chargeLimit)
        {
            Charge();
        }
        else
        if (aliveTime % 30 == 0 && !charging && sinceCharge > chargeLimit)
        {
            Charge();
        }
        else if ( segmentList.Count>1 && Vector3.Distance(segmentList[segmentList.Count - 1].transform.position, transform.position) < 1.4 && !charging && sinceCharge > chargeLimit)
        {
            Charge();
        }

        Move();
        MoveTail();
    }

    private void Move()
    {
        if (isStunned())
        {
            return;
        }
        if (charging)
        {
            dirToPlayer = (savedPlayerPos - transform.position).normalized;
            transform.rotation = rotateToDir(dirToPlayer, transform.rotation);

            var pos2 = transform.position;
            pos2 += dirToPlayer * Time.deltaTime * movemetSpeed;
            Vector3 axis2 = savedPlayerPos - transform.position;
            axis2 = Quaternion.Euler(0, 0, 90) * axis2;
            transform.position = pos2 + axis2 * Mathf.Sin(Time.time * frequency) * magnitude;

            return;
        }

        dirToPlayer = (target.position - transform.position).normalized;
        transform.rotation = rotateToDir(dirToPlayer, transform.rotation);


        var pos = transform.position;

        Vector3 adjustedDir = dirToPlayer;

        for (int i = 0; i < othersnakes.Count; i++)
        {
            Vector3 dirToOther = (transform.position - othersnakes[i].transform.position).normalized * 0.33f;
            adjustedDir += dirToOther;
        }

        pos += adjustedDir * Time.deltaTime * movemetSpeed;
        Vector3 axis = target.position - transform.position;
        axis = Quaternion.Euler(0, 0, 90) * axis;
        transform.position = pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;
    }

    private void MoveTail()
    {
        if (isStunned())
        {
            return;
        }
        for (int i = 0; i < segmentList.Count; i++)
        {
            Transform targetTransfrom = transform;
            SnakeBossSegment currentSegment = segmentList[i];

            if (currentSegment == null)
            {
                return;
            }

            if (i != 0)
            {
                targetTransfrom = segmentList[i - 1].transform;
            }

            Vector3 dir = (targetTransfrom.position - currentSegment.transform.position).normalized;

            currentSegment.transform.rotation = rotateToDir(dir, currentSegment.transform.rotation);

            Vector3 targetPos = targetTransfrom.position - targetTransfrom.right * segmentDistance * 0.9f;

            float followSpeed = 8f;

            if (charging)
            {
                followSpeed = 16f;
            }

            //if (Vector3.Distance(targetTransfrom.position, currentSegment.transform.position) > segmentDistance || i == 0)
            //{
            currentSegment.transform.position = Vector2.Lerp(currentSegment.transform.position, targetPos, followSpeed * Time.fixedDeltaTime);
            //currentSegment.transform.position = Vector3.MoveTowards(currentSegment.transform.position, targetTransfrom.position, 2f * Time.fixedDeltaTime);
            //}
        }
    }



    private void Charge()
    {
        if (charging)
        {
            return;
        }
        charging = true;
        movemetSpeed *= 4f;
        savedPlayerPos = target.transform.position + transform.right * 2f;
    }

    public void getHitSegment(float dmg, SnakeBossSegment hitSegment)
    {
        if (isDead)
        {
            return;
        }

        if (hitEffect != null)
        {
            Instantiate(hitEffect, hitSegment.transform.position, Quaternion.identity);
        }
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound, 0.3f);
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }

        health = segmentList.Sum(seg => seg.health) + headHealth;

        if (health <= 0)
        {
            Death();
        }
    }


    public override void getHit(float damage, Vector2 knockback, Vector3 directtion)
    {
        if (segmentList.Count <= 1)
        {
            health -= damage;
        }
        //this is head hit
        headHealth -= damage;
        if(headHealth < 0)
        {
            headHealth = 20f;
            addStatusEffect(Instantiate(stunStatusEffect));
        }
    }
}
