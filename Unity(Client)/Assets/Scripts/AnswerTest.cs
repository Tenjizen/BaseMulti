using PlayerIOClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerTest : MonoBehaviour, IFunction
{
    public void Execute(Message m)
    {
        Debug.Log(m.GetString(0));
    }


}
