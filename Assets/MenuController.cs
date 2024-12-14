using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame updates

    public TextMeshProUGUI highscore_Lvl1;
    public TextMeshProUGUI highscore_Lvl2;
    public TextMeshProUGUI highscore_Lvl3;
    public TextMeshProUGUI highscore_Endless;

    public int selectedSceneId;

    public string selectedSceneName;


    private void Start()
    {
        highscore_Lvl1.SetText("HighScore: " + PlayerPrefs.GetInt("HighScore_" + GameManager.Level.LEVEL1_1.ToString(), 0).ToString());
        highscore_Lvl2.SetText("HighScore: " + PlayerPrefs.GetInt("HighScore_" + GameManager.Level.LEVEL1_2.ToString(), 0).ToString());
        highscore_Lvl3.SetText("HighScore: " + PlayerPrefs.GetInt("HighScore_" + GameManager.Level.LEVEL1_3.ToString(), 0).ToString());
        highscore_Endless.SetText("HighScore: " + PlayerPrefs.GetInt("HighScore_" + GameManager.Level.ENDLESS.ToString(), 0).ToString());

    }

    public void LoadLevel1()
    {
        selectedSceneId = 3;
        selectedSceneName = "Level 1_1";
    }
    public void LoadLevel2()
    {
        selectedSceneId = 4;
        selectedSceneName = "Level 1_2";
    }
    public void LoadLevel3()
    {
        selectedSceneId = 5;
        selectedSceneName = "Level 1_3";
    }
    public void LoadEndless()
    {
        selectedSceneId = 6;
        selectedSceneName = "Endless";
    }

    public void setSceneId(int id)
    {
        selectedSceneId = id;
    }

    public void GoToHub()
    {
        SceneManager.LoadScene("HUB");
    }


    public void setSelectedWeapon(RiggedPlayerController.WeaponEnum weapon)
    {
        StaticData.chosenWeapon = weapon;
        SceneManager.LoadScene(selectedSceneId);
    }

    public void setSelectedWeapon(string weaponName)
    {
        RiggedPlayerController.WeaponEnum parsed_enum = (RiggedPlayerController.WeaponEnum)System.Enum.Parse(typeof(RiggedPlayerController.WeaponEnum), weaponName.ToUpper());
        StaticData.chosenWeapon = parsed_enum;
        SceneManager.LoadScene(selectedSceneId);
    }


    public void Quit()
    {
       Application.Quit();
    }

}
