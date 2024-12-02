using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;



    private void OnEnable()
    {
        volumeSlider = GetComponent<Slider>();
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            //go to source
            if (GameManager.Instance != null)
            {
                volumeSlider.value = GameManager.Instance.backgroundMusic.volume;
            }

        }
        else
        {
            volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
    }

    public void changeVolume()
    {
        GameManager.Instance.setBackgroundMusicVolume(volumeSlider.value);
    }

}
