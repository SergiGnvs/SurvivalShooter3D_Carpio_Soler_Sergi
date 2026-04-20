using UnityEngine;
using System.Collections;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] GameObject new_entity;
    [SerializeField] GameObject player;

    [SerializeField] float max_time;
    [SerializeField] float spawn_radius = 1;

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
        Gizmos.DrawWireSphere(transform.position, spawn_radius);
    }

    IEnumerator timer()
    {
        while (canSpawn)
        {
            yield return new WaitForSeconds(max_time);

            float radius = Random.Range(0, spawn_radius);
            float angle = Random.Range(0, 360);


            Vector3 newPosition = transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;

            GameObject newObject = Instantiate(new_entity, newPosition, Quaternion.identity);
            //newObject.GetComponent<Collider>().enabled = false;EnemyController>();

                /*
            Vector3 pos = new Vector3(1, 1, 0) * radius;
            Vector3 finalPosition = Vector3.Normalize(angle * pos) * radius;
            

            GameObject new_object = Instantiate(new_entity, transform.position + finalPosition, Quaternion.identity);
            new_object.GetComponent<EnemyController>().target = player; */
        }
    }

}
