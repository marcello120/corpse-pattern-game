using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class WeaponSwing : MonoBehaviour
{
    public float knockbackPower;
    public float weaponAttackPower;
    public float timeToLive;

    private bool isActive;

    private GameObject player;

    private PolygonCollider2D polycollider;

    public GameObject hitEffect;

    

    // Start is called before the first frame update
    void Start()
    {
        polycollider = GetComponent<PolygonCollider2D>();
        polycollider.enabled = false;
        isActive = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void EndAttack()
    {
        polycollider.enabled = false;
        isActive = false;
    }

    public void StartAttack()
    {
        polycollider.enabled = true;
        isActive = true;
    }

    public void DestorySelf()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 1f * Time.deltaTime);
    }

    public void InitWeaponAttack(float knockbackIN, float weaponAttackPowerIN)
    {
        knockbackPower = knockbackIN;
        weaponAttackPower = weaponAttackPowerIN;

    }

    private Vector2 GetKnockBack(Collider2D collision)
    {
        Vector3 parentPosition = gameObject.GetComponentInParent<Transform>().position;

        Vector2 direction = parentPosition - collision.gameObject.transform.position;

        return (direction.normalized * knockbackPower * -1);
    }

    private static readonly int ENEMY_DAMAGE_LAYER = 14;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Enemy" && isActive)
        {
            if (collision.gameObject.layer == ENEMY_DAMAGE_LAYER)
            {
                Debug.Log("Hello");
            }
             
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.getHit(weaponAttackPower, GetKnockBack(collision));
                Vector2 contactpoint= collision.ClosestPoint(enemy.transform.position);
                //Instantiate(hitEffect, contactpoint, Quaternion.identity);
            }
        }
    }
}
