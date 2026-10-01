using Unity.Netcode;
using UnityEngine;

public class PlayerColor : NetworkBehaviour
{
    // Inspectorからマテリアルを設定する
    [SerializeField]
    private Material[] m_playerMaterials = new Material[4];

    public override void OnNetworkSpawn()
    {
        var renderer = GetComponent<MeshRenderer>();

        if (renderer != null)
        {
            // プレイヤーごとにマテリアルを割り当てる
            int materialIndex = (int)(OwnerClientId % (ulong)m_playerMaterials.Length);

            // マテリアルを適用
            if (m_playerMaterials[materialIndex] != null)
            {
                renderer.material = m_playerMaterials[materialIndex];
            }
        }
    }

}