using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuliHook : MonoBehaviour
{
    RiggedPlayerController riggedPlayer;

    public enum HookState
    {
        Ready,
        Shooting,
        Attached,
        Retreating
    }

    public HookState state;
    public float maxDistance;
    public float hookSpeed;

    public GameObject attachedTo;
    public LineRenderer rope;


    public Vector3 shootDir;
    public float currentDist;


    // Start is called before the first frame update
    void Start()
    {
        rope = GetComponent<LineRenderer>();
        riggedPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<RiggedPlayerController>();
        shootDir = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {




        if (state == HookState.Ready)
        {
            rope.SetPosition(0, Vector3.zero);
            rope.SetPosition(1, Vector3.zero);
            return;
        }

        if (riggedPlayer != null)
        {
            rope.SetPosition(0, riggedPlayer.transform.position);
            rope.SetPosition(1, transform.position);
        }

        if (state == HookState.Attached)
        {
            if(attachedTo == null)
            {
                retreat();
            }
            else
            {
                transform.position = attachedTo.transform.position;
            }
        }


        if (state == HookState.Shooting)
        {
            if (riggedPlayer != null)
            {
                currentDist = Vector3.Distance(transform.position, riggedPlayer.gameObject.transform.position);
                if (currentDist > maxDistance)
                {
                    retreat();
                }
            }
            transform.Translate(shootDir * hookSpeed * Time.deltaTime);
            return;
        }
        if (state == HookState.Retreating)
        {
            Transform target = riggedPlayer.gameObject.transform;
            Vector3 dirToTarget = (transform.position - target.position).normalized;
            if (target != null)
            {
                currentDist = Vector3.Distance(transform.position, riggedPlayer.gameObject.transform.position);
                if (currentDist < 0.1f)
                {
                    state = HookState.Ready;
                    return;
                }
                transform.Translate(-dirToTarget * hookSpeed * Time.deltaTime);
            }
            return;
        }
    }

    public void shoot()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 lookDir = (mousePosition - Camera.main.WorldToScreenPoint(transform.parent.position)).normalized;
        shootDir = lookDir;
        state = HookState.Shooting;
    }

    public void retreat()
    {
        attachedTo = null;
        state = HookState.Retreating;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(state != HookState.Shooting)
        {
            return;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            attachedTo = collision.gameObject;
            state = HookState.Attached;
            return;
        }
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Obstacle")
        {
            retreat();
            return;
        }
    }

    //if colliding with enemy - attach
    //if colliding with wall - retract

}
