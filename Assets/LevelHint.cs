using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHint : MonoBehaviour
{
    public GameManager.Level level;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            int highscore = PlayerPrefs.GetInt("HighScore_" + level, 0);
            if(level == GameManager.Level.DARKNESS)
            {
               InfoInterfaceController.Instance.FadeInHintAndZoomAndMove(transform.position, StaticData.levelNames[level], "Top Score: " + highscore, "Earn TRIPLE the score when venturing into the Darkness <br><br>Make sure to bring a TORCH");
               return;
            }
            //is unlocked
            if ((level == GameManager.Level.LEVEL1_2 || level == GameManager.Level.LEVEL1_3) &&  PlayerPrefs.GetInt("Unlocked " + level, 0) == 0)
            {
                //level to beat
                string levelToBeat = level == GameManager.Level.LEVEL1_2 ? StaticData.levelNames[GameManager.Level.LEVEL1_1] : StaticData.levelNames[GameManager.Level.LEVEL1_2];
                InfoInterfaceController.Instance.FadeInHintAndZoomAndMove(transform.position, StaticData.levelNames[level], "Top Score: " + highscore, "Beat " + levelToBeat + " to unlock this level." );
                return;
            }

            InfoInterfaceController.Instance.FadeInHintAndZoomAndMove(transform.position, StaticData.levelNames[level], "Top Score: " + highscore);
            
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InfoInterfaceController.Instance.FadeOut();

        }
    }
}
