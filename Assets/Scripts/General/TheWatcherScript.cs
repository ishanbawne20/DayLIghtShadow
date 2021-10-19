using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheWatcherScript : MonoBehaviour
{
    public PlayerBasics Player;
    public int numberEnemies = 9;

    public TMPro.TMP_Text hp;
    public TMPro.TMP_Text enleft;

    float health;
    int enemies;

    void Start()
    {
        health = Player.getHealth();
        enemies = numberEnemies;
    }


    void Update()
    {
        health = Player.getHealth();

        hp.text = String.Format("HP : {0}", health);
        enleft.text = String.Format("Enemies Left : {0}", enemies);


        if(enemies <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void dedEnemy()
    {
        enemies--;
    }
}
