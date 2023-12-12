using PlayerIOClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientJoin : MonoBehaviour, IFunction
{
    public void Execute(Message m)
    {
        foreach (var item in GameManager.Instance.PlayersGameObject)
        {
            if (item.activeInHierarchy == false)
            {
                item.SetActive(true);
                GameManager.Instance.PlayersScript.Add(item.GetComponentInChildren<LineForce>());
            }
        }
    }
}
