using UnityEngine;

public class WeaponSwing : MonoBehaviour
{
    public float knockbackPower;
    public float weaponAttackPower;
    public float timeToLive;

    private bool isActive;

    private PolygonCollider2D polycollider;

    public GameObject hitEffect;

    public AudioClip onWallHitSound;

    AudioSource audioSource;

    

    // Start is called before the first frame update
    void Start()
    {
        polycollider = GetComponent<PolygonCollider2D>();
        polycollider.enabled = false;
        isActive = false;
        audioSource= GetComponent<AudioSource>();

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

    private static readonly int ENEMY_DAMAGE_LAYER = 17;

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
                Debug.LogError("THIS IS NOT SUPPOSED TO HAPPEN ANYMORE");

                enemy.getHit(weaponAttackPower, GetKnockBack(collision));

            }
            else if(collision.GetComponent<EnemyHitbox>()!= null)
            {
                Vector3 directionToEnemy = (collision.gameObject.transform.position - transform.parent.parent.position).normalized;
                collision.GetComponent<EnemyHitbox>().getHit(weaponAttackPower, GetKnockBack(collision),directionToEnemy);
                Vector2 contactpoint = collision.ClosestPoint(transform.position);
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, contactpoint, Quaternion.identity);
                }
            }

        }
        if (collision.tag == "Projectile" && isActive)
        {
            collision.GetComponent<Projectile>().Parried();

        }
        if (collision.tag == "Wall" && isActive)
        {
            Vector2 contactpoint = collision.ClosestPoint(transform.position);
            if (hitEffect != null)
            {
                Instantiate(hitEffect, contactpoint, Quaternion.identity);
            }
            //if(onWallHitSound!= null)
            //{
            //    audioSource.PlayOneShot(onWallHitSound);
            //}
        }
        //IF DESCTUCIBLE
        if (collision.tag == "Destructible" && isActive)
        {
            Vector2 contactpoint = collision.ClosestPoint(transform.position);
            if (hitEffect != null)
            {
                Instantiate(hitEffect, contactpoint, Quaternion.identity);
            }
            Destructible destructible= collision.GetComponent<Destructible>();
            if(destructible != null)
            {
                destructible.hitDestructible(weaponAttackPower);
            }
        }


    }
}
