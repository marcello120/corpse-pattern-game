using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryViewer : MonoBehaviour
{
    [Header("Set This")]
    public CinemachineVirtualCamera cinemachineCamera;


    [Header("Extra")]
    public StoryViewController storyViewController;
    public RiggedPlayerController player;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        storyViewController = FindObjectOfType<StoryViewController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RiggedPlayerController>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerShadow")
        {
            audioSource.Play();
            player.canAttack = false;
            StaticData.hubStorySpawn = true;
            storyViewController.Show();
            StartCoroutine(CinemachineCameraZoom(cinemachineCamera, 1f, 0.5f)); // Adjust target zoom and duration as needed
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PlayerShadow")
        {
            audioSource.Stop();
            player.canAttack = true;
            StaticData.hubStorySpawn = false;
            storyViewController.DontShow();
            StartCoroutine(CinemachineCameraZoom(cinemachineCamera, 2f, 0.5f)); // Adjust target zoom and duration as needed
        }

    }


    // Helper method for camera zoom
    private IEnumerator CinemachineCameraZoom(CinemachineVirtualCamera virtualCamera, float targetZoom, float duration)
    {
        if (virtualCamera == null) yield break;

        float startZoom = virtualCamera.m_Lens.OrthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startZoom, targetZoom, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = targetZoom;
    }
}
