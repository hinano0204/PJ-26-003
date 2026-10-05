using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
//[RequireComponent(typeof(NavMeshAgent))]

//public class EnemyMove : NetworkBehaviour
//{

//    //[SerializeField] private PlayerController _playerController;
//    public Transform player;
//    public float speed = 5f;
//    public GameObject Enemy;
//    public float avoidDistance = 2f;
//    public float rayDistance = 1f;

//    //敵が最初にいた場所
//    private Vector3 spawnPosition;

//    //リスポーン中かどうか
//    private bool isRespawning = false;

//    private void Start()
//    {
//        //最初のエネミーの位置だよ
//        spawnPosition = transform.position;

//        Invoke(nameof(Update), 5f);
//        Enemy.SetActive(false);
//    }
//    void Update()
//    {

//        Enemy.SetActive(true);
//        //リスポーン中なら移動しない
//        if (isRespawning)
//            return;

//        //サーバが敵を動かす
//        if (!IsServer)  //サーバじゃなかったら何もしない
//            return;

//        //一番近いプレイヤーを探す
//        FindNearestPlayer();

//        //プレイヤーがいなければなんもしないよ
//        if (player == null)
//            return;

//        Vector3 pos = transform.position;
//        Vector3 targetpos = player.position;
//        Vector3 dir = (targetpos - pos).normalized;

//        RaycastHit hit;



//        if (Physics.Raycast(transform.position,dir,out hit,rayDistance))   
//        {
//            // 障害物発見
//            //上いける？
//            bool canGoUp = !Physics.Raycast
//                (
//                transform.position,
//                Vector3.up,
//                avoidDistance
//                );

//            //下いける？
//            bool canGoDoun = !Physics.Raycast
//                (
//                transform.position,
//                Vector3.down,
//                avoidDistance
//                );

//            if(canGoUp)
//            {
//                targetpos = transform.position + Vector3.up * avoidDistance;
//            }

//            else if(canGoDoun)
//            {
//                targetpos = transform.position + Vector3.down * avoidDistance;
//            }
//        }

//        transform.position =
//            Vector3.MoveTowards
//            (
//                transform.position,
//                targetpos,
//                speed * Time.deltaTime
//            );
//    }

//    //------------------------
//    //一番近いプレイヤーを探す
//    //------------------------
//    private void FindNearestPlayer()
//    {
//        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
//        float nearestDistance = Mathf.Infinity;
//        Transform nearestPlayer = null;
//        foreach( GameObject p in players )
//        {
//            float distance = Vector3.Distance
//                (
//                transform.position, p.transform.position
//                );
//            if(distance < nearestDistance )
//            {
//                nearestDistance = distance;
//                nearestPlayer = p.transform;
//            }
//        }
//        player = nearestPlayer;
//    }

//    //--------------------
//    //プレイヤーにぶつかった
//    //--------------------

//    private void OnCollisionEnter(Collision collision)
//    {
//        //サーバだけ
//        if (!IsServer)
//            return;

//        //Playerタグじゃないなら無視
//        if (!collision.gameObject.CompareTag("Player"))
//            return;
//        //すでにリスポーン中ならむし
//        if (isRespawning)
//            return;

//        StartCoroutine(RespawnEnemy());
//    }

//    //-------------------
//    //敵をリスポーン
//    //-------------------

//    private IEnumerator RespawnEnemy()
//    {
//        isRespawning = true;

//        //けす
//        Enemy.SetActive(false);

//        //１０秒待つ
//        yield return new WaitForSeconds(10f);

//        //最初の場所に戻す
//        transform.position = spawnPosition;

//        //敵出現
//        Enemy.SetActive(true);
//        isRespawning = false;
//    }
//}

//using System.Collections;
//using Unity.Netcode;
//using UnityEngine;


//public class EnemyMove : NetworkBehaviour
//{
//    // ------------------------
//    // プレイヤー
//    // ------------------------

//    public Transform player;

//    // 敵の移動速度
//    public float speed = 5f;

//    // ------------------------
//    // 障害物回避
//    // ------------------------

//    public float avoidDistance = 2f;
//    public float rayDistance = 1f;

//    // ------------------------
//    // リスポーン
//    // ------------------------

//    // 敵が最初にいた場所
//    private Vector3 spawnPosition;

//    // リスポーン中かどうか
//    private bool isRespawning = false;

//    // 敵の見た目
//    private Renderer[] enemyRenderers;

//    // 敵の当たり判定
//    private Collider enemyCollider;


//    // ========================
//    // Start
//    // ========================

//    private void Start()
//    {
//        // 最初の位置を保存
//        spawnPosition = transform.position;

//        // Enemy自身と子オブジェクトにある
//        // Rendererを全部取得
//        enemyRenderers =
//            GetComponentsInChildren<Renderer>(true);

//        // Colliderを取得
//        enemyCollider =
//            GetComponent<Collider>();
//    }


//    // ========================
//    // Update
//    // ========================

//    private void Update()
//    {
//        // リスポーン中なら動かさない
//        if (isRespawning)
//            return;

//        // サーバーだけが敵を動かす
//        if (!IsServer)
//            return;

