using PlayerIOClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour, IFunction
{
    public void Execute(Message m)
    {
        Debug.Log(m.GetString(0));
    }


}
