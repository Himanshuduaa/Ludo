using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerTimer : MonoBehaviour
{
    public string currentTime;
    public string timeUpTime;
    public string startup_time;
    public string actionType;
    public float timer_value;
    public float timer = 0f;
    public bool isTimerRunning = false;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (isTimerRunning)
        {
            timer = Time.deltaTime + timer; // Increment the timer by the time since the last frame
            if (timer < timer_value)
            {
                this.gameObject.GetComponent<Image>().fillAmount = timer / timer_value;
            }
            else
            {
                this.gameObject.GetComponent<Image>().fillAmount = 1f;
                isTimerRunning = false;
                Debug.Log("timer >= timer_value");
                StopTimer();
            }
        }
    }

    [ContextMenu("Do StartTimer")]
    public void StartTimer()
    {
        this.gameObject.GetComponent<Image>().fillAmount = 0;
        if (!isTimerRunning)
        {
            isTimerRunning = true;
        }
    }

    [ContextMenu("Do StopTimer")]
    // Stop the timer coroutine
    public void StopTimer()
    {
        if (isTimerRunning)
        {
            isTimerRunning = false;
            timer = 0f;
            this.gameObject.GetComponent<Image>().fillAmount = 1f;
        }
    }
    public void resetValues()
    {
        timer = 0;
    }
    [ContextMenu("Do GetTimerValue")]
    public void ShowTime()
    {
        //Debug.Log("Starting Timer");
        isTimerRunning = false;
        DateTime currTime = DateTime.ParseExact(currentTime, "yyyy-MM-ddTHH:mm:ss.fffZ", null, System.Globalization.DateTimeStyles.AssumeUniversal);
        DateTime timeup = DateTime.ParseExact(timeUpTime, "yyyy-MM-ddTHH:mm:ss.fffZ", null, System.Globalization.DateTimeStyles.AssumeUniversal);
        DateTime startTime = DateTime.ParseExact(startup_time, "yyyy-MM-ddTHH:mm:ss.fffZ", null, System.Globalization.DateTimeStyles.AssumeUniversal);
        timer = 0;
        TimeSpan timeDiff = currTime - startTime;
        timer = (float)timeDiff.Seconds;
        // Calculate the time difference
        TimeSpan timeDifference = timeup - startTime;// startTime;
        float timeToPlay = (float)timeDifference.Seconds;
        //timeToPlay = timer + timeToPlay;
        timer_value = timeToPlay;
        if(timer>timer_value)
        {
            timer = timer_value;
        }
        Debug.Log("Current time is "+ currentTime + " Time up time is "+ timeUpTime+ " Start Time is "+ startup_time+" Timer is "+ timer+ " Time to play is "+ timeToPlay);
        StartTimer();
    }
}