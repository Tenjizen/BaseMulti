using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerIOClient;
using static ChessPiece;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    private Connection pioconnection;
    private List<Message> msgList = new List<Message>(); //  Messsage queue implementation
    private bool joinedroom = false;


    public GameObject ChessPiecePrefab;

    public GameObject[,] gridPos = new GameObject[8, 8];
    public GameObject[] playerBlack = new GameObject[16];
    public GameObject[] playerWhite = new GameObject[16];

    private string _currentPlayer = "w";

    private bool _gameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this; else { Debug.LogError("2 Game Manager"); }
    }

    private Type type;
    void Start()
    {

        Application.runInBackground = true;

        // Create a random userid 
        System.Random random = new System.Random();
        string userid = "Guest" + random.Next(0, 10000);

        Debug.Log("Starting");

        PlayerIO.Authenticate(
            "reseautestgame-4uhbkytuyzmzqno3js4q",            //Your game id
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



        CreatePieceGame();


    }
    void MasterServerJoined(Client client)
    {
        Debug.Log("Successfully connected to Player.IO");

        // Comment out the line below to use the live servers instead of your development server
        client.Multiplayer.DevelopmentServer = new ServerEndpoint("localhost", 8184);

        Debug.Log("CreateJoinRoom");
        //Create or join the room 
        client.Multiplayer.CreateJoinRoom(
            "UnityDemoRoom",                    //Room id. If set to null a random roomid is used
            "UnityMushrooms",                   //The room type started on the server
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
        pioconnection = connection;
        pioconnection.OnMessage += handlemessage;
        joinedroom = true;


        pioconnection.Send("TEST", "pierre", 15);
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
            switch (m.Type)
            {
                case "REPONSE":
                    Debug.Log(m.GetString(0));
                    break;
                    //case "Chat":
                    //	ChatText(m.GetString(0) + " says: " + m.GetString(1), false);
                    //	break;
            }
        }

        // clear message queue after it's been processed
        msgList.Clear();
    }




    public void CreatePieceGame()
    {
        playerWhite = new GameObject[] { Create(Type.WhiteRook, 0, 0), Create(Type.WhiteKnignt, 1, 0),
            Create(Type.WhiteBishop, 2, 0),Create(Type.WhiteQueen, 3, 0),Create(Type.WhiteKing, 4, 0),
            Create(Type.WhiteBishop, 5, 0),Create(Type.WhiteKnignt, 6, 0),Create(Type.WhiteRook, 7, 0),
            Create(Type.WhitePawn, 0, 1),Create(Type.WhitePawn, 1, 1),Create(Type.WhitePawn, 2, 1),
            Create(Type.WhitePawn, 3, 1),Create(Type.WhitePawn, 4, 1),Create(Type.WhitePawn, 5, 1),
            Create(Type.WhitePawn, 6, 1),Create(Type.WhitePawn, 7, 1)
        };
        playerBlack = new GameObject[] { Create(Type.BlackRook, 0, 7), Create(Type.BlackKnignt, 1, 7),
            Create(Type.BlackBishop, 2, 7),Create(Type.BlackQueen, 3, 7),Create(Type.BlackKing, 4, 7),
            Create(Type.BlackBishop, 5, 7),Create(Type.BlackKnignt, 6, 7),Create(Type.BlackRook, 7, 7),
            Create(Type.BlackPawn, 0, 6),Create(Type.BlackPawn, 1, 6),Create(Type.BlackPawn, 2, 6),
            Create(Type.BlackPawn, 3, 6),Create(Type.BlackPawn, 4, 6),Create(Type.BlackPawn, 5, 6),
            Create(Type.BlackPawn, 6, 6),Create(Type.BlackPawn, 7, 6)
        };

        for (int i = 0; i < playerWhite.Length; i++)
        {
            SetPosition(playerWhite[i]);
            SetPosition(playerBlack[i]);
        }
    }
    public GameObject Create(Type type, int x, int y)
    {
        GameObject go = Instantiate(ChessPiecePrefab, Vector3.zero, Quaternion.identity);
        go.name = type.ToString();
        ChessPiece cp = go.GetComponent<ChessPiece>();
        cp.TypeChess = type;
        cp.SetXBoard(x);
        cp.SetYBoard(y);
        cp.Initialiaze();
        return go;
    }
    public void SetPosition(GameObject go)
    {
        ChessPiece cp = go.GetComponent<ChessPiece>();
        gridPos[cp.GetXBoard, cp.GetYBoard] = go;
    }
    public void SetEmptyPosition(int x, int y)
    {
        gridPos[x, y] = null;
    }
    public GameObject GetPosition(int x, int y)
    {
        return gridPos[x, y];
    }
    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >=gridPos.GetLength(0)|| y>= gridPos.GetLength(1)) return false;

        return true;
    }

}
