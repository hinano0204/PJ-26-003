using UnityEngine;

public class StartoBake1 : MonoBehaviour
{

    public float speed = 0;
    bool muki = false;

    void Update()
    {
        RectTransform rect = GetComponent<RectTransform>();
        

        rect.anchoredPosition += Vector2.up * speed * Time.deltaTime;

        // XÇ™1980Çí¥Ç¶ÇΩÇÁç∂Ç÷ñﬂÇ∑
        if (rect.anchoredPosition.y<= 1.5)
        {
            muki = true;

        }

        if (rect.anchoredPosition.y>= 3)
        {
            muki = false;
        }


        if (muki ==false)
        {
            speed = -1;
        
        }
        else
        {
            speed = 1;
        }


    }
}
    
