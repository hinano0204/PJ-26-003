using UnityEngine;

public class StartoBake : MonoBehaviour
{

    public float speed = 0;
    bool muki = false;

    void Update()
    {
        RectTransform rect = GetComponent<RectTransform>();
        

        rect.anchoredPosition += Vector2.up * speed * Time.deltaTime;

        // XÇ™1980Çí¥Ç¶ÇΩÇÁç∂Ç÷ñﬂÇ∑
        if (rect.anchoredPosition.y<= -1)
        {
            muki = false;

        }

        if (rect.anchoredPosition.y>= 2)
        {
            muki = true;
        }


        if (muki ==true)
        {
            speed = -1;
        
        }
        else
        {
            speed = 1;
        }


    }
}
    
