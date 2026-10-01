using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectBotton : MonoBehaviour
{
    [SerializeField] private string sceneName;

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

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("シーン名が設定されていません");
        }
    }
}