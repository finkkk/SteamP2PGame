using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomPlayerGUI : MonoBehaviour
{
    [SerializeField]
    GameObject playerPanelPrefab;

    Button readyBtn;
    Button cancelBtn;
    Button removeBtn;
    Text playerName;
    Text readyState;
    GameObject playerlist;
    GameObject playerPanel;
    NetworkRoomPlayer player;

    private void Start()
    {
        InitializeUI();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 检查是否是当前场景
        if(scene.name == "LobbyScene")
        {
            InitializeUI();
        }
    }
    private void InitializeUI()
    {
        player = GetComponent<NetworkRoomPlayer>();
        playerlist = GameObject.FindWithTag("PlayerList");
        playerPanel = Instantiate(playerPanelPrefab,playerlist.transform) as GameObject;
        readyBtn = playerPanel.transform.Find("Ready Button").GetComponent<Button>();
        cancelBtn = playerPanel.transform.Find("Cancel Button").GetComponent<Button>();
        removeBtn = playerPanel.transform.Find("Remove Button").GetComponent<Button>();
        playerName = playerPanel.transform.Find("Player Name").GetComponent<Text>();
        readyState = playerPanel.transform.Find("Ready State").GetComponent<Text>();

        readyBtn.gameObject.SetActive(false);
        cancelBtn.gameObject.SetActive(false);
        removeBtn.gameObject.SetActive(false);

        if(NetworkClient.active && player.isLocalPlayer)
        {
            readyBtn.onClick.AddListener(OnReadyButtonClicked);
            cancelBtn.onClick.AddListener(OnCancelButtonClicked);
        }
        if(player.isServer && !player.isLocalPlayer)
        {
            removeBtn.gameObject.SetActive(true);
            removeBtn.onClick.AddListener(OnRemoveButtonClicked);
        }
    }
    private void Update()
    {
        if (playerName != null)
        {
            playerName.text = $"Player [{player.index + 1}]";
        }
        if(readyState != null)
        {
            readyState.text = player.readyToBegin ? "准备" : "未准备";
        }
        if (NetworkClient.active && player.isLocalPlayer)
        {
            if(readyBtn != null && cancelBtn != null)
            {
                readyBtn.gameObject.SetActive(!player.readyToBegin);
                cancelBtn.gameObject.SetActive(player.readyToBegin);
            }
        }
    }

    private void OnReadyButtonClicked()
    {
        readyBtn.gameObject.SetActive(false);
        cancelBtn.gameObject .SetActive(true);
        player.CmdChangeReadyState(true);
    }
    private void OnCancelButtonClicked()
    {
        cancelBtn.gameObject.SetActive(false);
        readyBtn.gameObject.SetActive(true);
        player.CmdChangeReadyState(false);
    }
    private void OnRemoveButtonClicked()
    {
        GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
        // 销毁PlayerPanel
        if(playerPanel != null)
        {
            Destroy(playerPanel.gameObject);
        }
    }

    private void OnDestroy()
    {
        readyBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();
        removeBtn.onClick.RemoveAllListeners();
    }
}
