using PlayerIOClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetIDPlayer : MonoBehaviour, IFunction
{
    public void Execute(Message m)
    {
        int id = m.GetInt(0);
        GameManager.Instance.IDClient = id;
    }
}
