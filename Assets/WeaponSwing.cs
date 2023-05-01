using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwing : MonoBehaviour
{
    public float knockbackPower;
    public float weaponAttackPower;
    public float timeToLive;

    private bool isActive;

    private PolygonCollider2D polycollider;

    

    // Start is called before the first frame update
    void Start()
    {
        polycollider = GetComponent<PolygonCollider2D>();
        polycollider.enabled = false;
        isActive = false;
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


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Enemy" && isActive)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.getHit(weaponAttackPower, GetKnockBack(collision));
            }
        }
    }
}
