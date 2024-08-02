using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuCol : MonoBehaviour
{
    public void ReturnToRoom()
    {
        MyNetworkRoomManager.Instance.ReturnToLobby();
    }
}
