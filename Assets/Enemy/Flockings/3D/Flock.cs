using UnityEngine;

public class Flock : MonoBehaviour
{
    float speed;
    bool turning = false;

    void Start()
    {
        speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        Bounds b = new Bounds(FlockManager.FM.transform.position, FlockManager.FM.movementLimit * 2);

        if (!b.Contains(transform.position))
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        if (turning)
        {
            Vector3 direction = FlockManager.FM.transform.position - transform.position;
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),FlockManager.FM.rotationSpeed * Time.deltaTime);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), FlockManager.FM.rotationSpeed * Time.deltaTime);

        }
        else
        {
            ResetFlockSpeed();

            ApplyRules();
        }
        this.transform.Translate(Vector3.up * speed * Time.deltaTime);
        //this.transform.Translate(0, speed * Time.deltaTime, 0);
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = FlockManager.FM.allFlock;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        Vector3 valign = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                nDistance = Vector2.Distance(go.transform.position, this.transform.position);

                if (nDistance <= FlockManager.FM.neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    // Separation (Avoidance)
                    if (nDistance < FlockManager.FM.avoidDistance)
                    {
                        float separationWeight = Mathf.Clamp01(nDistance / FlockManager.FM.maxSeparationDistance);
                        vavoid += (this.transform.position - go.transform.position).normalized * FlockManager.FM.separationWeight * separationWeight;
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed += anotherFlock.speed;

                    // Alignment
                    valign += anotherFlock.transform.up; // Assuming that the up direction is the forward direction for alignment
                }
            }
        }

        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize;

            // Dynamic Goal Weight
            float distanceToGoal = Vector2.Distance(transform.position, FlockManager.FM.goalPos);
            float dynamicGoalWeight = Mathf.Clamp01(distanceToGoal / FlockManager.FM.maxGoalDistance);
            vcentre += (FlockManager.FM.goalPos - this.transform.position) * FlockManager.FM.goalWeight * dynamicGoalWeight;

            speed = gSpeed / groupSize;

            if (speed > FlockManager.FM.maxSpeed)
            {
                speed = FlockManager.FM.maxSpeed;
            }

            // Collision Avoidance
            Vector3 collisionAvoidance = Vector3.zero;
            foreach (GameObject go in gos)
            {
                if (go != this.gameObject)
                {
                    float avoidRadius = FlockManager.FM.avoidRadius;
                    Vector3 toOther = go.transform.position - transform.position;
                    float relativeSpeed = (go.GetComponent<Flock>().speed + speed) / 2;
                    float timeToCollision = Mathf.Clamp01(Vector3.Dot(toOther, transform.up) / (relativeSpeed * relativeSpeed));
                    Vector3 futurePos = go.transform.position + go.GetComponent<Flock>().speed * timeToCollision * go.transform.up;
                    float separation = avoidRadius / (Vector2.Distance(transform.position, futurePos) + 0.1f); // Adding a small value to avoid division by zero
                    collisionAvoidance += (transform.position - futurePos).normalized * separation;
                }
            }
            vavoid += collisionAvoidance * FlockManager.FM.collisionAvoidWeight;

            // Separation
            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(Vector3.forward, direction),
                    FlockManager.FM.rotationSpeed * Time.deltaTime);
            }

            // Alignment
            valign /= groupSize;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(Vector3.forward, valign),
                FlockManager.FM.alignmentWeight * Time.deltaTime);

            // Cohesion
            Vector3 cohesionDirection = (vcentre - transform.position).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(Vector3.forward, cohesionDirection),
                FlockManager.FM.cohesionWeight * Time.deltaTime);

            // Bounds Checking
            Vector3 boundedDirection = CheckBounds();

            // Wandering
            Vector3 wanderForce = Wander();
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(Vector3.forward, wanderForce),
                FlockManager.FM.wanderWeight * Time.deltaTime);

            // Apply the bounds-corrected direction
            Vector3 finalDirection = boundedDirection.normalized * FlockManager.FM.boundsCorrectionWeight +
                wanderForce.normalized * (1 - FlockManager.FM.boundsCorrectionWeight);

            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(Vector3.forward, finalDirection),
                FlockManager.FM.rotationSpeed * Time.deltaTime);
        }
    }

    Vector3 Wander()
    {
        float angleChange = Random.Range(-FlockManager.FM.wanderAngleChange, FlockManager.FM.wanderAngleChange);
        FlockManager.FM.currentWanderAngle += angleChange;

        Vector3 wanderDirection = new Vector3(Mathf.Cos(FlockManager.FM.currentWanderAngle), Mathf.Sin(FlockManager.FM.currentWanderAngle), 0);
        Vector3 wanderForce = wanderDirection.normalized * FlockManager.FM.movementLimit.x/2;

        return wanderForce;
    }

    Vector3 CheckBounds()
    {
        float boundsX = FlockManager.FM.movementLimit.x;
        float boundsY = FlockManager.FM.movementLimit.y;
        float padding = 1.0f; // Adjust this to leave some space before correcting direction

        Vector3 currentPosition = transform.position;
        Vector3 goalDirection = FlockManager.FM.goalPos - currentPosition;
        Vector3 boundsDirection = Vector3.zero;

        if (Mathf.Abs(currentPosition.x) > boundsX - padding)
        {
            boundsDirection += new Vector3(-Mathf.Sign(currentPosition.x), 0, 0);
        }

        if (Mathf.Abs(currentPosition.y) > boundsY - padding)
        {
            boundsDirection += new Vector3(0, -Mathf.Sign(currentPosition.y), 0);
        }

        // If outside bounds, steer towards goalPos
        if (boundsDirection != Vector3.zero)
        {
            boundsDirection.Normalize();
            return boundsDirection;
        }

        // If within bounds, continue towards goalPos
        return goalDirection.normalized;
    }

    void ResetFlockSpeed()
    {
        if (Random.Range(0, 100) < FlockManager.FM.goalMoveChance + 5f)
        {
            speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
        }
    }
}
