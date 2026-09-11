using UnityEngine;

public class LoopBackground : MonoBehaviour
{
    public float speed = 100f;

    void Update()
    {
        RectTransform rect = GetComponent<RectTransform>();

        // ‰E‚ÖˆÚ“®
        rect.anchoredPosition += Vector2.right * speed * Time.deltaTime;

        // X‚ª1980‚ð’´‚¦‚½‚ç¶‚Ö–ß‚·
        if (rect.anchoredPosition.x >= 3700)
        {
            rect.anchoredPosition = new Vector2(-3700, rect.anchoredPosition.y);
        }
    }
}