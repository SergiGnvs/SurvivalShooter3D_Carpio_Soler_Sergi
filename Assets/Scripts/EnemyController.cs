using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    [SerializeField] EnemyData data;

    float maxHealth;
    float curHealth;

    [SerializeField] public GameObject target;

    NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = data.GetMaxHealth();
        curHealth = maxHealth;


        agent = GetComponent<NavMeshAgent>();

        Debug.Log(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {

        if (target) agent.SetDestination(target.transform.position);
    }
}
