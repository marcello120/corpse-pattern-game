using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class NewSnake : MonoBehaviour
{
    public float rotationSpeed;
    private Vector3 direction;
    public Transform Target;
    public float movespeed;

    Vector3 pos;
    Vector3 axis;

    public float frequency = 20f; // Speed of sine movement
    public float magnitude = 0.03f; //  Size of sine movement

    private SpriteRenderer sp;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        pos = transform.position;

        SnakeMovement();
    }

    void SnakeMovement()
    {
        direction = Target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        //Vector2 cursorPos = Target.position;
        //transform.position = Vector2.MoveTowards(transform.position, cursorPos, movespeed * Time.deltaTime);

        pos += direction * Time.deltaTime * movespeed;
        axis = Target.position - transform.position;
        axis = Quaternion.Euler(0, 0, 90) * axis;
        transform.position = pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;

        FlipHead();
    }

    void FlipHead()
    {
        if(Target.transform.position.x - transform.position.x > 1)
        {
            sp.flipY = false;
        }
        else
        {
            sp.flipY = true;
        }
    }

}
