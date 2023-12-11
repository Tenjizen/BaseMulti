using PlayerIOClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFunction 
{
    public void Execute(Message m);
}
