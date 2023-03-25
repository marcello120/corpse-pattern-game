using UnityEngine;

public class OrcController : Enemy
{
    public DetectionZoneController detectionZoneController;
    public float dirChangeChance = 1000;
    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        enemyname = "Orc";
        base.Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        
        if(!moveToPlayerWithDetectionZone(detectionZoneController))
        {
            float dirchange = Random.Range(0f, dirChangeChance);

            if(dirchange < 10f) {
                dirChangeChance = 1000;
                float random = Random.Range(0f, 260f);
                direction = new Vector2(Mathf.Sin(random), Mathf.Cos(random)).normalized;
            }
            else
            {
                dirChangeChance = dirChangeChance - 1;
                rb.AddForce(direction * movemetSpeed);

            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        damagePlayer(collision.collider);
    }
}
