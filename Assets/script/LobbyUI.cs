using Unity.Netcode;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// ロビー画面のUI制御（参加者リスト表示・マスターのみスタートボタン表示）
public class LobbyUI : MonoBehaviour
{
    public static LobbyUI Instance { get; private set; }

    [SerializeField] private GameObject loginToggle;
    [SerializeField] private GameObject loginInputField;
    [SerializeField] private GameObject loginButton;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private Text participantsText;
    [SerializeField] private GameObject startButton;

    private LobbyManager m_LobbyManager;

    private void Awake()
    {
        Instance = this;
        lobbyPanel.SetActive(false);
    }

    // ログイン（ホスト起動/クライアント接続）後にDirectorから呼び出される
    public void ShowLobby()
    {
        loginToggle.SetActive(false);
        loginInputField.SetActive(false);
        loginButton.SetActive(false);

        lobbyPanel.SetActive(true);
        // スタートボタンはマスター（ホスト）にのみ表示
        startButton.SetActive(NetworkManager.Singleton.IsHost);

        StartCoroutine(WaitForLobbyManager());
    }

    private IEnumerator WaitForLobbyManager()
    {
        while (LobbyManager.Instance == null)
        {
            yield return null;
        }

        m_LobbyManager = LobbyManager.Instance;
        m_LobbyManager.ConnectedClientIds.OnListChanged += _ => RefreshParticipantsList();
        RefreshParticipantsList();
    }

    private void RefreshParticipantsList()
    {
        var builder = new StringBuilder();
        foreach (var clientId in m_LobbyManager.ConnectedClientIds)
        {
            builder.Append("Player ").Append(clientId);
            if (clientId == NetworkManager.ServerClientId)
            {
                builder.Append(" (マスター)");
            }
            builder.Append('\n');
        }

        participantsText.text = builder.ToString();
    }

    // スタートボタンのOnClickから呼び出される
    public void OnStartClicked()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            LobbyManager.Instance.StartGame();
        }
    }
}