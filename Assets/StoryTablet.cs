using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryTablet : MonoBehaviour
{
    private string scene_name;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        scene_name = SceneManager.GetActiveScene().name;
        if (PlayerPrefs.GetFloat("Story " + scene_name + " Part 2", 0) != 0)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Unlock()
    {
        PlayerPrefs.SetFloat("Story " + scene_name + " Part 2", 0.5f);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if(collision.tag == "Player")
            {
                Unlock();
                GetComponent<Collider2D>().enabled = false;
                GetComponent<AudioSource>().Play();
                animator.SetTrigger("expand");
                Destroy(gameObject, 1f);
            }
        }
    }

}
