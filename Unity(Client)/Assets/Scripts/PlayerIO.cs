using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerIOClient;
using System;

public class PlayerIO : MonoBehaviour
{
    private Connection Pioconnection;

    private List<Message> _msgList = new List<Message>(); //  Message queue implementation
    private bool joinedroom = false;

    private Dictionary<string, IFunction> _msgPossible = new Dictionary<string, IFunction>();

    private string _gameID = "reseautestgame-4uhbkytuyzmzqno3js4q";

    void Start()
    {

        Application.runInBackground = true;

        // Create a random userid 
        System.Random random = new System.Random();
        string userid = "Guest" + random.Next(0, 10000);

        Debug.Log("Starting");

        PlayerIOClient.PlayerIO.Authenticate(
            _gameID,            //Your game id
            "public",                               //Your connection id
            new Dictionary<string, string> {        //Authentication arguments
				{ "userId", userid },
            },
            null,                                   //PlayerInsight segments
            this.MasterServerJoined,
            delegate (PlayerIOError error)
            {
                Debug.Log("Error connecting: " + error.ToString());
            }
        );
        AddFunctions();
    }

   

    private void AddFunctions()
    {
        _msgPossible.Add("PlayerSetId", new SetIDPlayer());
        _msgPossible.Add("PlayerJoined", new PlayerJoin());
        _msgPossible.Add("MovePlayer", new MovePlayerS2C());
        _msgPossible.Add("PlayerTurn", new SwitchTurnPlayer());
    }

    void MasterServerJoined(Client client)
    {
        Debug.Log("Successfully connected to Player.IO");

        // Comment out the line below to use the live servers instead of your development server
        client.Multiplayer.DevelopmentServer = new ServerEndpoint("localhost", 8184);

        Debug.Log("CreateJoinRoom");
        //Create or join the room 
        client.Multiplayer.CreateJoinRoom(
            "Pierre",                    //Room id. If set to null a random roomid is used
            "Server",                   //The room type started on the server
            true,                               //Should the room be visible in the lobby?
            null,
            null,
            RoomJoined,
            delegate (PlayerIOError error)
            {
                Debug.Log("Error Joining Room: " + error.ToString());
            }
        );
    }
    void RoomJoined(Connection connection)
    {
        Debug.Log("Joined Room.");
        // We successfully joined a room so set up the message handler
        Pioconnection = connection;
        Pioconnection.OnMessage += handlemessage;
        joinedroom = true;

        Pioconnection.Send("PlayerJoined");
    }

    void handlemessage(object sender, Message m)
    {
        _msgList.Add(m);
    }

    void Update()
    {
        // process message queue
        foreach (Message m in _msgList)
        {
            if (!_msgPossible.TryGetValue(m.Type, out IFunction func))
            {
                Debug.LogError("no message with that type" + m.Type);
                _msgList.Remove(m);
                return;
            }

            func.Execute(m);
        }

        // clear message queue after it's been processed
        _msgList.Clear();
    }


    public void SendMove(Vector3 newPos)
    {
        Pioconnection.Send("MovePlayer", newPos.x, newPos.y, newPos.z);
    }

    public void SendEndMove()
    {
        Pioconnection.Send("PlayerTurn",GameManager.Instance.IDClient);
    }
    public void SendNextScene()
    {
        Debug.Log("next");
    }

}
