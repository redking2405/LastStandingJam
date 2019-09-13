using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets._2D;

public class Timer : Singleton<Timer>
{
    public Text timer;
    public float startTimer;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        time = startTimer;
        timer.text=LifeHack.FloatToTime(startTimer, "#0:00.000");
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= 0)
        {
            time -= Time.deltaTime;
        }

        UpdateTimer(time);

        if (time <= 0)
        {
            int biggestScore = 0;
            string joueurWin = "noWin";
            foreach (GameObject player in GameManager.Instance.players)
            {
                if (player.gameObject.GetComponent<UserControl>().score > biggestScore)
                {
                    biggestScore = player.gameObject.GetComponent<UserControl>().score;
                    joueurWin = player.name;
                }
            }
            timer.text = joueurWin + " a gagné avec " + biggestScore + " points !";
        }

    }

    void UpdateTimer(float time)
    {
        timer.text = LifeHack.FloatToTime(time, "#0:00.000");
    }

    public void RestartTimer()
    {
        time = startTimer;
        UpdateTimer(time);
    }
}
