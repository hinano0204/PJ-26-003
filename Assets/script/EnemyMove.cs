using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
//[RequireComponent(typeof(NavMeshAgent))]

public class EnemyMove : NetworkBehaviour
{

    //[SerializeField] private PlayerController _playerController;
    public Transform player;
    public float speed = 5f;
    public GameObject Enemy;
    public float avoidDistance = 2f;
    public float rayDistance = 1f;
    //private int count;

    private void Start()
    {
      // Enemy.SetActive(false);
        Invoke(nameof(Update), 5f);
        Enemy.SetActive(false);
    }
    void Update()
    {
        
        Enemy.SetActive(true);
        Vector3 pos = transform.position;
        Vector3 targetpos = player.position;
        Vector3 dir = (targetpos - pos).normalized;
        if (targetpos.x > pos.x)
        {
            pos.x += speed * Time.deltaTime;
        }
        else
        {
            pos.x -= speed * Time.deltaTime;
        }

        transform.position = pos;

        RaycastHit hit;
        

        if (Physics.Raycast(transform.position,dir,out hit,rayDistance))   
        {
            // è·äQï®î≠å©
            //è„Ç¢ÇØÇÈÅH
            bool canGoUp = !Physics.Raycast
                (
                transform.position,
                Vector3.up,
                avoidDistance
                );

            //â∫Ç¢ÇØÇÈÅH
            bool canGoDoun = !Physics.Raycast
                (
                transform.position,
                Vector3.down,
                avoidDistance
                );
             
            if(canGoUp)
            {
                targetpos = transform.position + Vector3.up * avoidDistance;
            }

            else if(canGoDoun)
            {
                targetpos = transform.position + Vector3.down * avoidDistance;
            }
        }

        transform.position =
            Vector3.MoveTowards
            (
                transform.position,
                targetpos,
                speed * Time.deltaTime
            );
    }

}