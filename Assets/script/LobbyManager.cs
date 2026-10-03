using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

// ロビー中の参加者リストをサーバー権威で管理し、スタート時にプレイヤーを生成するネットワークオブジェクト
public class LobbyManager : NetworkBehaviour
{
    public static LobbyManager Instance { get; private set; }

    public NetworkList<ulong> ConnectedClientIds = new NetworkList<ulong>();

    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                ConnectedClientIds.Add(clientId);
            }

            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer && NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
        }

        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        if (!ConnectedClientIds.Contains(clientId))
        {
            ConnectedClientIds.Add(clientId);
        }
    }

    private void HandleClientDisconnected(ulong clientId)
    {
        int index = ConnectedClientIds.IndexOf(clientId);
        if (index >= 0)
        {
            ConnectedClientIds.RemoveAt(index);
        }
    }

    // マスター（ホスト）がスタートボタンを押したときに呼び出す
    public void StartGame()
    {
        if (!IsServer)
        {
            return;
        }

        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += HandleGameSceneLoaded;
        NetworkManager.Singleton.SceneManager.LoadScene("main", LoadSceneMode.Single);
    }

    // Gameシーンへの遷移が全クライアントで完了してからプレイヤーを生成する
    private void HandleGameSceneLoaded(string sceneName, LoadSceneMode mode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= HandleGameSceneLoaded;

        foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            var playerObject = Instantiate(playerPrefab);
            playerObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        }
    }
}