using UnityEngine;
using UnityEngine.AI;

public class SlowPickUp : MonoBehaviour
{
    [SerializeField] GameData gameData;

    [SerializeField] float slowMultiplier = 0.5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameData.enemySlowCollected)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            if (gameData.enemySlowCollected)
            {
                gameData.enemySlowCollected = true;

                gameData.playerSpeedMultiplier = slowMultiplier;

                EnemyController[] enemies = FindObjectsOfType<EnemyController>();

                foreach (EnemyController enemy in enemies)
                {
                    NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                    if (agent != null)
                    {
                        agent.speed = enemy.baseSpeed * gameData.enemySpeedMultiplier;
                        
                    }
                }
                Debug.Log("Enemy Slowed");
                
            }
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
