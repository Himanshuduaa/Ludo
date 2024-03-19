using UnityEngine;
using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class InternetConnectionChecker : MonoBehaviour
{
    private string apiUrl;
    private UnityWebRequest request;
    private bool resultReceived = false;
    private float timeoutDuration = 5f; // Timeout duration in seconds
    private float requestStartTime;
    public void getLeaderboard()
    {
        StartCoroutine(PostRequest());
    }
    IEnumerator PostRequest()
    {
        UnityWebRequest request = new UnityWebRequest(apiUrl, "GET");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            GameSocketManager.instance.loading();
            if(GameSocketManager.instance.ludoSocket != null)
            {
                GameSocketManager.instance.ludoSocket.Disconnect();
                GameSocketManager.instance.ludoSocket = null;
            }
        }
        else
        {
            if(GameSocketManager.instance.ludoSocket == null)
                GameSocketManager.instance.startTheLudoGame();

        }
        yield return new WaitForSeconds(3f);
        StartCoroutine(PostRequest());
    }
    void Update()
    {
        if (!resultReceived && Time.realtimeSinceStartup - requestStartTime >= timeoutDuration)
        {
            Debug.Log("API request timed out");
            // Handle timeout case here
            // For example, you can abort the request or retry
            request.Abort();
            resultReceived = true;
        }
    }
    private void Start()
    {
        apiUrl = "http://" + LudoManager.Instance.gameInfo.address + ":5000/status";
        requestStartTime = Time.realtimeSinceStartup;
        request = UnityWebRequest.Get(apiUrl);
        StartCoroutine(PostRequest());
    }

}
