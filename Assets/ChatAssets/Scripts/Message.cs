using System;
using UnityEngine;

[Serializable]
public class Message
{
  public string Sender;
  public string Content;
  public DateTime SendTime;
    public int avatarIndex;

  public Message(string sender, string content,int index)
  {
    Sender = sender;
    Content = content;
    SendTime = DateTime.Now;
    avatarIndex = index;
  }
}