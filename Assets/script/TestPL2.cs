using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class TestPL2 : NetworkBehaviour
{
    public float m_moveSpeed = 5;

    private Rigidbody m_rigidBody;
    private Vector2 m_moveInput = Vector2.zero;

    void Start()
    {
        // Rigidbody を取得
        m_rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //自分のオブジェクトだけ入力を設定
        if (IsOwner)
        {
            // 移動入力を設定
            SetMoveInputServerRpc(
                    Input.GetAxisRaw("Horizontal"),
                    Input.GetAxisRaw("Vertical"));
        }

        //ホストの場合
        if (IsHost)
        {
            HostUpdate();
        }
    }

    //=================================================================
    //RPC 
    //=================================================================
    // すべての端末で同時に実行される処理
    [ServerRpc]
    private void SetMoveInputServerRpc(float x, float y)
    {
        m_moveInput = new Vector2(x, y);  //入力情報の記録
    }

    //=================================================================
    //ホスト側だけで行う処理
    //=================================================================
    // ホストだけで呼び出すUpdate
    private void HostUpdate()
    {
        //移動
        var velocity = Vector3.zero;
        velocity.x = m_moveSpeed * m_moveInput.normalized.x;
        velocity.y = m_moveSpeed * m_moveInput.normalized.y;
        //移動処理
        m_rigidBody.AddForce(velocity);
    }


    // 同期させたい変数（ホストが書き込み、全員が読み取る設定）
    public NetworkVariable<int> playerScore = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server // 注: NGOの仕様上ここはServerですが、概念としてホストが実行します
    );

    // 当たり処理
    private void OnTriggerEnter(Collider other)
    {
        // サーバー（ホスト）だけで衝突処理を実行する
        if (!IsServer) return;

        // 当たった相手のタグが「Target」だった場合
        if (other.CompareTag("candy"))
        {
            //スコアを加算する。
            playerScore.Value += 1;
            Debug.Log("得点:" + playerScore.Value);

            //ネットワークオブジェクトとして消去（全員の画面から消える）
            NetworkObject targetNetObj = other.GetComponent<NetworkObject>();
            targetNetObj.Despawn();
        }
    }
}