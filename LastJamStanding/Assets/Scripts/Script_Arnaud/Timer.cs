using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets._2D;
using UnityEngine.SceneManagement;
public class Timer : Singleton<Timer>
{
    public Text timer;
    public float startTimer;
    public float time;
    public Hunter player2;
    public Hunter player;
    public UserControl prey;

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
            GameManager.Instance.Restart();
            timer.text="Press Start to Restart";
        }

        if(player.restart || prey.restart || player2.restart && time <= 0)
        {
            SceneManager.LoadScene("FinalScene");
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
