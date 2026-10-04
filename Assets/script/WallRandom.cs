
using UnityEngine;

public class WallRandom : MonoBehaviour
{
    [Header("‚±‚Ì•Ç‚ª•\Ž¦‚³‚ê‚éŠm—¦")]
    [Range(0f, 1f)]
    public float showProbability = 0.5f;

    // WallManager‚©‚çŒÄ‚Î‚ê‚é
    public void SetWallVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}