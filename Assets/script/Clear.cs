//using Unity.Netcode;
//using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

//using UnityEngine.UI;
 
//public class ScoreUI : MonoBehaviour
//{
//    public Text[] scoreTexts = new Text[4]; // InspectorでScore0〜Score3を設定
//    public Text messageText;                // 勝者表示用のMessageText (未設定時はStartで自動取得)
//    private int[] m_cachedScores = new int[4];

//    void Start()
//    {
//        // messageTextが未指定の場合はシーン内から名前で検索して取得
//        if (messageText == null)
//        {
//            GameObject go = GameObject.Find("MessageText");
//            if (go != null)
//            {
//                messageText = go.GetComponent<Text>();
//            }
//        }

//        // 初期状態では勝者メッセージを空にしておく
//        if (messageText != null)
//        {
//            messageText.text = "";
//        }
//    }

//    void Update()
//    {
//        if (!NetworkManager.Singleton.IsListening) return;

//        // シーン内のすべてのPlayer2を取得
//        // ※NetworkVariableは全クライアントに複製されるので、クライアントからも読み取り可能
//        TestPL2[] players = FindObjectsOfType<TestPL2>();

//        // OwnerClientIdの昇順でソートすることで、毎フレーム表示順を一定に保つ
//        System.Array.Sort(players, (a, b) => a.OwnerClientId.CompareTo(b.OwnerClientId));

//        TestPL2 winner = null;

//        for (int i = 0; i < scoreTexts.Length; i++)
//        {
//            if (scoreTexts[i] == null) continue;

//            if (i < players.Length)
//            {
//                int score = players[i].playerScore.Value;

//                // 10点以上になったら勝者として記録
//                if (score >= 10 && winner == null)
//                {
//                    winner = players[i];
//                }

//                // スコアが変わった場合のみUI更新（毎フレームのstring生成を抑制）
//                if (score != m_cachedScores[i])
//                {
//                    m_cachedScores[i] = score;
//                    scoreTexts[i].text = score.ToString();
//                }
//                scoreTexts[i].gameObject.SetActive(true);
//            }
//            else
//            {
//                // プレイヤーが存在しないスロットは非表示
//                scoreTexts[i].gameObject.SetActive(false);
//            }
//        }

//        // 勝者が決定した場合、MessageTextに表示する
//        if (winner != null && messageText != null)
//        {
//            string colorName = "";
//            int colorIndex = (int)(winner.OwnerClientId % 4);
//            switch (colorIndex)
//            {
//                case 0: colorName = "赤"; break;
//                case 1: colorName = "青"; break;
//                case 2: colorName = "緑"; break;
//                case 3: colorName = "黄"; break;
//            }
//            string winMessage = $"プレイヤー {winner.OwnerClientId} ({colorName}) の勝利！";
//            if (messageText.text != winMessage)
//            {
//                messageText.text = winMessage;
//            }
//        }
//        else if (messageText != null && messageText.text != "")
//        {
//            messageText.text = "";
//        }
//    }
//}