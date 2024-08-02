using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby Instance;
    public Text debugText;

    private MyNetworkRoomManager _roomManager;
    private const string hostAddressKey = "HostAddress";

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> lobbyJoinRequested;
    protected Callback<LobbyEnter_t > lobbyEntered;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if(!SteamManager.Initialized)
        {
            debugText.text = "Steam初始化失败/未连接到Steam服务器";
            return;
        }
        debugText.text = "Steam初始化成功/已连接到Steam服务器";

        _roomManager = GetComponent<MyNetworkRoomManager>();

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        lobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }
    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(callback.m_eResult != EResult.k_EResultOK)
        {
            debugText.text = "Steam联机大厅创建失败";
            return;
        }
        debugText.text = "Steam联机大厅创建成功";
        _roomManager.StartHost();
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), hostAddressKey, SteamUser.GetSteamID().ToString());

    }
    private void OnLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        debugText.text = "收到加入大厅的申请";
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }
    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        debugText.text = "有玩家进入大厅";
        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), hostAddressKey);
        _roomManager.networkAddress = hostAddress;

        if(!_roomManager.isNetworkActive)
        {
            _roomManager.StartClient();
            debugText.text = "玩家正在连接到主机...请稍候...";
        }
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, _roomManager.maxConnections);
    }
}
