using Unity.Netcode;
using UnityEngine;

public class PlayerColor : NetworkBehaviour
{
    // 割り当てる4色を配列で準備（インデックス 0=赤, 1=青, 2=緑, 3=黄）
    private Color[] m_playerColors = new Color[]
    {
        Color.white,
        Color.black,
        Color.green,
        Color.yellow
    };

    // Startの代わりに、ネットワークが繋がった時に呼ばれるOnNetworkSpawnを使います
    public override void OnNetworkSpawn()
    {
        var renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            // OwnerClientId は ulong 型（非常に大きな整数）なので int 型に変換
            int colorIndex = ((int)OwnerClientId % m_playerColors.Length);

            // 配列から対応する色を取り出してマテリアルに設定
            renderer.material.color = m_playerColors[colorIndex];
        }
    }
}