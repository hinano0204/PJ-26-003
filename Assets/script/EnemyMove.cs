using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
//[RequireComponent(typeof(NavMeshAgent))]

public class EnemyMove : NetworkBehaviour
{

    //[SerializeField] private PlayerController _playerController;
    public Transform player;
    public float speed = 5f;
    public GameObject Enemy;
    public float avoidDistance = 2f;
    public float rayDistance = 1f;

    //敵が最初にいた場所
    private Vector3 spawnPosition;

    //リスポーン中かどうか
    private bool isRespawning = false;

    private void Start()
    {
        //最初のエネミーの位置だよ
        spawnPosition = transform.position;

        Invoke(nameof(Update), 5f);
        Enemy.SetActive(false);
    }
    void Update()
    {
        
        Enemy.SetActive(true);
        //リスポーン中なら移動しない
        if (isRespawning)
            return;

        //サーバが敵を動かす
        if (!IsServer)  //サーバじゃなかったら何もしない
            return;

        //一番近いプレイヤーを探す
        FindNearestPlayer();

        //プレイヤーがいなければなんもしないよ
        if (player == null)
            return;

        Vector3 pos = transform.position;
        Vector3 targetpos = player.position;
        Vector3 dir = (targetpos - pos).normalized;

        RaycastHit hit;

        

        if (Physics.Raycast(transform.position,dir,out hit,rayDistance))   
        {
            // 障害物発見
            //上いける？
            bool canGoUp = !Physics.Raycast
                (
                transform.position,
                Vector3.up,
                avoidDistance
                );

            //下いける？
            bool canGoDoun = !Physics.Raycast
                (
                transform.position,
                Vector3.down,
                avoidDistance
                );
             
            if(canGoUp)
            {
                targetpos = transform.position + Vector3.up * avoidDistance;
            }

            else if(canGoDoun)
            {
                targetpos = transform.position + Vector3.down * avoidDistance;
            }
        }

        transform.position =
            Vector3.MoveTowards
            (
                transform.position,
                targetpos,
                speed * Time.deltaTime
            );
    }

    //------------------------
    //一番近いプレイヤーを探す
    //------------------------
    private void FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float nearestDistance = Mathf.Infinity;
        Transform nearestPlayer = null;
        foreach( GameObject p in players )
        {
            float distance = Vector3.Distance
                (
                transform.position, p.transform.position
                );
            if(distance < nearestDistance )
            {
                nearestDistance = distance;
                nearestPlayer = p.transform;
            }
        }
        player = nearestPlayer;
    }

    //--------------------
    //プレイヤーにぶつかった
    //--------------------

    private void OnCollisionEnter(Collision collision)
    {
        //サーバだけ
        if (!IsServer)
            return;

        //Playerタグじゃないなら無視
        if (!collision.gameObject.CompareTag("Player"))
            return;
        //すでにリスポーン中ならむし
        if (isRespawning)
            return;

        StartCoroutine(RespawnEnemy());
    }

    //-------------------
    //敵をリスポーン
    //-------------------

    private IEnumerator RespawnEnemy()
    {
        isRespawning = true;

        //けす
        Enemy.SetActive(false);

        //１０秒待つ
        yield return new WaitForSeconds(10f);

        //最初の場所に戻す
        transform.position = spawnPosition;

        //敵出現
        Enemy.SetActive(true);
        isRespawning = false;
    }
}