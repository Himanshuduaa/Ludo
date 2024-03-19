using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardMapping : MonoBehaviour
{
    //private int whichTurn;
    public List<UserDetails> users = new List<UserDetails>();
    [Header(" Serialize Fields")]
    public RectTransform RedBottom;
    public RectTransform BlueBottom;
    public RectTransform GreenBottom;
    public RectTransform YellowBottom;
    public RectTransform GreenReverse;
    public RectTransform BlueReverse;
    public RectTransform YellowReverse;
    public RectTransform RedReverse;
    public GameObject Board;
    public GameObject DummyBoard;

    [Space]
    public RectTransform RedDummyBottom;
    public RectTransform BlueDummyBottom;
    public RectTransform GreenDummyBottom;
    public RectTransform YellowDummyBottom;
    //public RectTransform finalPosition;
    //public List<Turn> colours=new List<Turn>();
    //public List<UserDetailPosition> userDetailsPosition = new List<UserDetailPosition>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setPawns()
    {
        for (int i = 0; i < LudoManager.Instance.matchDataManager.MatchData.players.Count; i++)
        {
            for (int j = 0; j < LudoManager.Instance.matchDataManager.MatchData.players[i].pown_position.Count; j++)
            {
                if (LudoManager.Instance.matchDataManager.MatchData.players[i].pown_position[(j + 1).ToString()] == 0)
                {
                    LudoManager.Instance.matchDataManager.MatchData.players[i].pown_position[(j + 1).ToString()] = int.Parse(LudoManager.Instance.matchDataManager.MatchData.players[i].player_number + "00" + (j + 1).ToString());
                }
            }
        }
        for (int i = 0; i < LudoManager.Instance.players.Count; i++)
        {

            Player Player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == LudoManager.Instance.players[i].PlayerID);

            if (Player.player_number != 0)
            {
                LudoManager.Instance.players[i].TurnNumber = Player.player_number;

                int turn = LudoManager.Instance.players[i].TurnNumber;
                Player foundPlayer = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.player_number == turn);
                PlayerController PlayerController = LudoManager.Instance.players.Find(spot => spot.TurnNumber == turn);
                for (int j = 0; j < PlayerController.myPawns.Count; j++)
                {
                    PlayerController.myPawns[j].GetComponent<Pawnmanager>().currentIndex = foundPlayer.pown_position[(j + 1).ToString()];
                    PlayerController.myPawns[j].highlight();
                }
                if (PlayerController == null)
                {
                    UnityEngine.Debug.LogError("Null player controller");
                }
                if (PlayerController.SetSprite == null)
                {
                    PlayerController.setSpriteAction();
                    UnityEngine.Debug.LogError("Null set sprite in pc");

                }
                PlayerController.SetSprite.Invoke();

                for (int j = 0; j < LudoManager.Instance.players[i].myPawns.Count; j++)
                {
                    LudoManager.Instance.players[i].myPawns[j].GetComponent<Pawnmanager>().currentIndex = foundPlayer.pown_position[(j + 1).ToString()];
                    PlayerController.myPawns[j].highlight();
                }
                LudoManager.Instance.players[i].SetSprite.Invoke();

            }

        }

        for (int i = 0; i < LudoManager.Instance.matchDataManager.MatchData.players.Count; i++)
        {
            PlayerController player = LudoManager.Instance.players.Find(spot => spot.TurnNumber == LudoManager.Instance.matchDataManager.MatchData.players[i].player_number);
            if (player != null)
            {
                SpotsMap foundSpot = new SpotsMap();
                for (int j = 0; j < player.myPawns.Count; j++)
                {
                    //Debug.Log("I : " + i + " j : " + j + " player.myPawns.Count" + player.myPawns.Count);
                    if (player.myPawns.Count == 0)
                    {
                        Debug.Log("NULL SPOT");
                    }
                    foundSpot = LudoManager.Instance.spotsMap.Find(spot => spot.index == LudoManager.Instance.matchDataManager.MatchData.players[i].pown_position.ElementAt(j).Value);
                    if (foundSpot == null)
                    {
                        Debug.Log("NULL SPOT");
                    }
                    Pawnmanager foundPawn = player.myPawns[j];
                    if (foundPawn.transform.parent.name != "Inside")
                    {
                        foundPawn.gameObject.SetActive(true);
                        foundPawn.transform.position = foundSpot.transform.position;
                        foundPawn.transform.SetParent(foundSpot.transform, true);
                    }
                }
            }
        }
        for (int i = 0; i < LudoManager.Instance.spotsMap.Count; i++)
        {
            LudoManager.Instance.spotsMap[i].setPlayerOnSpot();
        }
        LudoManager.Instance.setPawnsScale();

        for (int i = 0; i < LudoManager.Instance.players.Count; i++)
        {
            for (int j = 0; j < LudoManager.Instance.players[i].myPawns.Count; j++)
            {
                LudoManager.Instance.players[i].myPawns[j].highlight();
            }
        }
    }
    public void setUsers() // Setting users profile according to their turn
    {
        if(LudoManager.Instance.matchDataManager.MatchData.players.Count==4)
        {
            if (LudoManager.Instance.localPlayerTurn == PlayerTurn.One)
            {
                for (int i = 0; i < 4; i++)
                {
                    users[i].Turn = i + 1;
                }
            }
            if (LudoManager.Instance.localPlayerTurn == PlayerTurn.Two)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                        users[i].Turn = 2;
                    if (i == 1)
                        users[i].Turn = 3;
                    if (i == 2)
                        users[i].Turn = 4;
                    if (i == 3)
                        users[i].Turn = 1;
                }
            }
            if (LudoManager.Instance.localPlayerTurn == PlayerTurn.Three)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                        users[i].Turn = 3;
                    if (i == 1)
                        users[i].Turn = 4;
                    if (i == 2)
                        users[i].Turn = 1;
                    if (i == 3)
                        users[i].Turn = 2;
                }
            }
            if (LudoManager.Instance.localPlayerTurn == PlayerTurn.Four)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                        users[i].Turn = 4;
                    if (i == 1)
                        users[i].Turn = 1;
                    if (i == 2)
                        users[i].Turn = 2;
                    if (i == 3)
                        users[i].Turn = 3;
                }
            }
        }
        if (LudoManager.Instance.matchDataManager.MatchData.players.Count == 3)
        {
            if (LudoManager.Instance.localPlayerTurn == PlayerTurn.One)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    users[i].Turn = i + 1;
                }
            }
            if (LudoManager.Instance.localPlayerTurn == PlayerTurn.Two)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (i == 0)
                        users[i].Turn = 2;
                    if (i == 1)
                        users[i].Turn = 3;
                    if (i == 2)
                        users[i].Turn = 4;
                    if (i == 3)
                        users[i].Turn = 1;
                }
            }
            if (LudoManager.Instance.localPlayerTurn == PlayerTurn.Three)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (i == 0)
                        users[i].Turn = 3;
                    if (i == 1)
                        users[i].Turn = 4;
                    if (i == 2)
                        users[i].Turn = 1;
                    if (i == 3)
                        users[i].Turn = 2;
                }
            }
            
        }
        if (LudoManager.Instance.matchDataManager.MatchData.players.Count == 2)
        {
            if (LudoManager.Instance.localPlayerTurn == PlayerTurn.One)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (i == 0)
                        users[i].Turn = 1;
                    if (i == 1)
                        users[i].Turn = 2;
                    if (i == 2)
                        users[i].Turn = 3;
                    if (i == 3)
                        users[i].Turn = 4;
                }
            }
            if (LudoManager.Instance.localPlayerTurn == PlayerTurn.Three)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (i == 0)
                        users[i].Turn = 3;
                    if (i == 1)
                        users[i].Turn = 4;
                    if (i == 2)
                        users[i].Turn = 1;
                    if (i == 3)
                        users[i].Turn = 2;
                }
            }
            
        }

    }

    public void StateCheck(PlayerPosition CurrentLocalPosition)
    {
        //setUsers();
        if (CurrentLocalPosition == PlayerPosition.RedBottom)
        {
            Board.transform.GetComponent<RectTransform>().rotation = RedBottom.rotation;
            Board.transform.GetComponent<RectTransform>().position = RedBottom.position;
            DummyBoard.transform.GetComponent<RectTransform>().position = RedDummyBottom.position;
            
            return;
        }
        else if (CurrentLocalPosition == PlayerPosition.BlueBottom)
        {
            DummyBoard.transform.GetComponent<RectTransform>().position = BlueDummyBottom.position;

            Board.transform.GetComponent<RectTransform>().position = BlueBottom.position;
            Board.transform.GetComponent<RectTransform>().rotation = BlueBottom.rotation;
            
            return;
        }
        else if (CurrentLocalPosition == PlayerPosition.GreenBottom)
        {
            
            DummyBoard.transform.GetComponent<RectTransform>().position = GreenDummyBottom.position;

            Board.transform.GetComponent<RectTransform>().rotation = GreenBottom.rotation;
            Board.transform.GetComponent<RectTransform>().position = GreenBottom.position;
            for (int i = 0; i < LudoManager.Instance.players.Count; i++)
            {
                if (LudoManager.Instance.players[i].myColour == Turn.Green)
                {
                    LudoManager.Instance.players[i].gameObject.GetComponent<RectTransform>().position = GreenReverse.position;
                    LudoManager.Instance.players[i].gameObject.GetComponent<RectTransform>().rotation = GreenReverse.rotation;
                }
                else if (LudoManager.Instance.players[i].myColour == Turn.Blue)
                {
                    LudoManager.Instance.players[i].gameObject.GetComponent<RectTransform>().position = BlueReverse.position;
                    LudoManager.Instance.players[i].gameObject.GetComponent<RectTransform>().rotation = BlueReverse.rotation;
                }
                else if (LudoManager.Instance.players[i].myColour == Turn.Yellow)
                {
                    LudoManager.Instance.players[i].gameObject.GetComponent<RectTransform>().position = YellowReverse.position;
                    LudoManager.Instance.players[i].gameObject.GetComponent<RectTransform>().rotation = YellowReverse.rotation;
                }
                else if (LudoManager.Instance.players[i].myColour == Turn.Red)
                {
                    LudoManager.Instance.players[i].gameObject.GetComponent<RectTransform>().position = RedReverse.position;
                    LudoManager.Instance.players[i].gameObject.GetComponent<RectTransform>().rotation = RedReverse.rotation;
                }
            }
            return;
        }
        else if (CurrentLocalPosition == PlayerPosition.YellowBottom)
        {
            
            DummyBoard.transform.GetComponent<RectTransform>().position = YellowDummyBottom.position;

            Board.transform.GetComponent<RectTransform>().position = YellowBottom.position;
            Board.transform.GetComponent<RectTransform>().rotation = YellowBottom.rotation;
            return;
        }
    }
    public void setWatcherGame()
    {
        setUsers();
        //List<PlayerController> list = new List<PlayerController>();
        //for (int i = 0; i < LudoManager.Instance.players.Count; i++)
        //{
        //    for (int j = 0; j < LudoManager.Instance.players[i].myPawns.Count; j++)
        //    {
        //        LudoManager.Instance.players[i].pawnState(false);//.myPawns[j].gameObject.SetActive(false);
        //    }
        //}
        //if (LudoManager.Instance.MaxPlayers == 4)
        //{
        //    Turn turncolour = Turn.Red;
        //    PlayerController playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
        //    Player player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == LudoManager.Instance.matchDataManager.MatchData.players[0].uid);
        //    playerController.TurnNumber = player.player_number;
        //    playerController.setPlayerID(player.uid);
        //    playerController.PlayerID = player.uid;
        //    //users[0].Turn = player.player_number;
        //    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
        //    LudoManager.Instance.whichturn = playerController.TurnNumber;
        //    list.Add(playerController);

        //    turncolour = Turn.Green;
        //    playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
        //    playerController.TurnNumber = LudoManager.Instance.GetNextTurn(LudoManager.Instance.whichturn);
        //    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.player_number == playerController.TurnNumber);
        //    playerController.setPlayerID(player.uid);
        //    playerController.PlayerID = player.uid;
        //    //users[1].Turn = player.player_number;
        //    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
        //    list.Add(playerController);

        //    turncolour = Turn.Yellow;
        //    playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
        //    playerController.TurnNumber = LudoManager.Instance.GetNextTurn(LudoManager.Instance.whichturn);
        //    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.player_number == playerController.TurnNumber);
        //    playerController.setPlayerID(player.uid);
        //    playerController.PlayerID = player.uid;
        //    //users[2].Turn = player.player_number;
        //    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
        //    list.Add(playerController);

        //    turncolour = Turn.Blue;
        //    playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
        //    playerController.TurnNumber = LudoManager.Instance.GetNextTurn(LudoManager.Instance.whichturn);
        //    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.player_number == playerController.TurnNumber);
        //    playerController.setPlayerID(player.uid);
        //    playerController.PlayerID = player.uid;
        //    //users[3].Turn = player.player_number;
        //    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
        //    list.Add(playerController);
        //}
        //if (LudoManager.Instance.MaxPlayers == 2)
        //{
        //    Turn turncolour = Turn.Red;
        //    PlayerController playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
        //    Player player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == /*LudoManager.Instance.playerID*/LudoManager.Instance.matchDataManager.MatchData.players[0].uid);
        //    playerController.TurnNumber = player.player_number;
        //    playerController.setPlayerID(player.uid);
        //    playerController.PlayerID = player.uid;
        //    //users[0].Turn = player.player_number;
        //    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
        //    LudoManager.Instance.whichturn = playerController.TurnNumber;
        //    list.Add(playerController);

        //    turncolour = Turn.Yellow;
        //    playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
        //    playerController.TurnNumber = LudoManager.Instance.GetNextTurn(LudoManager.Instance.whichturn);
        //    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.player_number == playerController.TurnNumber);
        //    playerController.setPlayerID(player.uid);
        //    playerController.PlayerID = player.uid;
        //    //users[2].Turn = player.player_number;
        //    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
        //    list.Add(playerController);

        //}
        //else if (LudoManager.Instance.MaxPlayers == 3)
        //{
        //    Turn turncolour = Turn.Red;
        //    PlayerController playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
        //    Player player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == /*LudoManager.Instance.playerID*/LudoManager.Instance.matchDataManager.MatchData.players[0].uid);
        //    playerController.TurnNumber = player.player_number;
        //    playerController.setPlayerID(player.uid);
        //    playerController.PlayerID = player.uid;
        //    //users[0].Turn = player.player_number;
        //    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
        //    LudoManager.Instance.whichturn = playerController.TurnNumber;
        //    list.Add(playerController);

        //    turncolour = Turn.Green;
        //    playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
        //    playerController.TurnNumber = LudoManager.Instance.GetNextTurn(LudoManager.Instance.whichturn);
        //    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.player_number == playerController.TurnNumber);
        //    playerController.setPlayerID(player.uid);
        //    playerController.PlayerID = player.uid;
        //    //users[1].Turn = player.player_number;
        //    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
        //    list.Add(playerController);

        //    turncolour = Turn.Yellow;
        //    playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
        //    playerController.TurnNumber = LudoManager.Instance.GetNextTurn(LudoManager.Instance.whichturn);
        //    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.player_number == playerController.TurnNumber);
        //    playerController.setPlayerID(player.uid);
        //    playerController.PlayerID = player.uid;
        //    //users[2].Turn = player.player_number;
        //    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
        //    list.Add(playerController);
        //}
        //LudoManager.Instance.players = list;
        //for (int i = 0; i < LudoManager.Instance.players.Count; i++)
        //{
        //    for (int j = 0; j < LudoManager.Instance.players[i].myPawns.Count; j++)
        //    {
        //        LudoManager.Instance.players[i].myPawns[j].GetComponent<Pawnmanager>().PawnNumber = j + 1;
        //        LudoManager.Instance.players[i].myPawns[j].gameObject.SetActive(true);

        //    }
        //}
        List<PlayerController> list = new List<PlayerController>();
        for (int i = 0; i < LudoManager.Instance.players.Count; i++)
        {
            for (int j = 0; j < LudoManager.Instance.players[i].myPawns.Count; j++)
            {
                LudoManager.Instance.players[i].myPawns[j].gameObject.SetActive(false);
            }
        }
        if (LudoManager.Instance.MaxPlayers == 4)
        {
            Turn turncolour = settingLocalPlayerColour();
            int next = 0;
            Player player = new Player();
            for (int i = 0; i < LudoManager.Instance.MaxPlayers; i++)
            {
                if (i == 0)
                {
                    PlayerController playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
                    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == LudoManager.Instance.matchDataManager.MatchData.players[0].uid);
                    playerController.TurnNumber = player.player_number;
                    playerController.PlayerID = player.uid;
                    playerController.setPlayerID(player.uid);
                    LudoManager.Instance.whichturn = playerController.TurnNumber;
                    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
                    list.Add(playerController);
                }
                if (i != 0)
                {
                    turncolour = getColour(player.player_number, turncolour);
                    PlayerController playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
                    playerController.TurnNumber = LudoManager.Instance.GetNextTurn(LudoManager.Instance.whichturn);
                    playerController.PlayerID = player.uid;
                    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.player_number == playerController.TurnNumber);
                    playerController.setPlayerID(player.uid);
                    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
                    list.Add(playerController);
                }

            }
        }
        else if (LudoManager.Instance.MaxPlayers == 2)
        {
            Turn turncolour = settingLocalPlayerColour();
            int next = 0;
            Player player = new Player();


            for (int i = 0; i < LudoManager.Instance.MaxPlayers; i++)
            {
                //Turn turncolour = Turn.Red;
                if (i == 0)
                {
                    PlayerController playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
                    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == LudoManager.Instance.matchDataManager.MatchData.players[0].uid);
                    playerController.TurnNumber = player.player_number;
                    playerController.setPlayerID(player.uid);
                    playerController.PlayerID = player.uid;
                    LudoManager.Instance.whichturn = playerController.TurnNumber;
                    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
                    list.Add(playerController);
                }
                if (i != 0)
                {

                    turncolour = getColour(player.player_number, turncolour);
                    PlayerController playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
                    playerController.TurnNumber = LudoManager.Instance.GetNextTurn(LudoManager.Instance.whichturn);
                    playerController.PlayerID = player.uid;
                    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.player_number == playerController.TurnNumber);
                    playerController.setPlayerID(player.uid);
                    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
                    list.Add(playerController);
                }

            }
        }
        else if (LudoManager.Instance.MaxPlayers == 3)
        {
            Turn turncolour = settingLocalPlayerColour();
            int next = 0;
            Player player = new Player();
            for (int i = 0; i < LudoManager.Instance.MaxPlayers; i++)
            {
                //Turn turncolour = Turn.Red;
                if (i == 0)
                {
                    PlayerController playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
                    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == LudoManager.Instance.matchDataManager.MatchData.players[0].uid);
                    playerController.TurnNumber = player.player_number;
                    playerController.PlayerID = player.uid;
                    LudoManager.Instance.whichturn = playerController.TurnNumber;
                    //users[i].Turn = player.player_number;
                    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
                    list.Add(playerController);
                    next = player.player_number;
                }
                if (i != 0)
                {
                    turncolour = getColour(player.player_number, turncolour);
                    PlayerController playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
                    next = GetNextTurn(next);
                    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.player_number == next);
                    playerController.TurnNumber = player.player_number;
                    playerController.PlayerID = player.uid;
                    LudoManager.Instance.whichturn = playerController.TurnNumber;
                    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
                    list.Add(playerController);
                }
            }
        }
        if (LudoManager.Instance.MaxPlayers != 0)
        {
            LudoManager.Instance.players = list;
            for (int i = 0; i < LudoManager.Instance.players.Count; i++)
            {
                for (int j = 0; j < LudoManager.Instance.players[i].myPawns.Count; j++)
                {
                    LudoManager.Instance.players[i].myPawns[j].GetComponent<Pawnmanager>().PawnNumber = j + 1;
                    LudoManager.Instance.players[i].myPawns[j].gameObject.SetActive(true);
                }
            }
        }
    }
    public void SetColour()
    {
        //setUsers();

        List<PlayerController> list = new List<PlayerController>();
        for (int i = 0; i < LudoManager.Instance.players.Count; i++)
        {
            for (int j = 0; j < LudoManager.Instance.players[i].myPawns.Count; j++)
            {
                LudoManager.Instance.players[i].myPawns[j].gameObject.SetActive(false);
            }
        }
        if (LudoManager.Instance.MaxPlayers == 4)
        {
            Turn turncolour = settingLocalPlayerColour();
            int next = 0;
            Player player = new Player();
            for (int i = 0; i < LudoManager.Instance.MaxPlayers; i++)
            {
                if (i == 0)
                {
                    PlayerController playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
                    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == LudoManager.Instance.gameInfo.playerID);
                    playerController.TurnNumber = player.player_number;
                    playerController.PlayerID = player.uid;
                    playerController.setPlayerID(player.uid);
                    LudoManager.Instance.whichturn = playerController.TurnNumber;
                    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
                    list.Add(playerController);
                }
                if (i != 0)
                {
                    turncolour = getColour(player.player_number, turncolour);
                    PlayerController playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
                    playerController.TurnNumber = LudoManager.Instance.GetNextTurn(LudoManager.Instance.whichturn);
                    playerController.PlayerID = player.uid;
                    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.player_number == playerController.TurnNumber);
                    playerController.setPlayerID(player.uid);
                    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
                    list.Add(playerController);
                }
                
            }
        }
        else if (LudoManager.Instance.MaxPlayers == 2)
        {
            Turn turncolour=settingLocalPlayerColour();
            int next = 0;
            Player player = new Player();
            

            for (int i = 0; i < LudoManager.Instance.MaxPlayers; i++)
            {
                //Turn turncolour = Turn.Red;
                if (i == 0)
                {
                    PlayerController playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
                    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == LudoManager.Instance.gameInfo.playerID);
                    playerController.TurnNumber = player.player_number;
                    playerController.setPlayerID(player.uid);
                    playerController.PlayerID = player.uid;
                    LudoManager.Instance.whichturn = playerController.TurnNumber;
                    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
                    list.Add(playerController);
                }
                if (i != 0)
                {
                    
                    turncolour = getColour(player.player_number, turncolour);
                    PlayerController playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
                    playerController.TurnNumber = LudoManager.Instance.GetNextTurn(LudoManager.Instance.whichturn);
                    playerController.PlayerID = player.uid;
                    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.player_number == playerController.TurnNumber);
                    playerController.setPlayerID(player.uid);
                    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
                    list.Add(playerController);
                }

            }
        }
        else if (LudoManager.Instance.MaxPlayers == 3)
        {
            Turn turncolour=settingLocalPlayerColour();
            int next = 0;
            Player player = new Player();
            for(int i=0;i< LudoManager.Instance.MaxPlayers;i++)
            {
                //Turn turncolour = Turn.Red;
                if(i==0)
                {
                    PlayerController playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
                    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == LudoManager.Instance.gameInfo.playerID);
                    playerController.TurnNumber = player.player_number;
                    playerController.PlayerID = player.uid;
                    LudoManager.Instance.whichturn = playerController.TurnNumber;
                    //users[i].Turn = player.player_number;
                    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
                    list.Add(playerController);
                    next = player.player_number;
                }
                if (i != 0)
                {
                    turncolour = getColour(player.player_number, turncolour);
                    PlayerController playerController = LudoManager.Instance.players.Find(spot => spot.myColour == turncolour);
                    next=GetNextTurn(next);
                    player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.player_number ==next);
                    playerController.TurnNumber = player.player_number;
                    playerController.PlayerID = player.uid;
                    LudoManager.Instance.whichturn = playerController.TurnNumber;
                    playerController.userDetails = users.Find(spot => spot.Turn == player.player_number);
                    list.Add(playerController);
                }
            }
        }
        if(LudoManager.Instance.MaxPlayers!=0)
        {
            LudoManager.Instance.players = list;
            for (int i = 0; i < LudoManager.Instance.players.Count; i++)
            {
                for (int j = 0; j < LudoManager.Instance.players[i].myPawns.Count; j++)
                {
                    LudoManager.Instance.players[i].myPawns[j].GetComponent<Pawnmanager>().PawnNumber = j + 1;
                    LudoManager.Instance.players[i].myPawns[j].gameObject.SetActive(true);
                }
            }
        }    
    }
    public Turn settingLocalPlayerColour()
    {
        Turn turncolour=new Turn();
        //PlayerController playerController = players.Find(spot => spot.PlayerID == playerID);
        Player player;
        if (LudoManager.Instance.watcher==false)
        {
            player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == LudoManager.Instance.gameInfo.playerID);
        }
        else
        {
            player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == LudoManager.Instance.matchDataManager.MatchData.players[0].uid);
        }
        string color=player.color;
        if (color == "Red")
        {
            turncolour = Turn.Red;
        }
        else if (color == "Blue")
        {
            turncolour = Turn.Blue;
        }
        else if (color == "Green")
        {
            turncolour = Turn.Green;
        }
        else if (color == "Yellow")
        {
            turncolour = Turn.Yellow;
        }
        return turncolour;
    }
    private void getLocalPlayerTurnNumber()
    {
        for (int i = 0; i < LudoManager.Instance.players.Count; i++)
        {
            Player Player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == LudoManager.Instance.players[i].PlayerID);
            LudoManager.Instance.players[i].TurnNumber = Player.player_number;
        }
    }
    public void setBoard()// Setting the spots index according to player local position as the spots are fixed and not rotating with the board
    {
        getLocalPlayerTurnNumber();
        if (LudoManager.Instance.localPlayerTurn == PlayerTurn.One)
        {
            int homecount = 0;
            // Defining the path
            for (int i = 0; i < 52; i++)
            {
                LudoManager.Instance.spotsMap[i].GetComponent<SpotsMap>().index = i + 1;
                //Defining  safe zone
                if (LudoManager.Instance.SafeZoneIndex.Contains(LudoManager.Instance.spotsMap[i].GetComponent<SpotsMap>().index))
                {
                    LudoManager.Instance.spotsMap[i].GetComponent<SpotsMap>().star = true;
                }
            }
            int count = 1;
            LudoManager.Instance.homePoints[0].HomeIndex = count;

            int counter = 1;
            for (int j = 52; j < 57; j++)
            {
                LudoManager.Instance.spotsMap[j].GetComponent<SpotsMap>().index = int.Parse(count.ToString() + "0" + counter.ToString());

                counter++;
            }
            for (int i = 72; i < 76; i++)
            {
                homecount = homecount + 1;
                LudoManager.Instance.spotsMap[i].index = int.Parse(count.ToString() + "00" + (homecount).ToString());
            }
            count = 2;
            LudoManager.Instance.homePoints[1].HomeIndex = count;
            homecount = 0;
            for (int i = 76; i < 80; i++)
            {
                homecount = homecount + 1;
                LudoManager.Instance.spotsMap[i].index = int.Parse(count.ToString() + "00" + (homecount).ToString());
            }
            counter = 1;
            for (int j = 57; j < 62; j++)
            {
                LudoManager.Instance.spotsMap[j].GetComponent<SpotsMap>().index = int.Parse(count.ToString() + "0" + counter.ToString());
                counter++;
            }
            count = 3;
            homecount = 0;
            for (int i = 80; i < 84; i++)
            {
                homecount = homecount + 1;
                LudoManager.Instance.spotsMap[i].index = int.Parse(count.ToString() + "00" + (homecount).ToString());
            }
            LudoManager.Instance.homePoints[2].HomeIndex = count;

            counter = 1;
            for (int j = 62; j < 67; j++)
            {
                LudoManager.Instance.spotsMap[j].GetComponent<SpotsMap>().index = int.Parse(count.ToString() + "0" + counter.ToString());
                counter++;
            }
            count = 4;
            homecount = 0;
            for (int i = 84; i < 88; i++)
            {
                homecount = homecount + 1;
                LudoManager.Instance.spotsMap[i].index = int.Parse(count.ToString() + "00" + (homecount).ToString());
            }
            LudoManager.Instance.homePoints[3].HomeIndex = count;

            counter = 1;
            for (int j = 67; j < 72; j++)
            {
                LudoManager.Instance.spotsMap[j].GetComponent<SpotsMap>().index = int.Parse(count.ToString() + "0" + counter.ToString());
                counter++;
            }
            LudoManager.Instance.spotsMap[88].GetComponent<SpotsMap>().index = 106;
            LudoManager.Instance.spotsMap[89].GetComponent<SpotsMap>().index = 206;
            LudoManager.Instance.spotsMap[90].GetComponent<SpotsMap>().index = 306;
            LudoManager.Instance.spotsMap[91].GetComponent<SpotsMap>().index = 406;
        }
        else if (LudoManager.Instance.localPlayerTurn == PlayerTurn.Two)
        {
            int a = 1;
            for (int i = 0; i < 52; i++)
            {
                int index = i + 1 + 13;
                if (index > 52)
                {
                    index = 0;
                    LudoManager.Instance.spotsMap[i].GetComponent<SpotsMap>().index = a;
                    a = a + 1;
                }
                else
                    LudoManager.Instance.spotsMap[i].GetComponent<SpotsMap>().index = index;

                if (LudoManager.Instance.SafeZoneIndex.Contains(LudoManager.Instance.spotsMap[i].GetComponent<SpotsMap>().index))
                {
                    LudoManager.Instance.spotsMap[i].GetComponent<SpotsMap>().star = true;
                }
            }
            int count = 2;
            LudoManager.Instance.homePoints[0].HomeIndex = count;
            int homecount = 0;
            for (int i = 72; i < 76; i++)
            {
                homecount = homecount + 1;
                LudoManager.Instance.spotsMap[i].index = int.Parse(count.ToString() + "00" + (homecount).ToString());
            }

            int counter = 1;
            for (int j = 52; j < 57; j++)
            {
                LudoManager.Instance.spotsMap[j].GetComponent<SpotsMap>().index = int.Parse(count.ToString() + "0" + counter.ToString());
                counter++;
            }
            count = 3;
            homecount = 0;
            for (int i = 76; i < 80; i++)
            {
                homecount = homecount + 1;
                LudoManager.Instance.spotsMap[i].index = int.Parse(count.ToString() + "00" + (homecount).ToString());
            }
            LudoManager.Instance.homePoints[1].HomeIndex = count;
            counter = 1;
            for (int j = 57; j < 62; j++)
            {
                LudoManager.Instance.spotsMap[j].GetComponent<SpotsMap>().index = int.Parse(count.ToString() + "0" + counter.ToString());
                counter++;
            }
            count = 4;
            homecount = 0;
            for (int i = 80; i < 84; i++)
            {
                homecount = homecount + 1;
                LudoManager.Instance.spotsMap[i].index = int.Parse(count.ToString() + "00" + (homecount).ToString());
            }
            LudoManager.Instance.homePoints[2].HomeIndex = count;

            counter = 1;
            for (int j = 62; j < 67; j++)
            {
                LudoManager.Instance.spotsMap[j].GetComponent<SpotsMap>().index = int.Parse(count.ToString() + "0" + counter.ToString());
                counter++;
            }
            count = 1;
            homecount = 0;
            for (int i = 84; i < 88; i++)
            {
                homecount = homecount + 1;
                LudoManager.Instance.spotsMap[i].index = int.Parse(count.ToString() + "00" + (homecount).ToString());
            }
            LudoManager.Instance.homePoints[3].HomeIndex = count;

            counter = 1;
            for (int j = 67; j < 72; j++)
            {
                LudoManager.Instance.spotsMap[j].GetComponent<SpotsMap>().index = int.Parse(count.ToString() + "0" + counter.ToString());
                counter++;
            }
            LudoManager.Instance.spotsMap[88].GetComponent<SpotsMap>().index = 206;
            LudoManager.Instance.spotsMap[89].GetComponent<SpotsMap>().index = 306;
            LudoManager.Instance.spotsMap[90].GetComponent<SpotsMap>().index = 406;
            LudoManager.Instance.spotsMap[91].GetComponent<SpotsMap>().index = 106;
        }
        else if (LudoManager.Instance.localPlayerTurn == PlayerTurn.Three)
        {
            int a = 1;
            for (int i = 0; i < 52; i++)
            {
                int index = i + 1 + 26;
                if (index > 52)
                {
                    index = 0;
                    LudoManager.Instance.spotsMap[i].GetComponent<SpotsMap>().index = a;
                    a = a + 1;
                }
                else
                    LudoManager.Instance.spotsMap[i].GetComponent<SpotsMap>().index = index;

                if (LudoManager.Instance.SafeZoneIndex.Contains(LudoManager.Instance.spotsMap[i].GetComponent<SpotsMap>().index))
                {
                    LudoManager.Instance.spotsMap[i].GetComponent<SpotsMap>().star = true;
                }
            }
            int count = 3;
            LudoManager.Instance.homePoints[0].HomeIndex = count;
            int homecount = 0;
            for (int i = 72; i < 76; i++)
            {
                homecount = homecount + 1;
                LudoManager.Instance.spotsMap[i].index = int.Parse(count.ToString() + "00" + (homecount).ToString());
            }
            int counter = 1;
            for (int j = 52; j < 57; j++)
            {
                LudoManager.Instance.spotsMap[j].GetComponent<SpotsMap>().index = int.Parse(count.ToString() + "0" + counter.ToString());
                counter++;
            }
            count = 4;
            homecount = 0;
            for (int i = 76; i < 80; i++)
            {
                homecount = homecount + 1;
                LudoManager.Instance.spotsMap[i].index = int.Parse(count.ToString() + "00" + (homecount).ToString());
            }
            LudoManager.Instance.homePoints[1].HomeIndex = count;

            counter = 1;
            for (int j = 57; j < 62; j++)
            {
                LudoManager.Instance.spotsMap[j].GetComponent<SpotsMap>().index = int.Parse(count.ToString() + "0" + counter.ToString());
                counter++;
            }
            count = 1;
            homecount = 0;
            for (int i = 80; i < 84; i++)
            {
                homecount = homecount + 1;
                LudoManager.Instance.spotsMap[i].index = int.Parse(count.ToString() + "00" + (homecount).ToString());
            }
            LudoManager.Instance.homePoints[2].HomeIndex = count;

            counter = 1;
            for (int j = 62; j < 67; j++)
            {
                LudoManager.Instance.spotsMap[j].GetComponent<SpotsMap>().index = int.Parse(count.ToString() + "0" + counter.ToString());
                counter++;
            }
            count = 2;
            homecount = 0;
            for (int i = 84; i < 88; i++)
            {
                homecount = homecount + 1;
                LudoManager.Instance.spotsMap[i].index = int.Parse(count.ToString() + "00" + (homecount).ToString());
            }
            LudoManager.Instance.homePoints[3].HomeIndex = count;

            counter = 1;
            for (int j = 67; j < 72; j++)
            {
                LudoManager.Instance.spotsMap[j].GetComponent<SpotsMap>().index = int.Parse(count.ToString() + "0" + counter.ToString());
                counter++;
            }
            LudoManager.Instance.spotsMap[88].GetComponent<SpotsMap>().index = 306;
            LudoManager.Instance.spotsMap[89].GetComponent<SpotsMap>().index = 406;
            LudoManager.Instance.spotsMap[90].GetComponent<SpotsMap>().index = 106;
            LudoManager.Instance.spotsMap[91].GetComponent<SpotsMap>().index = 206;
        }
        else if (LudoManager.Instance.localPlayerTurn == PlayerTurn.Four)
        {
            int a = 1;
            for (int i = 0; i < 52; i++)
            {
                int index = i + 1 + 39;
                if (index > 52)
                {
                    index = 0;
                    LudoManager.Instance.spotsMap[i].GetComponent<SpotsMap>().index = a;
                    a = a + 1;
                }
                else
                    LudoManager.Instance.spotsMap[i].GetComponent<SpotsMap>().index = index;

                if (LudoManager.Instance.SafeZoneIndex.Contains(LudoManager.Instance.spotsMap[i].GetComponent<SpotsMap>().index))
                {
                    LudoManager.Instance.spotsMap[i].GetComponent<SpotsMap>().star = true;
                }
            }
            int count = 4;
            int homecount = 0;
            for (int i = 72; i < 76; i++)
            {
                homecount = homecount + 1;
                LudoManager.Instance.spotsMap[i].index = int.Parse(count.ToString() + "00" + (homecount).ToString());
            }
            LudoManager.Instance.homePoints[0].HomeIndex = count;

            int counter = 1;
            for (int j = 52; j < 57; j++)
            {
                LudoManager.Instance.spotsMap[j].GetComponent<SpotsMap>().index = int.Parse(count.ToString() + "0" + counter.ToString());
                counter++;
            }
            count = 1;
            homecount = 0;
            homecount = 0;
            for (int i = 76; i < 80; i++)
            {
                homecount = homecount + 1;
                LudoManager.Instance.spotsMap[i].index = int.Parse(count.ToString() + "00" + (homecount).ToString());
            }
            LudoManager.Instance.homePoints[1].HomeIndex = count;

            counter = 1;
            for (int j = 57; j < 62; j++)
            {
                LudoManager.Instance.spotsMap[j].GetComponent<SpotsMap>().index = int.Parse(count.ToString() + "0" + counter.ToString());
                counter++;
            }
            count = 2;
            homecount = 0;
            for (int i = 80; i < 84; i++)
            {
                homecount = homecount + 1;
                LudoManager.Instance.spotsMap[i].index = int.Parse(count.ToString() + "00" + (homecount).ToString());
            }
            LudoManager.Instance.homePoints[2].HomeIndex = count;

            counter = 1;
            for (int j = 62; j < 67; j++)
            {
                LudoManager.Instance.spotsMap[j].GetComponent<SpotsMap>().index = int.Parse(count.ToString() + "0" + counter.ToString());
                counter++;
            }
            count = 3;
            LudoManager.Instance.homePoints[3].HomeIndex = count;

            counter = 1;
            homecount = 0;
            for (int i = 84; i < 88; i++)
            {
                homecount = homecount + 1;
                LudoManager.Instance.spotsMap[i].index = int.Parse(count.ToString() + "00" + (homecount).ToString());
            }
            for (int j = 67; j < 72; j++)
            {
                LudoManager.Instance.spotsMap[j].GetComponent<SpotsMap>().index = int.Parse(count.ToString() + "0" + counter.ToString());
                counter++;
            }
            LudoManager.Instance.spotsMap[88].GetComponent<SpotsMap>().index = 406;
            LudoManager.Instance.spotsMap[89].GetComponent<SpotsMap>().index = 106;
            LudoManager.Instance.spotsMap[90].GetComponent<SpotsMap>().index = 206;
            LudoManager.Instance.spotsMap[91].GetComponent<SpotsMap>().index = 306;
        }
    }
    public Turn getColour(int index, Turn turn)
    {
        int newIndex = GetNextTurn(index);
        if(LudoManager.Instance.MaxPlayers == 3)
        {
            if (index == 3)
            {
                turn = GetNextColor(turn, 2);
            }
            else
            {
                turn = GetNextColor(turn, 1);
            }
        }
        if(LudoManager.Instance.MaxPlayers == 2)
        {
            turn = GetNextColor(turn, 2);
        }
        if (LudoManager.Instance.MaxPlayers == 4)
        {
            turn = GetNextColor(turn, 1);
        }
        return turn;
    }
    public Turn GetNextColor(Turn currentColor,int jump)
    {
        // Get the values of the Turn enum
        Turn[] allColors = (Turn[])Enum.GetValues(typeof(Turn));

        // Find the index of the current color
        int currentIndex = Array.IndexOf(allColors, currentColor);

        // Calculate the index of the next color, wrapping around if necessary
        int nextIndex = (currentIndex + jump) % allColors.Length;

        // Return the next color
        return allColors[nextIndex];
    }
    public int GetNextTurn(int currentTurn)
    {
        if (LudoManager.Instance.MaxPlayers == 4)
        {
            currentTurn = currentTurn + 1;
            if (currentTurn > 4)
            {
                currentTurn = 1;
            }
            return currentTurn;
        }
        else if (LudoManager.Instance.MaxPlayers == 2)
        {
            if (currentTurn == 3)
            {
                currentTurn = 1;
            }
            else
            {
                currentTurn = 3;
            }
            return currentTurn;
        }
        else
        {
            currentTurn = currentTurn + 1;
            if (currentTurn > 3)
            {
                currentTurn = 1;
            }
            return currentTurn;
        }
    }
}

