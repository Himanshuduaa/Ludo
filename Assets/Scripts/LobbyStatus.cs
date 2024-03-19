using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyStatus : MonoBehaviour
{
    public bool inLobby;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        inLobby = true;
    }
    private void OnDisable()
    {
        inLobby = false;
    }
}
