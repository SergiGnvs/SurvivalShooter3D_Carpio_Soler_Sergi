using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{

    enum TargetType
    {
        None, Floor, Enemy
    }

    struct Target
    {

        public Target(TargetType type, RaycastHit hit)
        {
            Type = type;
            Hit = hit;
        }

        public TargetType Type;
        public RaycastHit Hit;

        
    }

    NavMeshAgent m_agent;
    DiabloInput m_input;

    InputAction m_moveAction;
    InputAction[] m_switchWeaponActions = new InputAction[3];


    [SerializeField] GameObject[] Weapons = new GameObject[3];
    IWeapon currentWeapon;
    int currentWeaponNumber;

    Vector3 mousePos = new Vector3();

    Target m_target = new Target(TargetType.None, new RaycastHit());

    [SerializeField] LayerMask Mask; //string[] Layers;

    [SerializeField] float attack_range = 10f;
    [SerializeField] float cooldown = 1f;
    bool can_shoot = true;

    void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();

        m_input = new DiabloInput();

        m_input.Main.Enable();

        m_moveAction = m_input.Main.Move;

        m_switchWeaponActions = new InputAction[3] { m_input.Main.Weapon1, m_input.Main.Weapon2, m_input.Main.Weapon3 };
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        for (int i = 0; i < Weapons.Length; i++) 
        {
            if (i == 0)
            {
                currentWeapon = Weapons[i].GetComponent<IWeapon>();
                currentWeaponNumber = i;
            }
            else
            {
                Weapons[i].SetActive(false);
            }
        }

        currentWeapon = Weapons[0].GetComponent<IWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Mouse.current.position.value;

        if (m_moveAction.WasPressedThisFrame())
        {

            MoveTo();
        }

        switch (m_target.Type)
        {
            case TargetType.Enemy:
                m_agent.destination = m_target.Hit.transform.position;
                if (Vector3.Distance(transform.position, m_agent.destination) <= currentWeapon.GetRange())
                {
                    m_agent.isStopped = true;
                    transform.LookAt(m_agent.destination);
                    currentWeapon.Shoot(m_target.Hit.transform.GetComponent<EnemyController>());
                }
                break;
            case TargetType.Floor:
            case TargetType.None:
            default:
                break;
        }

        for(int i = 0;i < m_switchWeaponActions.Length; i++)
        {
            if (m_switchWeaponActions[i].WasPressedThisFrame())
            {
                if (Weapons[i] & currentWeapon != Weapons[i].GetComponent<IWeapon>())
                {

                    Weapons[currentWeaponNumber].SetActive(true);
                    currentWeapon = Weapons[i].GetComponent<IWeapon>();
                    currentWeapon.SwitchWeapon();
                    currentWeaponNumber = i;
                    break;
                }
            }
        }

    }

    void MoveTo()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100, LayerMask.GetMask("Floor" )))
        {
            string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);

            switch (layerName)
            {
                case "Enemy":
                    m_target = new Target(TargetType.Enemy, hit);
                    break;
                case "Floor":
                    m_target = new Target(TargetType.Floor, hit);
                    break;
                case "Interactable":
                default:
                    break;
            }

            m_agent.isStopped = false;
            m_agent.destination = m_target.Hit.point;
            /*
            switch (layerName)
            {
                case LayerMask.GetMask("Floor"):
                    m_target = new Target();
                    m_target.type = TargetType.Floor;
                    m_target.body = null;
                    m_target.pos = hit.point;
                    break;

                case 8:
                    m_target = new Target();
                    m_target.type = TargetType.Floor;
                    m_target.body = hit.collider.gameObject;
                    m_target.pos = hit.point;
                    break;
                default:
                    break;


            }*/

            m_agent.destination = hit.point;
        }
    }
     
    /*void Shoot()
    {
        Debug.Log("BANG!!");
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.Normalize(m_target.Hit.transform.position - transform.position), out hit, attack_range);
        Debug.DrawLine(transform.position, hit.point, Color.yellow, 0.1f);
        StartCoroutine(ShootCooldown());    
    }*/

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        can_shoot = true;
    }

    private void OnDrawGizmos()
    {

        if(m_target.Type != TargetType.None)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(m_agent.destination, 0.5f);
        }
        Gizmos.color = Color.red;
        if(currentWeapon != null)
        {
            Gizmos.DrawWireSphere(transform.position, currentWeapon.GetRange());
        }
        

    }
}
