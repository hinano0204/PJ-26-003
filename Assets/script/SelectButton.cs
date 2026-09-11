using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SelectBotton : MonoBehaviour


{

#if UNITY_EDITOR
    [SerializeField] private SceneAsset sceneAsset;
#endif

    private bool isLoading = false;

    public void LoadScene()
    {
        if (isLoading) return;

        isLoading = true;

        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.interactable = false;
        }

        Debug.Log("ボタン押された");
#if UNITY_EDITOR
        if (sceneAsset != null)
        {
            string sceneName = sceneAsset.name;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("シーンが設定されていません");
        }
#endif
    }
}
