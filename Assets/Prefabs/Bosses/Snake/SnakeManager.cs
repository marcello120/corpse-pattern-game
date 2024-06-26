using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SnakeManager : MonoBehaviour
{
    //A Snake movementhez:
    public float frequency = 20f; // Speed of sine movement
    public float magnitude = 0.03f; //  Size of sine movement
    public float rotationSpeed = 1.0f;

    public Transform player;

    Vector3 pos;
    Vector3 axis;
    Vector3 direction;

    //A Snake fel�p�t�s�hez:
    [SerializeField] float distenceBetween = 0.2f;
    [SerializeField] float speed = 10f;
    // [SerializeField] float turnspeed = 20f;
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();
    List<GameObject> snakeBody = new List<GameObject>();


    // Snake attack
    [SerializeField] float inAttackRange = 1.0f;
    private bool isAttacking = false;
    private Vector3 whereToAttack;

    //�j Snake
    GameObject SnakeObject;
    public GameObject SnakePrefab;
    private float x = 1;

    float countUp = 0;
    void Start()
    {
        CreateBodyParts();
        if ( player == null )
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = transform.position;

        ManageSnakeBody();
        
        if (Vector2.Distance(player.position, transform.position) > inAttackRange)
        {
            SnakeMovement();
        }
        else
        {
            isAttacking= true;
            whereToAttack = player.position;
            StartCoroutine(SnakeAttack(whereToAttack));
        }
        
        
        
    }

    void ManageSnakeBody()
    {
        if (bodyParts.Count > 0)
        {
            CreateBodyParts();
        }
        for (int i = 0; i < snakeBody.Count; i++)
        {
            if (snakeBody[i] == null)
            {
                snakeBody.RemoveAt(i);
                i = i - 1;
                               
                AddNewSnake();
            }
        } 
       if (snakeBody.Count == 0) 
        {
            Destroy(this);
        }
    }

    void SnakeMovement()
    {

        direction = player.position - transform.position;
        pos += direction * Time.deltaTime * speed;
        axis = player.position - transform.position;
        axis = Quaternion.Euler(0, 0, 90) * axis;
        transform.position = pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;

        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, axis);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        //snakeBody[0].GetComponent<Rigidbody2D>().velocity = snakeBody[0].transform.right * speed * Time.deltaTime;
        //if(Input.GetAxis("Horizontal") != 0)
        //{
        //    snakeBody[0].transform.Rotate(new Vector3(0, 0, -turnspeed * Time.deltaTime * Input.GetAxis("Horizontal")));
        //}

        if (snakeBody.Count > 1 )
        {
            for (int i = 1; i < snakeBody.Count; i++)
            {
                MarkerManager markM = snakeBody[i-1].GetComponent<MarkerManager>();
                snakeBody[i].transform.position = markM.markerList[0].position;
                snakeBody[i].transform.rotation = markM.markerList[0].rotation;
                markM.markerList.RemoveAt(0);
            }
        }
    }

    void CreateBodyParts()
    {
        if (snakeBody.Count == 0 )
        {
           GameObject temp1 = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            if (!temp1.GetComponent<MarkerManager>())
                temp1.AddComponent<MarkerManager>();
            if (!temp1.GetComponent<Rigidbody2D>())
            {
                temp1.AddComponent<Rigidbody2D>();
                temp1.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            snakeBody.Add(temp1);
            bodyParts.RemoveAt(0);
        }

        MarkerManager markM = snakeBody[snakeBody.Count - 1].GetComponent<MarkerManager>();
        if (countUp == 0)
        {
            markM.ClearMarkerList();
        }
        countUp += Time.deltaTime;
        if(countUp >= distenceBetween)
        {
            GameObject temp = Instantiate(bodyParts[0], markM.markerList[0].position, markM.markerList[0].rotation, transform);
            if (!temp.GetComponent<MarkerManager>())
            {
                temp.AddComponent<MarkerManager>();
            }
            if(!temp.GetComponent<Rigidbody2D>()) 
            {
                temp.AddComponent<Rigidbody2D>();
                temp.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            snakeBody.Add(temp);
            bodyParts.RemoveAt(0);
            temp.GetComponent<MarkerManager>().ClearMarkerList();
            countUp= 0;
        }
    }
    public void AddBodyParts(GameObject obj)
    {
        bodyParts.Add(obj);
    }

    public void AddNewSnake()
    {
        Instantiate(SnakePrefab, transform.position, Quaternion.identity);
    }
    IEnumerator SnakeAttack(Vector3 attackDirection)
    {
        yield return new WaitForSeconds(1);
        while (transform.position != attackDirection)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, 10 * speed * Time.deltaTime); 
            //nem j� mert az eg�sz snake egyszerre odaker�l ahelyett, hogy minden tagja egyes�vel
            yield return null;
            isAttacking= false;
        }
    }
}
