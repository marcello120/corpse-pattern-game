using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public RiggedPlayerController.WeaponEnum weaponEnum;
    public GameObject selectionEffect;
    public AudioClip audioClip;
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StaticData.chosenWeapon = weaponEnum;
            collision.gameObject.GetComponent<RiggedPlayerController>().selectWeapon();
            Instantiate(selectionEffect,transform);
            audioSource.PlayOneShot(audioClip);
        }
    }
}
