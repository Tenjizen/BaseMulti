using PlayerIOClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTurnPlayer : MonoBehaviour, IFunction
{
    public void Execute(Message m)
    {
        int index = m.GetInt(0);
        GameManager.Instance.PlayerIDTurn = index;
        GameManager.Instance.TurnText.text = $"Player {index}, it's your turn";
    }

}
