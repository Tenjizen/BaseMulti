using PlayerIOClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoin : MonoBehaviour, IFunction
{
    public void Execute(Message m)
    {
        int id = m.GetInt(0);

        var gameManager = GameManager.Instance;

        var ptsSpawn = gameManager.PlayersGameObject[id - 1];
        var mesh = gameManager.PlayerColors[id - 1];

        GameObject go = Instantiate(gameManager.PlayerPrefab, ptsSpawn.transform.position, Quaternion.identity, ptsSpawn.transform);
        go.GetComponent<MeshFilter>().mesh = mesh;
        go.GetComponent<LineForce>().SetId(id);
    }
}
