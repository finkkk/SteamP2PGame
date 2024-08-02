using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkRoomManager : NetworkRoomManager
{
    public static MyNetworkRoomManager Instance;

    public GameObject startGameButton;


    public override void Awake()
    {
        base.Awake();
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void Start()
    {
        base.Start();

        if(Utils.IsSceneActive(RoomScene) && startGameButton != null)
        {
            startGameButton.SetActive(false);
        }
    }

    public override void OnRoomServerPlayersReady()
    {
        if (Utils.IsSceneActive(RoomScene) && startGameButton != null)
        {
            startGameButton.SetActive(true);
        }
    }

    public override void OnRoomServerPlayersNotReady()
    {
        if (Utils.IsSceneActive(RoomScene) && startGameButton != null)
        {
            startGameButton.SetActive(false);
        }
    }

    public override void OnGUI()
    {
        if(!showRoomGUI)
        {
            return;
        }
        if(!Utils.IsSceneActive(RoomScene))
        {
            return;
        }
    }

    #region UI°´Å¥ÀàÖ´ÐÐÂß¼­
    public void StartGame()
    {
        ServerChangeScene(GameplayScene);
    }

    public void ReturnToLobby()
    {
        if(NetworkServer.active && Utils.IsSceneActive(GameplayScene))
        {
            ServerChangeScene(RoomScene);
        }
    }
    #endregion


}
