using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Void : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<LineForce>();
        if (player != null)
        {
            player.ResetOldPos();
        }
    }
}
