using UnityEngine;

public class CandyBag : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("‰½‚©“ü‚Á‚½");
        TestPL2 player =
        other.GetComponentInParent<TestPL2>();

        if (player != null)
        {
            Debug.Log("Bag‚É“–‚½‚Á‚½");

            player.playerScore.Value +=
                player.carryingCandy.Value;

            player.carryingCandy.Value = 0;
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Bag"))
    //    {
    //        Debug.Log("Bag‚É“–‚½‚Á‚½");
    //    }
    //}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
