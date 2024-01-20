using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Member : MonoBehaviour
{
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    Vector3 wanderTarget;

    public Level level;
    public MemberConfig conf;

    void Start()
    {
        level = FindObjectOfType<Level>();
        conf = FindObjectOfType<MemberConfig > ();

        position = transform.position;
        velocity = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
    }

    void Update()
    {
        acceleration = Combine();
        acceleration = Vector3.ClampMagnitude(acceleration, conf.maxAccelaration);
        velocity = velocity + acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, conf.maxVelocity);
        position = position + velocity * Time.deltaTime;
        float halfBounds = level.bounds;
        Vector3 minBounds = -new Vector3(halfBounds, halfBounds, 0) + level.localPos;
        Vector3 maxBounds = new Vector3(halfBounds, halfBounds, 0) + level.localPos;
        transform.position = position;
    }

    void WrapAround(ref Vector3 vector, Vector3 min, Vector3 max)
    {
        vector.x = WrapAroundFloat(vector.x, min.x, max.x);
        vector.y = WrapAroundFloat(vector.y, min.y, max.y);
        vector.z = WrapAroundFloat(vector.z, min.z, max.z);
    }

    float WrapAroundFloat(float value, float min, float max)
    {
        if (value > max)
        {
            value = min;
        }
        else if (value < min)
        {
            value = max;
        }
        return value;
    }

    float RandomBinomial()
    {
        return Random.Range(0f, 1f) - Random.Range(0f, 1f);
    }
    
    protected Vector3 Wander()
    {
        float jitter = conf.wanderJitter * Time.deltaTime;
        wanderTarget += new Vector3(RandomBinomial() * jitter, RandomBinomial() * jitter, 0);
        wanderTarget = wanderTarget.normalized;
        wanderTarget *= conf.wanderRadius;
        Vector3 targetInLocalSpace = wanderTarget + new Vector3(conf.wanderDistance, conf.wanderDistance, 0);
        Vector3 targetInWorldSpace = transform.TransformPoint(targetInLocalSpace);
        targetInWorldSpace -= this.position;
        return targetInWorldSpace.normalized;
    }

    Vector3 Cohesion()
    {
        Vector3 cohesionVector= new Vector3();
        int countMembers = 0;
        var neighbors = level.GetNeighbors(this, conf.cohesionRadius);
        if(neighbors.Count == 0)
        {
            return cohesionVector;
        }
        foreach(var member in neighbors) 
        {
            if (IsInFOV(member.position))
            {
                cohesionVector += member.position;
                countMembers++;
            }
        }
        if(countMembers == 0)
        {
            return cohesionVector;
        }
        cohesionVector /= countMembers;
        cohesionVector = cohesionVector - this.position;
        cohesionVector = Vector3.Normalize(cohesionVector);
        return cohesionVector;
    }

    Vector3 Alignment()
    {
        Vector3 alignVector = new Vector3();
        var members = level.GetNeighbors(this, conf.alignmentRadius);
        if(members.Count == 0)
        {
            return alignVector;
        }
        foreach(var member in members)
        {
            if (IsInFOV(member.position))
            {
                alignVector += member.velocity;
            }
        }
        return alignVector.normalized;
    }

    Vector3 Seperation()
    {
        Vector3 seperateVector = new Vector3();
        var members = level.GetNeighbors(this, conf.seperationRadius);
        if(members.Count == 0)
        {
            return seperateVector;
        }
        foreach(var member in members)
        {
            if (IsInFOV(member.position))
            {
                Vector3 movingTowards = this.position - member.position;
                if(movingTowards.magnitude > 0)
                {
                    seperateVector += movingTowards.normalized / movingTowards.magnitude;
                }
            }
        }
        return seperateVector.normalized;
    }

    Vector3 Avoidance()
    {
        Vector3 avoidVector = new Vector3();
        var enemyList = level.GetEnemies(this, conf.avoidanceRadius);
        if(enemyList.Count == 0)
        {
            return avoidVector;
        }
        foreach(var enemy in enemyList)
        {
            avoidVector += RunAway(enemy.transform.position);
        }
        return avoidVector.normalized;
    }

    Vector3 RunAway(Vector3 target)
    {
        Vector3 neededVelocity = (position - target.normalized) * conf.maxVelocity;
        return neededVelocity - velocity;
    }

    virtual protected Vector3 Combine()
    {
        Vector3 finalVec = conf.cohesionPriority * Cohesion() + conf.wanderPriority * Wander()
                            +conf.alignmentPriority * Alignment()
                            + conf.seperationPriority * Seperation()
                            + conf.avoidancePriority * Avoidance();
        return finalVec;
    }

    public bool IsInFOV(Vector3 vec)
    {
        return Vector3.Angle(this.velocity, vec - this.position) <= conf.maxFOV;
    }
}
