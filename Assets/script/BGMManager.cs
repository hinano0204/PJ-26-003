using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // シーンが変わっても、このオブジェクトを残す
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // BGMが二重に存在しないようにする
            Destroy(gameObject);
        }
    }
}
