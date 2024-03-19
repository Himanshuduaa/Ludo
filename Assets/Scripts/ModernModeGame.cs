using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModernModeGame : MonoBehaviour
{
    public DateTime startTime;
    public DateTime endTime;
    public Image timerImage;
    public TextMeshProUGUI timerText;
    public Color startColor = Color.green;
    public Color endColor = Color.red;
    private bool startTimer;
    // Start is called before the first frame update
    void Start()
    {
        //// Set startTime to 10 minutes before the current time
        //startTime = new DateTime(2023, 12, 18, 15, 0, 52);
        ////startTime = DateTime.Now.Subtract(TimeSpan.FromMinutes(10));

        //// Set endTime to 10 minutes after the current time
        ////endTime = DateTime.Now.Add(TimeSpan.FromMinutes(10));
        //endTime = new DateTime(2023, 12, 18, 15, 2, 51);
        //Debug.Log(endTime.ToString());
    }

    // Update is called once per frame
    void Update()
    {   if (startTimer)
        {
            UpdateTimerDisplay();
        }
    }
    public void setStartAndEnd(string start, string end)
    {

        startTime = DateTime.ParseExact(start, "yyyy-MM-ddTHH:mm:ss.fffZ", null, System.Globalization.DateTimeStyles.AssumeUniversal);
        endTime = DateTime.ParseExact(end, "yyyy-MM-ddTHH:mm:ss.fffZ", null, System.Globalization.DateTimeStyles.AssumeUniversal);
        startTimer = true;
    }
    private void UpdateTimerDisplay()
    {
        // Calculate the time remaining
        TimeSpan remainingTime = endTime - DateTime.Now;
        if (remainingTime.TotalSeconds <= 0)
        {
            // Timer has ended, implement your logic here (e.g., show a message)
            //Debug.Log("Timer has ended!");
            timerText.text = "00:00";
            return; // Exit the method to prevent further updates
        }
        // Calculate the percentage of time elapsed
        float progress = Mathf.Clamp01(1 - (float)(remainingTime.TotalSeconds) / (float)(endTime - startTime).TotalSeconds);

        // Update the fill amount of the circular timer
        timerImage.fillAmount = progress;
        timerImage.color = Color.Lerp(startColor, endColor, progress);
        // Display remaining time (optional)
        timerText.text = string.Format("{0:D2}:{1:D2}", remainingTime.Minutes, remainingTime.Seconds);

        // Check if the timer has reached the end
        if (progress <= 0)
        {
            // Timer has ended, implement your logic here (e.g., show a message)
            Debug.Log("Timer has ended!");
        }
    }
}
