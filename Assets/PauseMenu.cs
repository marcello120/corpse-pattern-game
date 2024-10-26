using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
   public void Resume()
    {
        RiggedPlayerController player = (RiggedPlayerController)FindFirstObjectByType(typeof(RiggedPlayerController));
        player.Resume();

    }

    public void Quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
