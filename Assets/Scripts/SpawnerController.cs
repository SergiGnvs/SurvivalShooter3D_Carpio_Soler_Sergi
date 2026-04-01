using UnityEngine;
using System.Collections;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] GameObject new_entity;
    [SerializeField] GameObject player;

    [SerializeField] float max_time;

    public bool canSpawn = true;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(timer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1);
    }

    IEnumerator timer()
    {
        while (canSpawn)
        {
            yield return new WaitForSeconds(max_time);
            GameObject new_object = Instantiate(new_entity, transform.position, Quaternion.identity);
            new_object.GetComponent<EnemyController>().target = player;
        }
    }

}
