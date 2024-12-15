using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHint : MonoBehaviour
{
    public GameManager.Level level;

    private Dictionary<GameManager.Level, string> levelNames = new Dictionary<GameManager.Level, string>
    {
        {GameManager.Level.LEVEL1_1, "Level 1" },
        {GameManager.Level.LEVEL1_2, "Level 2" },
        {GameManager.Level.LEVEL1_3, "Level 3" },
        {GameManager.Level.ENDLESS, "Endless Mode" },
        {GameManager.Level.DARKNESS, "Darkness" },

    };


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
               InfoInterfaceController.Instance.FadeInHintAndZoomAndMove(transform.position, levelNames[level], "Top Score: " + highscore, "Earn TRIPLE the score when venturing into the Darkness");
            }
            else
            {
                InfoInterfaceController.Instance.FadeInHintAndZoomAndMove(transform.position, levelNames[level], "Top Score: " + highscore);
            }
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
