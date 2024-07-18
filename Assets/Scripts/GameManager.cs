using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int enemyLeft = 10;
    public float timeLeft = 60f;

    bool isPlaying = true;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


    void Update()
    {
        if (isPlaying)
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0)
            {
                GameOverScene();
            }
        }
    }


    public void EnemyDied()
    {
        enemyLeft--;

        if (enemyLeft <= 0)
        {
            GameOverScene();
        }
    }


    public void GameOverScene()
    {
        isPlaying = false;
        SceneManager.LoadScene("GameOverScene");
    }
}
