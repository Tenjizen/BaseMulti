using PlayerIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
class MovePlayerS2C : IFunction
{
    public void Execute(Message m)
    {
        float newPosX = m.GetFloat(0);
        float newPosY = m.GetFloat(1);
        float newPosZ = m.GetFloat(2);

        int id = m.GetInt(3);

        if (id == GameManager.Instance.IDClient) { return; }

        var player = GameManager.Instance.PlayersScript[id - 1].gameObject;
        Vector3 pos = new Vector3(newPosX, newPosY, newPosZ);
        player.transform.localPosition = pos;
    }
}
