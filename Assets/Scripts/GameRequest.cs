using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameRequest : MonoBehaviour
{
    public TextMeshProUGUI avatarName;
    public TextMeshProUGUI mode1;
    public TextMeshProUGUI mode2;
    public TextMeshProUGUI amount;
    public Image avatarDP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        avatarName.text ="";
        mode1.text ="";
        mode2.text ="";
        amount.text ="";
        avatarDP.sprite =null;
    }
    public void setGameRequest(GameReq details)
    {
        this.gameObject.SetActive(true);
        if (details != null)
        {
            avatarName.text = details.name;
            mode1.text = details.mode1;
            mode2.text = details.mode2;
            amount.text = details.amount;
            avatarDP.sprite =UIManager.Instance.avatars[int.Parse(details.avatarIndex)].sprite; ;
        }
    }
}
public class GameReq
{
    public string avatarIndex;
    public string name;
    public string mode1;
    public string mode2;
    public string amount;
    public string uid;
}
