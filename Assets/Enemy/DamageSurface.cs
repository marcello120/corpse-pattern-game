using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSurface : MonoBehaviour
{

    private Animator animator;
    private Collider2D coll;

    public float attackPower;
    public Enemy parent;

    public float timeToLive;
    public float lifeTimer;
    public bool fading = false;

    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            return;
        }

        lifeTimer += Time.deltaTime;
        if (lifeTimer > timeToLive)
        {
            fading = true;
            Fade();
        }
    }

    private void Fade()
    {
        animator.SetTrigger("Fade");

    }

    public void RemoveSelf()
    {
        Destroy(gameObject);
    }

    //we collide with the shadow
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShadow")
        {
            RiggedPlayerController playerController = collision.gameObject.GetComponentInParent<RiggedPlayerController>();
            playerController.takeDamage(attackPower, null);
        }
    }
}
