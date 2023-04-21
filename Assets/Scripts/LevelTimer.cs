using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public float timer;
    private TextMeshProUGUI timerText;
    public static LevelTimer instance;
    float timeStarted = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        float curTime = GetTime();
        timerText.text = "Time left: " + TimeSpan.FromSeconds(curTime).Minutes + ":"
                            + TimeSpan.FromSeconds(curTime).Seconds;
    }

    float GetTime()
    {
        return (timer - (Time.time - timeStarted));
    }
    public void StartTimer(float startTime)
    {
        timeStarted = Time.time;
        timer = startTime;
    }
}
