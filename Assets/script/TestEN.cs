using UnityEngine;

public class TestEN : MonoBehaviour
{
   
    public float speed = 5f;

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(x, z, 0);

        transform.position += move * speed * Time.deltaTime;
    }
}
