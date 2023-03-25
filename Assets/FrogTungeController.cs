using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogTungeController : MonoBehaviour
{

    private BoxCollider2D boxCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public float attackPower;


    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void ReadyToAttack(Vector3 targetDir) {
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, angle);

        if (angle >= 90f && angle <= 270f)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }

        animator.SetBool("Ready", true);
    }

    public void NotReadyToAttack(Vector3 targetDir)
    {

        animator.SetBool("Ready", false);
    }

    public void StartAttack()
    {
        boxCollider.enabled = true;
    }

    public void EndAttack()
    {
        boxCollider.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.takeDamage(attackPower, GetComponentInParent<Enemy>());
        }
    }
}
