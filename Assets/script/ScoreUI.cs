using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using static UnityEditor.Experimental.GraphView.GraphView;
public class ScoreUI : MonoBehaviour
{
    public Text[] scoreTexts = new Text[4];

    // PL1～PL4の勝利画像
    public Image[] winImages = new Image[4];

    // ロビーに戻るボタン
    public Button lobbyButton;

    private int[] m_cachedScores = new int[4];

    private bool gameFinished = false;

    void Start()
    {
        // 勝利画像を非表示
        for (int i = 0; i < winImages.Length; i++)
        {
            if (winImages[i] != null)
            {
                winImages[i].gameObject.SetActive(false);
            }
        }

        // ロビーに戻るボタンも最初は非表示
        if (lobbyButton != null)
        {
            lobbyButton.gameObject.SetActive(false);

            // ボタンのクリック処理を登録
            lobbyButton.onClick.AddListener(ReturnToLobby);
        }
    }

    void Update()
    {
        if (!NetworkManager.Singleton.IsListening)
            return;

        // ゲーム終了後は処理しない
        if (gameFinished)
            return;

        TestPL2[] players = FindObjectsOfType<TestPL2>();

        // ClientId順
        System.Array.Sort(
            players,
            (a, b) => a.OwnerClientId.CompareTo(b.OwnerClientId)
        );

        TestPL2 winner = null;

        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (scoreTexts[i] == null)
                continue;

            if (i < players.Length)
            {
                int score = players[i].playerScore.Value;

                // 10点以上で勝者
                if (score >= 1 && winner == null)
                {
                    winner = players[i];
                }

                // スコア更新
                if (score != m_cachedScores[i])
                {
                    m_cachedScores[i] = score;
                    scoreTexts[i].text = score.ToString();
                }

                scoreTexts[i].gameObject.SetActive(true);
            }
            else
            {
                scoreTexts[i].gameObject.SetActive(false);
            }
        }

        // 勝者が決まった
        if (winner != null)
        {
            gameFinished = true;

            // いったん全部の勝利画像を非表示
            for (int i = 0; i < winImages.Length; i++)
            {
                if (winImages[i] != null)
                {
                    winImages[i].gameObject.SetActive(false);
                }
            }

            // 勝者の画像を表示
            int winnerIndex = (int)winner.OwnerClientId;

            if (winnerIndex >= 0 &&
                winnerIndex < winImages.Length)
            {
                if (winImages[winnerIndex] != null)
                {
                    winImages[winnerIndex].gameObject.SetActive(true);
                }
            }

            // ロビーに戻るボタンを表示
            if (lobbyButton != null)
            {
                lobbyButton.gameObject.SetActive(true);
            }
        }
    }

    // ロビーに戻る
    private void ReturnToLobby()
    {
        // ホストだけが実行
        if (!NetworkManager.Singleton.IsServer)
            return;

        // Gameシーン内のNetworkObjectを全部取得
        NetworkObject[] networkObjects =
            FindObjectsOfType<NetworkObject>();

        foreach (NetworkObject obj in networkObjects)
        {
            // NetworkManager自身は除外
            if (obj.gameObject == NetworkManager.Singleton.gameObject)
                continue;

            // SpawnされているものだけDespawn
            if (obj.IsSpawned)
            {
                obj.Despawn(true);
            }
        }

        // Lobbyへ移動
        NetworkManager.Singleton.SceneManager.LoadScene(
            "Slect",
            UnityEngine.SceneManagement.LoadSceneMode.Single
        );
    
        //if (!NetworkManager.Singleton.IsServer)
        //    return;

        //NetworkManager.Singleton.SceneManager.LoadScene(
        //    "Slect",
        //    LoadSceneMode.Single
        //);
    }
}