using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    public GameObject enemyPrefab; // 敵のプレハブ（あらかじめ登録しておく）
    private float timer = 0f;

    void Update()
    {
        // 【超重要】自分が「サーバー（ホスト）」でなければ、これ以降の処理をしない！
        // これがないと、参加者全員がバラバラに敵を作ってしまいます。
        if (!IsServer) return;

        // 3秒経過ごとに敵を出す処理
        timer += Time.deltaTime;
        if (timer >3.0f)
        {
            timer = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Debug.Log("uuuu");
        // 1. まずは普通のUnityと同じように作る（この時点では自分のPCにしか存在しない）
        //Vector2 random = Random.insideUnitCircle * 4f; // 半径4

        //Vector3 pos = new Vector3(random.x, random.y, 0);

        Vector3 pos = new Vector3(Random.Range(-9.25f, 9.25f), Random.Range(-5.25f, 5.25f), 0);
        if (pos.x > 3.7f && pos.x < 3.7f && (pos.y > 4.25f || pos.y < -4.25f))
        {
            pos.x = Random.Range(-9.25f, 9.25f);
            pos.y = Random.Range(-5.25f, 5.25f);
            pos.z = 0;



            return;
        }
        ;




        GameObject go = Instantiate(enemyPrefab, pos, Quaternion.identity);

        // 2. 【NGO特有】これをネットワーク全体に「公開」する！
        // プレハブには NetworkObject コンポーネントがついている必要があります。
        NetworkObject netObj = go.GetComponent<NetworkObject>();
        netObj.Spawn(); // これで全プレイヤーの画面に敵が現れます
    }
}