using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberConfig : MonoBehaviour
{
    public float maxFOV = 180f;
    public float maxAccelaration;
    public float maxVelocity;
    public float rotationSpeed;
    [Header("Wander")]
    public float wanderJitter;
    public float wanderRadius;
    public float wanderDistance;
    public float wanderPriority;
    [Header("Cohesion")]
    public float cohesionRadius;
    public float cohesionPriority;
    [Header("Alignment")]
    public float alignmentRadius;
    public float alignmentPriority;
    [Header("Seperation")]
    public float seperationRadius;
    public float seperationPriority;
    [Header("Avoidance")]
    public float avoidanceRadius;
    public float avoidancePriority;
}
