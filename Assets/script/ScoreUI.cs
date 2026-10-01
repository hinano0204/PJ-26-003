using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class ScoreUI : MonoBehaviour
{
    public Text[] scoreTexts = new Text[4]; // InspectorでScore0〜Score3を設定
    private int[] m_cachedScores = new int[4];

    void Update()
    {
        if (!NetworkManager.Singleton.IsListening) return;

        // シーン内のすべてのPlayer2を取得
        // ※NetworkVariableは全クライアントに複製されるので、クライアントからも読み取り可能
        TestPL2[] players = FindObjectsOfType<TestPL2>();

        // OwnerClientIdの昇順でソートすることで、毎フレーム表示順を一定に保つ
        System.Array.Sort(players, (a, b) => a.OwnerClientId.CompareTo(b.OwnerClientId));

        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (scoreTexts[i] == null) continue;

            if (i < players.Length)
            {
                int score = players[i].playerScore.Value;

                // スコアが変わった場合のみUI更新（毎フレームのstring生成を抑制）
                if (score != m_cachedScores[i])
                {
                    m_cachedScores[i] = score;
                    scoreTexts[i].text = score.ToString();
                }
                scoreTexts[i].gameObject.SetActive(true);
            }
            else
            {
                // プレイヤーが存在しないスロットは非表示
                scoreTexts[i].gameObject.SetActive(false);
            }
        }
    }
}