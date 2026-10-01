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
        if (timer >6.0f)
        {
            timer = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Debug.Log("uuuu");

        Vector2 random = Random.insideUnitCircle * 5.3f;

        Vector3 pos = new Vector3(random.x, random.y, 0.0f);

    


       GameObject go=Instantiate(enemyPrefab,pos,Quaternion.identity);

        NetworkObject netObj=go.GetComponent<NetworkObject>();
        netObj.Spawn();
    }
}