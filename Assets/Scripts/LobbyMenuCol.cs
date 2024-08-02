using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenuCol : MonoBehaviour
{
    [SerializeField]
    GameObject startButton;
    [SerializeField]
    Text debugText;

    private void Awake()
    {
        MyNetworkRoomManager.Instance.startGameButton = startButton;
    }

    private void Start()
    {
        startButton.SetActive(false);
    }

    public void StartGame()
    {
        MyNetworkRoomManager.Instance.StartGame();
    }
}