//        // 一番近いプレイヤーを探す
//        FindNearestPlayer();

//        // プレイヤーがいなければ何もしない
//        if (player == null)
//            return;


//        // ------------------------
//        // プレイヤー方向
//        // ------------------------

//        Vector3 pos = transform.position;

//        Vector3 targetPos =
//            player.position;

//        Vector3 dir =
//            (targetPos - pos).normalized;


//        // ------------------------
//        // 障害物チェック
//        // ------------------------

//        RaycastHit hit;

//        if (Physics.Raycast(
//            transform.position,
//            dir,
//            out hit,
//            rayDistance))
//        {
//            // 上に行けるか
//            bool canGoUp =
//                !Physics.Raycast(
//                    transform.position,
//                    Vector3.up,
//                    avoidDistance
//                );

//            // 下に行けるか
//            bool canGoDown =
//                !Physics.Raycast(
//                    transform.position,
//                    Vector3.down,
//                    avoidDistance
//                );


//            if (canGoUp)
//            {
//                targetPos =
//                    transform.position +
//                    Vector3.up * avoidDistance;
//            }
//            else if (canGoDown)
//            {
//                targetPos =
//                    transform.position +
//                    Vector3.down * avoidDistance;
//            }
//        }


//        // ------------------------
//        // 敵を移動
//        // ------------------------

//        transform.position =
//            Vector3.MoveTowards(
//                transform.position,
//                targetPos,
//                speed * Time.deltaTime
//            );
//    }


//    // ========================
//    // 一番近いプレイヤーを探す
//    // ========================

//    private void FindNearestPlayer()
//    {
//        GameObject[] players =
//            GameObject.FindGameObjectsWithTag("Player");

//        float nearestDistance =
//            Mathf.Infinity;

//        Transform nearestPlayer = null;


//        foreach (GameObject p in players)
//        {
//            float distance =
//                Vector3.Distance(
//                    transform.position,
//                    p.transform.position
//                );


//            if (distance < nearestDistance)
//            {
//                nearestDistance = distance;
//                nearestPlayer = p.transform;
//            }
//        }


//        player = nearestPlayer;
//    }


//    // ========================
//    // プレイヤーと衝突
//    // ========================

//    private void OnCollisionEnter(
//        Collision collision)
//    {
//        // サーバーだけが処理する
//        if (!IsServer)
//            return;


//        // Playerタグじゃなければ無視
//        if (!collision.gameObject.CompareTag("Player"))
//            return;


//        // ========================
//        // プレイヤーのスコアを0にする
//        // ========================

//        TestPL2 playerScript =
//            collision.gameObject.GetComponent<TestPL2>();


//        if (playerScript != null)
//        {
//            //playerScript.playerScore.Value = 0;
//            playerScript.DecreaseScore();

//        }

//        // すでにリスポーン中なら無視
//        if (isRespawning)
//            return;


//        // リスポーン開始
//        StartCoroutine(
//            RespawnEnemy()
//        );
//    }


//    // ========================
//    // 敵を消してリスポーン
//    // ========================

//    private IEnumerator RespawnEnemy()
//    {
//        // リスポーン中
//        isRespawning = true;


//        // ========================
//        // 敵を消す
//        // ========================

//        // 見た目を消す
//        if (enemyRenderers != null)
//        {
//            foreach (
//                Renderer renderer
//                in enemyRenderers)
//            {
//                renderer.enabled = false;
//            }
//        }


//        // 当たり判定を消す
//        if (enemyCollider != null)
//        {
//            enemyCollider.enabled = false;
//        }


//        // ========================
//        // 10秒待つ
//        // ========================

//        yield return new WaitForSeconds(10f);


//        // ========================
//        // 最初の位置へ戻す
//        // ========================

//        transform.position =
//            spawnPosition;


//        // ========================
//        // 敵を復活
//        // ========================

//        // 見た目を戻す
//        if (enemyRenderers != null)
//        {
//            foreach (
//                Renderer renderer
//                in enemyRenderers)
//            {
//                renderer.enabled = true;
//            }
//        }


//        // 当たり判定を戻す
//        if (enemyCollider != null)
//        {
//            enemyCollider.enabled = true;
//        }


//        // リスポーン終了
//        isRespawning = false;
//    }
//}



public class EnemyMove : NetworkBehaviour
{
    // ========================
    // 設定
    // ========================

    public Transform player;
    public float speed = 5f;

    public float avoidDistance = 2f;
    public float rayDistance = 1f;


    // ========================
    // リスポーン
    // ========================

    private Vector3 spawnPosition;
    private bool isRespawning = false;


    // ========================
    // 敵の表示・当たり判定
    // ========================

    private Renderer[] enemyRenderers;
    private Collider enemyCollider;


    // ========================
    // Start
    // ========================

    //private void Start()
    //{
    //    // 最初の位置を保存
    //    //spawnPosition = transform.position;

    //    //// Rendererを取得
    //    //enemyRenderers =
    //    //    GetComponentsInChildren<Renderer>(true);

    //    //// Colliderを取得
    //    //enemyCollider =
    //    //    GetComponent<Collider>()

