using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class LobbyWindow : MonoBehaviour
{
    public TextMeshProUGUI time;
    public string currentTime;
    private DateTime current;
    private DateTime toStart;
    public string StartedAt;
    //public string currentTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void startTimer()
    {
        //this.gameObject.transform.localScale = new Vector3(0, 0, 0);
        //this.gameObject.transform.DOScale(new Vector3(1, 1, 1), 0.4f);

        // Get the current date and time
        //DateTimeOffset currentDateTimeOffset = DateTimeOffset.Now;

        // Get the time zone offset
        //TimeSpan timeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(currentDateTimeOffset.DateTime);

        // Format the date and time string
        //currentTime = currentDateTimeOffset.ToString("ddd MMM dd yyyy HH:mm:ss 'GMT'zzz") +
        //    FormatTimeZoneOffset(timeZoneOffset);


        // Parse the input string to extract the date and time
        string[] partsStartedAt = StartedAt.Split(new[] { " GMT" }, StringSplitOptions.None);
        string dateTimePartStartedAt = partsStartedAt[0];
        //string offsetPartStartedAt = partsStartedAt[1];

        // Parse the extracted date and time
        DateTime dateTimeStartedAt = DateTime.ParseExact(
            dateTimePartStartedAt,
            /*"ddd MMM dd yyyy HH:mm:ss"*/"yyyy-MM-ddTHH:mm:ss.fffZ",
            System.Globalization.CultureInfo.InvariantCulture
        );

        // Reconstruct the formatted date and time string
        StartedAt = dateTimeStartedAt.ToString("ddd MMM dd yyyy HH:mm:ss");
        currentTime = dateTimeStartedAt.ToString("ddd MMM dd yyyy HH:mm:ss");
        //string[] parts = currentTime.Split(new[] { " GMT" }, StringSplitOptions.None);
        //string dateTimePart = parts[0];
        ////string offsetPart = parts[1];

        // Parse the extracted date and time
        //DateTime dateTime = DateTime.ParseExact(
        //    dateTimePart,
        //    "ddd MMM dd yyyy HH:mm:ss",
        //    System.Globalization.CultureInfo.InvariantCulture
        //);

        // Reconstruct the formatted date and time string
        //currentTime = dateTime.ToString("ddd MMM dd yyyy HH:mm:ss");

        //Debug.Log("Formatted time: " + formattedDateTime);
        //Debug.Log("Formatted current time: " + currentTime);

        current = DateTime.ParseExact(
            currentTime,
            "ddd MMM dd yyyy HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture
        );
        toStart = DateTime.ParseExact(
            StartedAt,
            "ddd MMM dd yyyy HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture
        );

        StartCoroutine(UpdateTimeDifference());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator UpdateTimeDifference()
    {
        // Get the current date and time
        DateTimeOffset currentDateTimeOffset = DateTimeOffset.Now;

        // Get the time zone offset
        TimeSpan timeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(currentDateTimeOffset.DateTime);

        // Format the date and time string
        currentTime = currentDateTimeOffset.ToString("ddd MMM dd yyyy HH:mm:ss 'GMT'zzz") +
            FormatTimeZoneOffset(timeZoneOffset);

        // Parse the input string to extract the date and time
        string[] parts = currentTime.Split(new[] { " GMT" }, StringSplitOptions.None);
        string dateTimePart = parts[0];
        string offsetPart = parts[1];

        // Parse the extracted date and time
        DateTime dateTime = DateTime.ParseExact(
            dateTimePart,
            "ddd MMM dd yyyy HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture
        );
        // Reconstruct the formatted date and time string
        currentTime = dateTime.ToString("ddd MMM dd yyyy HH:mm:ss");

        //Debug.Log("Formatted time: " + formattedDateTime);
        //UnityEngine.Debug.Log("Formatted current time: " + currentTime);

        current = DateTime.ParseExact(
            currentTime,
            "ddd MMM dd yyyy HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture
        );
        //Debug.LogError("NOW IS " + current + "Calculated start time is  " + toStart + "   Received " + StartedAt);
        TimeSpan timeDifference = toStart - current;
        if (timeDifference.Minutes >= 0 && timeDifference.Seconds >= 0)
        {
            // Calculate the updated time difference
            time.text = "Matching "+timeDifference.Minutes + " min " + timeDifference.Seconds + " sec";
            yield return new WaitForSeconds(1f); // Wait for 1 second
            StartCoroutine(UpdateTimeDifference());
        }
        else
        {
            time.text = "Time Up!";
        }
    }
    private string FormatTimeZoneOffset(TimeSpan offset)
    {
        int hours = offset.Hours;
        int minutes = offset.Minutes;

        string sign = hours < 0 ? "-" : "+";
        hours = Math.Abs(hours);

        return string.Format("{0}{1:D2}{2:D2}", sign, hours, minutes);
    }
}
