using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class Title : MonoBehaviour
{
    public void StartHost()
    {
        //ホスト開始
        NetworkManager.Singleton.StartHost();
        //シーンを切り替え
        NetworkManager.Singleton.SceneManager.LoadScene("Lobe", LoadSceneMode.Single);
    }

    public void StartClient()
    {
        //ホストに接続
        NetworkManager.Singleton.StartClient();
    }
}
