using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIHandler : MonoBehaviour
{
    public string baseUrl = "http://localhost:5008/";

    private void Start()
    {
        //GetMessages("654ce91e7a2788b911f45c1b");
        //StartCoroutine(GetRequest("http://192.168.0.193:5008/api/game/messages/room8"));
    }
    // Function to get all messages of a room
    public void GetMessages(string roomId)
    {
        roomId = "654ce91e7a2788b911f45c1b";
        string apiUrl = baseUrl + "messages/" + roomId;
        baseUrl = "http://192.168.0.193:8300/api/game/save-game";
        //StartCoroutine(GetRequest(apiUrl));
    }

    // Coroutine for handling the GET request
    IEnumerator GetRequest()
    {
        //using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        //{
        //    // Request and wait for the desired page.
        //    yield return webRequest.SendWebRequest();

        //    // Check for errors
        //    if (webRequest.isNetworkError || webRequest.isHttpError)
        //    {
        //        Debug.LogError("Error: " + webRequest.error);
        //    }
        //    else
        //    {
        //        // Print the response results
        //        Debug.Log("Response: " + webRequest.downloadHandler.text);
        //    }
        //}

        ////using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.193:8300/api/game/save-game", "{ \"field1\": 1, \"field2\": 2 }", "application/json"))
        //using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.193:8300/api/game/save-game","",""))
        //{
        //    yield return www.SendWebRequest();

        //    if (www.result != UnityWebRequest.Result.Success)
        //    {
        //        Debug.Log(www.error);
        //    }
        //    else
        //    {
        //        Debug.Log("Form upload complete!");
        //    }
        //}






        Debug.Log("Calling api ");

        //WWWForm form = new WWWForm();
        //form.AddField("roomId", "room8");

        UnityWebRequest www = UnityWebRequest.Get("http://192.168.0.193:5008/api/game/messages/room8");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }

    IEnumerator GetRequest(string uri)
    {
        Debug.Log("Calling api  " +uri);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }
}