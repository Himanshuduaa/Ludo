using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManagerChat : MonoBehaviour
{

    //public ChatPanel chatPanel;
    public TMP_InputField message;
    public List<AllMessages> allMessagesList;
    public MessageNotification notification;

    public Chat chat;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(testing());
    }
    IEnumerator testing()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("Testing chat");
        var message = new Message("HIMANSHU", "ILU", 2);
        chat.ReceiveMessage(message);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public string getMessage()
    {
        string msg = message.text;
        message.text = string.Empty;
        return msg;
    }
    private void sendMessage(bool myPlayer,string msg,int index)
    {
        //if (msg != "")
        //{
        //    //chatPanel.wcp.AddChatAndUpdate(myPlayer, msg, index);

        //    var message = new Message(LudoManager.Instance.playerName, msg,0);
        //    chat.ReceiveMessage(message);
        //}

    }
    public void DisplayMessage(AllMessages Message)
    {

    }
    public void ShowAllMessageInChat()
    {
        //chatPanel.wcp.Clear();
        if(notification != null) {
            //if (notification.newMessage.uid == LudoManager.Instance.playerID)
            //{
            //    Player player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == notification.newMessage.uid);
            //    //sendMessage(false, notification.newMessage.message, player.avatar);

            //    var message = new Message(player.userName, notification.newMessage.message);
            //    chat.ReceiveMessage(message);
            //}
            //else
            //{
            //    Player player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == notification.newMessage.uid);
            //    //sendMessage(true, notification.newMessage.message, player.avatar);
            //    var message = new Message(player.userName, notification.newMessage.message);
            //    chat.ReceiveMessage(message);
            //}

            Player player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == notification.newMessage.uid);
            var message = new Message(player.userName, notification.newMessage.message,player.avatar);
            chat.ReceiveMessage(message);
        }
        
    }
    public void DownloadAllChat()
    {
        //chatPanel.wcp.Clear();
        for(int i=0;i< allMessagesList.Count;i++)
        {
            if (allMessagesList[i] != null)
            {
                //if(notification!=null)
                //{
                //    Player player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == allMessagesList[i].uid)
                //    var message = new Message(player.userName, allMessagesList[i].message, player.avatar);
                //    chat.ReceiveMessage(message);
                //}
                Player player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == allMessagesList[i].uid);
                var message = new Message(player.userName, allMessagesList[i].message, player.avatar);
                chat.ReceiveMessage(message);

            }
        }
    }
}
