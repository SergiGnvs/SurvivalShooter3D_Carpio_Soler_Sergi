using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    [SerializeField] EnemyData data;

    float maxHealth;
    float curHealth;

    [SerializeField] public GameObject target;

    [SerializeField] GameData gameData;

    public float baseSpeed = 3.5f;

    NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = data.GetMaxHealth();
        curHealth = maxHealth;


        agent = GetComponent<NavMeshAgent>();

        Debug.Log(maxHealth);

        agent.speed = baseSpeed * gameData.enemySpeedMultiplier;
    }

    // Update is called once per frame
    void Update()
    {

        if (target) agent.SetDestination(target.transform.position);
    }

    public void GetDamaged(float amount)
    {
        curHealth -= amount;

        if(curHealth <= 0)
        {
            Destroy(this.gameObject);
        }

    }
}
