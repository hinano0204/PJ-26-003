using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WallManager : NetworkBehaviour
{
    [Header("ランダム表示する壁")]
    [SerializeField] private List<WallRandom> walls = new List<WallRandom>();

    [Header("シーン移動後、何秒待って開始するか")]
    [SerializeField] private float startDelay = 5f;

    [Header("何秒ごとに抽選するか")]
    [SerializeField] private float changeInterval = 4f;


    // 最大64個までの壁を管理できる
    // 1 = 表示
    // 0 = 非表示
    private NetworkVariable<ulong> wallMask =
        new NetworkVariable<ulong>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );


    private void Start()
    {
        // 現在の壁を全部非表示
        HideAllWalls();
    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // NetworkVariableが変更されたときに呼ばれる
        wallMask.OnValueChanged += OnWallMaskChanged;

        // 現在の状態を反映
        ApplyWallMask(wallMask.Value);

        // ホスト・サーバーだけがランダム処理を行う
        if (IsServer)
        {
            StartCoroutine(WallLoop());
        }
    }


    public override void OnNetworkDespawn()
    {
        wallMask.OnValueChanged -= OnWallMaskChanged;

        base.OnNetworkDespawn();
    }


    // =========================================
    // ホスト側のランダム処理
    // =========================================

    private IEnumerator WallLoop()
    {
        // シーン移動後5秒待つ
        yield return new WaitForSeconds(startDelay);


        while (true)
        {
            // ランダムで壁を決定
            ulong newMask = CreateRandomWallMask();

            // NetworkVariableに結果を入れる
            // → 全クライアントへ同期される
            wallMask.Value = newMask;

            // 4秒待つ
            yield return new WaitForSeconds(changeInterval);
        }
    }


    // =========================================
    // 各壁を個別にランダム判定
    // =========================================

    private ulong CreateRandomWallMask()
    {
        ulong mask = 0;

        // 最大64個まで
        int count = Mathf.Min(walls.Count, 64);

        for (int i = 0; i < count; i++)
        {
            WallRandom wall = walls[i];

            if (wall == null)
                continue;

            // 壁ごとに個別でランダム判定
            float randomValue = Random.value;

            if (randomValue < wall.showProbability)
            {
                // この壁を表示する
                mask |= (1UL << i);
            }
        }

        return mask;
    }


    // =========================================
    // NetworkVariableが変化したとき
    // =========================================

    private void OnWallMaskChanged(ulong oldMask, ulong newMask)
    {
        ApplyWallMask(newMask);
    }


    // =========================================
    // 実際に壁を表示・非表示
    // =========================================

    private void ApplyWallMask(ulong mask)
    {
        int count = Mathf.Min(walls.Count, 64);

        for (int i = 0; i < count; i++)
        {
            if (walls[i] == null)
                continue;

            // ビットを確認
            bool shouldShow = (mask & (1UL << i)) != 0;

            walls[i].SetWallVisible(shouldShow);
        }
    }


    // =========================================
    // 最初は全部非表示
    // =========================================

    private void HideAllWalls()
    {
        foreach (WallRandom wall in walls)
        {
            if (wall != null)
            {
                wall.SetWallVisible(false);
            }
        }
    }
}