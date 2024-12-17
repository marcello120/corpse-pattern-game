using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarScript : MonoBehaviour
{
    [Header("Model")]
    public int acceptedCoprseCode = -1;
    public int healAmount = 0;
    public RiggedPlayerController.Utility ability;

    [Header("View")]
    public SpriteRenderer coprseView;
    public SpriteRenderer utiltiyView;
    public GameObject coprseDetecter;

    [Header("Effects")]
    public GameObject positiveEffect;
    public GameObject negativeEffect;

    [Header("Sounds")]
    public AudioClip positivieClip;
    public AudioClip negativeClip;



    // Start is called before the first frame update
    void Start()
    {
        if(acceptedCoprseCode != -1)
        {
            coprseView.sprite = CorpseStore.Instance.configs[acceptedCoprseCode];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Coll: " + collision.gameObject.name);
        if (collision.gameObject.tag == "Corpse")
        {
            CorpseScript corpseScript = collision.gameObject.GetComponent<CorpseScript>();

            if (corpseScript != null)
            {
                if(corpseScript.corpseNumber == acceptedCoprseCode || acceptedCoprseCode==-1)
                {
                    GameObject riggedPlayerController = GameObject.FindGameObjectWithTag("Player");

                    if(riggedPlayerController != null && riggedPlayerController.GetComponent<RiggedPlayerController>() != null)
                    {
                        RiggedPlayerController rigged = riggedPlayerController.GetComponent<RiggedPlayerController>();
                        if (healAmount > 0)
                        {
                            rigged.heal(healAmount);

                        }
                        else
                        {
                            rigged.setUtility(ability);
                        }
                        AudioSource.PlayClipAtPoint(positivieClip, transform.position);
                        GameObject effect = Instantiate(positiveEffect,transform.position, Quaternion.identity);
                        effect.transform.localScale = effect.transform.localScale * 0.4f;
                    }
                }
                else
                {
                    GameObject effect = Instantiate(negativeEffect, transform.position, Quaternion.identity);
                    effect.transform.localScale = effect.transform.localScale * 0.4f;
                }
            }
        }

    }
}
