
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.Netcode;

[RequireComponent(typeof(NavMeshAgent))]
// 敵の状態スクリプト
public class EnemyStatus : MobStatus
{
    private NavMeshAgent _agent;

    // MobStatusからvirtualなメソッドを書き換えることができる
    protected override void Start()
    {
        // MobStatusのStartメソッドを実行
        base.Start();
        // ナビメッシュエージェントを取得
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update()
    {
        // NavMeshAgentのvelocityから移動速度のベクトルを取得して
        // 移動アニメーションを再生する
        //_animator.SetFloat("MoveSpeed", _agent.velocity.magnitude);
    }

    // キャラクターが倒れた時の処理
    protected override void OnDie()
    {
        // MobStatusのOnDieメソッドを実行
        base.OnDie();
        // 消滅コルーチンの再生
        StartCoroutine(DestoroyCoroutine());
    }

    // 倒されたときの消滅コルーチン
    private IEnumerator DestoroyCoroutine()
    {
        // 3秒待つ
        yield return new WaitForSeconds(3f);
        //yield return null;
        // ゲームオブジェクトを消去する
        Destroy(gameObject);
    }

}
