using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using AYellowpaper.SerializedCollections;
using System;
using Newtonsoft.Json;
using System.Diagnostics;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    public TextMeshProUGUI Mode;
    public TextMeshProUGUI NoOfPlayers;
    public TextMeshProUGUI Time;
    public TextMeshProUGUI EntryPrice;
    public TextMeshProUGUI WinningPrice;
    public TextMeshProUGUI MaxPlayersCountTMP;
    public Button joinLobbyButton;
    public Button joinWatcherButton;
    //public string CreatedAt;
    //public string UpdatedAt;
    //public string CreatedBy;
    public string StartedAt;
    public string currentTime;
    public string id;
    public string win_with_token;
    public string mode;
    public int MaxPlayersCount;
    public bool active;

    private DateTime current;
    private DateTime toStart;
    //[SerializedDictionary("Player ID", "Colour")]
    //public SerializedDictionary<string, JoinedPlayerInformation> PlayersInLobby = new SerializedDictionary<string, JoinedPlayerInformation>();

    // Start is called before the first frame update
    void Start()
    {
        //this.gameObject.transform.localScale = new Vector3(0, 0, 0);
        //this.gameObject.transform.DOScale(new Vector3(1, 1, 1), 0.4f);

        // Get the current date and time
        DateTimeOffset currentDateTimeOffset = DateTimeOffset.Now;

        // Get the time zone offset
        TimeSpan timeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(currentDateTimeOffset.DateTime);

        // Format the date and time string
        currentTime = currentDateTimeOffset.ToString("ddd MMM dd yyyy HH:mm:ss 'GMT'zzz") +
            FormatTimeZoneOffset(timeZoneOffset);


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
    private string FormatTimeZoneOffset(TimeSpan offset)
    {
        int hours = offset.Hours;
        int minutes = offset.Minutes;

        string sign = hours < 0 ? "-" : "+";
        hours = Math.Abs(hours);

        return string.Format("{0}{1:D2}{2:D2}", sign, hours, minutes);
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
        if (timeDifference.Minutes>=0 && timeDifference.Seconds>=0)
        {
            // Calculate the updated time difference
            Time.text = timeDifference.Minutes + " min " + timeDifference.Seconds + " sec";
            yield return new WaitForSeconds(1f); // Wait for 1 second
            StartCoroutine(UpdateTimeDifference());
        }
        else
        {
            Time.text = "Time Up!";
            joinLobbyButton.GetComponent<Button>().interactable = false;
            joinLobbyButton.gameObject.SetActive(false);
            //joinWatcherButton.GetComponent<Button>().interactable = true;
            //joinWatcherButton.gameObject.SetActive(true);
        }
    }
    public void joinLobby()
    {
        LudoManager.Instance.lobbymanager.playOnline();
        joinLobbyButton.interactable = false;
        LudoManager.Instance.lobbymanager.activeLobby.ModeOfGameJoining = mode;
        LudoManager.Instance.lobbymanager.activeLobby.TokenToWinInJoining = win_with_token;
        LudoManager.Instance.lobbymanager.activeLobby.LobbyIDJoining = id;
        LudoManager.Instance.lobbymanager.activeLobby.MaxPlayersCount = MaxPlayersCount;
        UIManager.Instance.whatToShow("SelectJoiningColour", false);
    }
    public void joinAsWatcher()
    {
        if (GameSocketManager.instance.connected && LudoManager.Instance.gameInfo.playerID != string.Empty)
        {
            LudoManager.Instance.lobbymanager.playOnline();
            SendingID iD = new SendingID();
            iD.uid = LudoManager.Instance.gameInfo.playerID;
            iD.lobby_id = id;
            string json = JsonConvert.SerializeObject(iD);
            UnityEngine.Debug.Log("Sending watcher json as ==> " + json);
            GameSocketManager.instance.ludoSocket.Emit("join-watcher", json);
        }
        else if (!GameSocketManager.instance.connected)
            GameSocketManager.instance.loading();
    }
}
public class JoinLobby
{
    public string lobby_id;
    public string uid;
    public string color;
    public int avatar;
}
public class GamePlayerInformation
{
    public int avatarIndex;
    public string name;
    public string colorChosen;
}
public class Error
{
    public int statusCode;
    public string message;
}
