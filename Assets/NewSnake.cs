using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSnake : MonoBehaviour
{
    public float rotationSpeed;
    private Vector2 direction;
    public Transform Target;

    public float movespeed;

    // Update is called once per frame
    void Update()
    {
        direction = Target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        Vector2 cursorPos = Target.position;
        transform.position = Vector2.MoveTowards(transform.position, cursorPos, movespeed * Time.deltaTime);
    }
}
