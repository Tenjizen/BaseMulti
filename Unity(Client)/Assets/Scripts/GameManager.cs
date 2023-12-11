using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerIOClient;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    private Connection Pioconnection;
    private List<Message> msgList = new List<Message>(); //  Message queue implementation
    private bool joinedroom = false;

    private Dictionary<string, IFunction> _msgPossible = new Dictionary<string, IFunction>();

    private string _gameID = "reseautestgame-4uhbkytuyzmzqno3js4q";

    private void Awake()
    {
        if (Instance == null) Instance = this; else { Debug.LogError("2 Game Manager"); }
    }

    void Start()
    {

        Application.runInBackground = true;

        // Create a random userid 
        System.Random random = new System.Random();
        string userid = "Guest" + random.Next(0, 10000);

        Debug.Log("Starting");

        PlayerIO.Authenticate(
            _gameID,            //Your game id
            "public",                               //Your connection id
            new Dictionary<string, string> {        //Authentication arguments
				{ "userId", userid },
            },
            null,                                   //PlayerInsight segments
            MasterServerJoined,
            delegate (PlayerIOError error)
            {
                Debug.Log("Error connecting: " + error.ToString());
            }
        );
        AddFunctions();
    }

    private void AddFunctions()
    {
        _msgPossible.Add("TEST", new AnswerTest());
        _msgPossible.Add("TESTMOVE", new TestMove());
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


        Pioconnection.Send("TEST", "pierre", 15);
    }

    void handlemessage(object sender, Message m)
    {
        msgList.Add(m);
    }

    void Update()
    {
        // process message queue
        foreach (Message m in msgList)
        {
            if (!_msgPossible.TryGetValue(m.Type, out IFunction func))
            {
                Debug.LogError("no message with that type");
                msgList.Remove(m);
                return;
            }

            func.Execute(m);
        }

        // clear message queue after it's been processed
        msgList.Clear();
    }
    public void SendMovePawn(Vector2Int newPos)
    {
        print("send move pawn");
        Pioconnection.Send("MOVE", newPos.x, newPos.y);
    }
}
