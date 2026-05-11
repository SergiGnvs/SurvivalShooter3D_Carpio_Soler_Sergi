using UnityEngine;
using UnityEngine.AI;

public class SpeedPickUp : MonoBehaviour
{
    [SerializeField] GameData gameData;

    [SerializeField] float speedMultiplier = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (gameData.playerSpeedCollected)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();

        if(player != null)
        {
            if (gameData.playerSpeedCollected)
            {
                gameData.playerSpeedCollected = true;

                gameData.playerSpeedMultiplier = speedMultiplier;

                NavMeshAgent agent = player.GetComponent<NavMeshAgent>();

                if(agent != null)
                {
                    agent.speed = player.baseSpeed * gameData.playerSpeedMultiplier;
                    Debug.Log("SPEED UP!");
                }
                Destroy(gameObject);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
