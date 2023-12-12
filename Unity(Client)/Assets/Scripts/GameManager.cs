using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public PlayerIO PlayerIORef;

    public int PlayerIDTurn = 1;

    public TextMeshProUGUI TurnText;

    public List<LineForce> PlayersScript;
    public List<GameObject> PlayersGameObject;

    public GameObject PlayerPrefab;
    public List<Mesh> PlayerColors;


    public int IDClient;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("2 instance of game manager");
        }
    }

    void Update()
    {
        if (PlayersScript.Count > 0)
        {
            CheckIfCanPlay();
        }
    }

    public void CheckIfCanPlay()
    {
        if (PlayersScript[PlayerIDTurn - 1].InHole == true)
        {
            PlayerIORef.SendEndMove();
        }
    }

    public void CheckIfNextLevel()
    {
        foreach (var item in PlayersScript)
        {
            if (item.InHole == false) return;
        }
        PlayerIORef.SendNextScene();
    }

}
