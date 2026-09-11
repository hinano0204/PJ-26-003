using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Director : MonoBehaviour
{
    public GameObject toggle;     // ホストのチェックボックス
    public GameObject inputField; // IPアドレスの入力欄

    void Start()
    {
        string ipAddress = "";
        // ビルド版は物理IPアドレスを自動取得
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                ipAddress = ip.ToString();
                break;
            }
        }
        inputField.GetComponent<InputField>().text = ipAddress;
    }

    // ログインボタンがクリックされたときの処理
    public void OnClick()
    {
        // InputFieldのテキストからIPアドレスを取得
        string ipAddress = inputField.GetComponent<InputField>().text;

        // UTPトランスポートにIPアドレスを設定
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData(ipAddress, 7777);

        // トグルのチェック状態を確認してホストかクライアントかを決定
        bool isHost = toggle.GetComponent<Toggle>().isOn;

        if (isHost)
        {
            // ホストとして起動
            NetworkManager.Singleton.StartHost();
            // Gameシーンを読み込む
            NetworkManager.Singleton.SceneManager.LoadScene("main", LoadSceneMode.Single);
        }
        else
        {
            // クライアントとして起動
            NetworkManager.Singleton.StartClient();
        }
    }
}