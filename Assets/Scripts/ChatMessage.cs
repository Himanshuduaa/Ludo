using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Collections;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;
using System.Drawing.Drawing2D;
using BestHTTP.SocketIO3;
using BestHTTP.SocketIO3.Transports;
using BestHTTP.SocketIO3.Events;
using BestHTTP.JSON;
using System.Net;
using Unity.VisualScripting;
using AYellowpaper.SerializedCollections;
//using BestHTTP.SocketIO;

public class ChatMessage : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public RectTransform messageContainer;
    
    public void SetMessage(string message)
    {
        messageText.text = message;

        // Adjust the size of the message container to fit the text content.
        float preferredHeight = messageText.preferredHeight;
        messageContainer.sizeDelta = new Vector2(messageContainer.sizeDelta.x, preferredHeight);
    }
}