    //}
    private void Start()
    {
        spawnPosition = transform.position;

        enemyRenderers = GetComponentsInChildren<Renderer>(true);
        enemyCollider = GetComponent<Collider>();

        // 最初は非表示
        foreach (Renderer renderer in enemyRenderers)
            renderer.enabled = false;

        if (enemyCollider != null)
            enemyCollider.enabled = false;

        StartCoroutine(InitialSpawn());
    }

    private IEnumerator InitialSpawn()
    {
        yield return new WaitForSeconds(5f);

        foreach (Renderer renderer in enemyRenderers)
            renderer.enabled = true;

        if (enemyCollider != null)
            enemyCollider.enabled = true;
    }


    // ========================
    // Update
    // ========================

    private void Update()
    {
        // リスポーン中なら動かない
        if (isRespawning)
            return;

        // サーバーだけが敵を動かす
        if (!IsServer)
            return;

        // 一番近いプレイヤーを探す
        FindNearestPlayer();

        if (player == null)
            return;


        // ========================
        // プレイヤー方向
        // ========================

        Vector3 targetPos =
            player.position;

        Vector3 dir =
            (targetPos - transform.position).normalized;


        // ========================
        // 障害物チェック
        // ========================

        RaycastHit hit;

        if (Physics.Raycast(
            transform.position,
            dir,
            out hit,
            rayDistance))
        {
            bool canGoUp =
                !Physics.Raycast(
                    transform.position,
                    Vector3.up,
                    avoidDistance
                );

            bool canGoDown =
                !Physics.Raycast(
                    transform.position,
                    Vector3.down,
                    avoidDistance
                );


            if (canGoUp)
            {
                targetPos =
                    transform.position +
                    Vector3.up * avoidDistance;
            }
            else if (canGoDown)
            {
                targetPos =
                    transform.position +
                    Vector3.down * avoidDistance;
            }
        }


        // ========================
        // 移動
        // ========================

        transform.position =
            Vector3.MoveTowards(
                transform.position,
                targetPos,
                speed * Time.deltaTime
            );
    }


    // ========================
    // 一番近いプレイヤー
    // ========================

    private void FindNearestPlayer()
    {
        GameObject[] players =
            GameObject.FindGameObjectsWithTag("Player");

        float nearestDistance =
            Mathf.Infinity;

        Transform nearestPlayer = null;


        foreach (GameObject p in players)
        {
            float distance =
                Vector3.Distance(
                    transform.position,
                    p.transform.position
                );


            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPlayer = p.transform;
            }
        }


        player = nearestPlayer;
    }


    // =========================================================
    // 通常のCollision
    // =========================================================

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsServer)
            return;

        Debug.Log(
            "Enemy Collision: "
            + collision.gameObject.name
            + " / Tag: "
            + collision.gameObject.tag
        );


        if (!collision.gameObject.CompareTag("Player"))
            return;


        HitPlayer(collision.gameObject);
    }


    // =========================================================
    // Trigger
    // =========================================================

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer)
            return;

        Debug.Log(
            "Enemy Trigger: "
            + other.gameObject.name
            + " / Tag: "
            + other.gameObject.tag
        );


        if (!other.CompareTag("Player"))
            return;


        HitPlayer(other.gameObject);
    }


    // =========================================================
    // Playerに当たった処理
    // =========================================================

    private void HitPlayer(GameObject hitObject)
    {
        Debug.Log(
            "EnemyがPlayerに命中: "
            + hitObject.name
        );


        // Player自身または親からTestPL2を探す
        TestPL2 playerScript =
            hitObject.GetComponentInParent<TestPL2>();


        // 見つからなかった場合
        if (playerScript == null)
        {
            Debug.LogWarning(
                "TestPL2が見つかりません！"
            );

            return;
        }


        // ========================
        // スコアを1減らす
        // ========================

        playerScript.DecreaseScore();


        Debug.Log(
            "Enemy命中後のスコア: "
            + playerScript.playerScore.Value
        );


        // ========================
        // Enemyをリスポーン
        // ========================

        if (isRespawning)
            return;

        StartCoroutine(
            RespawnEnemy()
        );
    }


    // =========================================================
    // Enemyリスポーン
    // =========================================================

    private IEnumerator RespawnEnemy()
    {
        isRespawning = true;


        // ========================
        // 見た目を消す
        // ========================

        if (enemyRenderers != null)
        {
            foreach (
                Renderer renderer
                in enemyRenderers)
            {
                renderer.enabled = false;
            }
        }


        // ========================
        // Colliderを無効化
        // ========================

        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }


        // ========================
        // 10秒待つ
        // ========================

        yield return new WaitForSeconds(3f);


        // ========================
        // 初期位置へ戻す
        // ========================

        transform.position =
            spawnPosition;


        // ========================
        // 見た目を戻す
        // ========================

        if (enemyRenderers != null)
        {
            foreach (
                Renderer renderer
                in enemyRenderers)
            {
                renderer.enabled = true;
            }
        }


        // ========================
        // Colliderを戻す
        // ========================

        if (enemyCollider != null)
        {
            enemyCollider.enabled = true;
        }


        isRespawning = false;
    }
}
