using System;
using TMPro;
using UnityEngine;
using BestHTTP.SocketIO3;
using BestHTTP.SocketIO3.Transports;
using BestHTTP.SocketIO3.Events;
using Newtonsoft.Json;
using SG;
using System.Collections.Generic;
using BestHTTP.JSON.LitJson;
using System.Collections;
using WCP;
using DG.Tweening;

public class ChatManager : MonoBehaviour
{
    public static ChatManager instance;
    public Socket chatSocket;
    private SocketManager Manager;
    MessageRootObject MessageRootObject;
    private UIManagerChat uiManagerChat;
    public int count = 0;
    public GameObject containerMessage;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {

    }
    IEnumerator turnonchat()
    {
        yield return new WaitForSeconds(1f);
        if(chatSocket==null || !chatSocket.IsOpen)
        {
            uiManagerChat = GetComponent<UIManagerChat>();
            Debug.Log("Connecting to chat...");
            SocketOptions options = new SocketOptions();
            options.ConnectWith = TransportTypes.WebSocket;
            options.AutoConnect = true;
            options.Timeout = TimeSpan.FromSeconds(1);
            options.AdditionalQueryParams = new PlatformSupport.Collections.ObjectModel.ObservableDictionary<string, string>();
            options.AdditionalQueryParams.Add("token", GameSocketManager.instance.authToken);
            options.AdditionalQueryParams.Add("gateway", "ludochat");
            Manager = new SocketManager(new Uri(GameSocketManager.instance.address), options);

            // Get the socket for the chat endpoint
            chatSocket = Manager.GetSocket("/ludochat");
            Manager.Open();

            chatSocket.On<ConnectResponse>(SocketIOEventTypes.Connect, OnConnected);
            chatSocket.On<ConnectResponse>(SocketIOEventTypes.Error, OnError);
            chatSocket.On<string>("newMessageSuccess", onMessage); // Current message
            chatSocket.On<string>("onError", onError);
            chatSocket.On<string>("getAllMessagesResponse", allMessages);
        }
        
    }
    public void turnOnChat()
    {
        StartCoroutine(turnonchat());
    }
    private void onMessage(string json)
    {
        Debug.Log("Message received" + json);
        //MessageRootObject messagerootObject = JsonUtility.FromJson<MessageRootObject>(json);
        MessageNotification messagerootObject = JsonConvert.DeserializeObject<MessageNotification>(json);


        if (messagerootObject.newMessage.message!=null)
        {
            // Access the data
            Debug.Log("Room ID: " + messagerootObject.newMessage.roomId);
            Debug.Log("Player ID: " + messagerootObject.newMessage.uid);
            Debug.Log("Message: " + messagerootObject.newMessage.message);
            Debug.Log("_id: " + messagerootObject.newMessage._id);
            Debug.Log("__v: " + messagerootObject.newMessage.__v);
            Debug.Log("Player ID: " + messagerootObject.uid);
            AllMessages newMessages = new AllMessages();
            newMessages.uid = messagerootObject.uid;
            newMessages.roomId = messagerootObject.newMessage.roomId;
            newMessages.message = messagerootObject.newMessage.message;
            //uiManagerChat.allMessagesList.Add(newMessages);
            uiManagerChat.notification = messagerootObject;
            uiManagerChat.ShowAllMessageInChat();
            if(messagerootObject.uid!=LudoManager.Instance.gameInfo.playerID)
            {
                UIManager.Instance.newMessage.SetActive(true);
            }
            //getAllMessages();
            //uiManagerChat.ShowAllMessageInChat();
        }
        
    }
    private void allMessages(string json)
    {
        Debug.Log("Message received" + json);
        List<AllMessages> messagerootObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AllMessages>>(json);
        DeleteChatMessages();
        if (messagerootObject != null)
        {
            uiManagerChat.allMessagesList = messagerootObject;
            uiManagerChat.DownloadAllChat();
        }
    }
    void DeleteChatMessages()
    {
        foreach (Transform child in containerMessage.transform)
        {
            // Destroy each child GameObject
            Destroy(child.gameObject);
        }
    }
    private void onError(string json)
    {
        if (json != null)
        {
            Debug.Log("Error is : " + json);
        }
    }
    public void openTheChat()
    {
        StartCoroutine(openChatBox(0.1f));
    }
    IEnumerator openChatBox(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (AnimationManager.instance.ChatClicked == true)
        {
            AnimationManager.instance.chatPanel.gameObject.SetActive(true);
            if (AnimationManager.instance.chat == false)
            {
                Debug.Log("If");
                GameSocketManager.instance.chatManager.getAllMessages();
                AnimationManager.instance.chatPanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.4f);
                yield return new WaitForSeconds(0.4f);
                AnimationManager.instance.gameBoard.transform.DOMove(AnimationManager.instance.LudoGameUpper.position, 0.3f);
                AnimationManager.instance.chat = true;
            }
            else
            {
                Debug.Log("Else");
                AnimationManager.instance.chat = false;
                AnimationManager.instance.chatPanel.transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f);
                yield return new WaitForSeconds(0.3f);
                AnimationManager.instance.gameBoard.transform.DOMove(AnimationManager.instance.LudoGameCenter.position, 0.3f);
            }
            AnimationManager.instance.ChatClicked = false;
            AnimationManager.instance.Chatbutton.interactable = true;
        }
    }
    public void getAllMessages()
    {
        GetMessages message = new GetMessages();
        message.roomId = LudoManager.Instance.matchDataManager.MatchData._id;
        message.uid = LudoManager.Instance.gameInfo.playerID;
        string json =JsonConvert.SerializeObject(message);
        chatSocket.Emit("getAllMessages", json);
        count = count + 1;
    }
    public void SenndMessage()
    {
        SendMessage msg = new SendMessage();
        msg.message = uiManagerChat.getMessage();/*"dcfhjklkjhgbvjkl"*/;
        msg.roomId = LudoManager.Instance.matchDataManager.MatchData._id;/*"room88"*/;
        msg.uid = LudoManager.Instance.gameInfo.playerID;
        string json=JsonConvert.SerializeObject(msg);
        UnityEngine.Debug.Log(json);
        if(msg.message!="" && msg.message!=string.Empty)
            chatSocket.Emit("newMessage", json)/*.On(SocketIOEventTypes.Event,sendMessage)*/;
    }
    public void sendEmoticon(string message)
    {
        SendMessage msg = new SendMessage();
        msg.message = message;
        msg.roomId = LudoManager.Instance.matchDataManager.MatchData._id;/*"room88"*/;
        msg.uid = LudoManager.Instance.gameInfo.playerID;
        string json = JsonConvert.SerializeObject(msg);
        UnityEngine.Debug.Log(json);
        if (msg.message != "" && msg.message != string.Empty)
            chatSocket.Emit("newMessage", json)/*.On(SocketIOEventTypes.Event,sendMessage)*/;
    }
    private void OnConnected(ConnectResponse resp)
    {
        MainThreadDispatcher.RunOnMainThread(() =>
        {
            StartCoroutine(playerchatStatus());
            UnityEngine.Debug.Log("Connected to Chat server");
            if (LudoManager.Instance.matchDataManager.MatchData.match_id != "")
            {
                UnityEngine.Debug.Log("Connecting again with Match ID");

                chatSocket.Emit("joinRoom", LudoManager.Instance.matchDataManager.MatchData._id);
                //getAllMessages();

            }
        });
    }
    IEnumerator playerchatStatus()
    {
        //Debug.Log("Inside Player Status");
        yield return new WaitForSeconds(0.2f);
        if (/*LudoManager.Instance.internetConnectionChecker.CheckInternetConnection() &&*/ LudoManager.Instance.gameInfo.playerID != "")
        {
            SendingID iD = new SendingID();
            iD.uid = LudoManager.Instance.gameInfo.playerID;
            string json = JsonConvert.SerializeObject(iD);
            Debug.Log("Sending chat status in chat status as ==> " + json);
            chatSocket.Emit(/*"chat-status"*/"joinPlayers", json);
        }
    }
    private void OnError(ConnectResponse resp)
    {
        MainThreadDispatcher.RunOnMainThread(() =>
        {
            UnityEngine.Debug.Log(resp.ToString());
        });
    }
    
}
public class SendMessage
{
    public string roomId;
    public string message;
    public string uid;
}
public class NewMessage
{
    public string roomId;
    public string uid;
    public string message;
    public string _id;
    public int __v;
}

public class MessageData
{
    public NewMessage newMessage;
    public string uid;
}

public class MessageRootObject
{
    public MessageData data;
}
public class ChatError
{
    public string error;
}
[Serializable]
public class AllMessages
{
    public string uid;
    public string roomId;
    public string message;
}
[Serializable]
public class GetAllMessages
{
    public AllMessages[] messages;
}
public class GetMessages
{
    public string roomId;
    public string uid;

}
public class MessageNew
{
    public string roomId;

    public string uid;

    public string message;

    public string _id;

    public int __v;
}

public class MessageNotification
{
    public MessageNew newMessage;

    public string uid;
}
