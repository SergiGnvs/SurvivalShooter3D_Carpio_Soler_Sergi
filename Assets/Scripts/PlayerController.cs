using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{

    public enum TargetType
    {
        none, Enemy, position
    }

    public struct Target
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
    Target m_target = new Target(TargetType.none, new RaycastHit());
    [SerializeField] LayerMask mask; //string[] Layers;

    InputAction m_moveAction;
    InputAction m_interaction;
    InputAction[] m_switchWeaponActions = new InputAction[3];
    InputAction m_saveAction;
    InputAction m_resetAction;


    [SerializeField] GameObject[] Weapons = new GameObject[3];
    IWeapon CurrentWeaponController;
    [SerializeField] GameObject CurrentWeapon;

    Vector3 MousePosition = new Vector3();

    [SerializeField] GameData gameData;

    public float baseSpeed = 5f;

    void Awake()
    {

        m_agent = GetComponent<NavMeshAgent>();

        m_input = new DiabloInput();

        m_input.Main.Enable();

        m_moveAction = m_input.Main.Move;

        m_switchWeaponActions = new InputAction[3] { m_input.Main.Weapon1, m_input.Main.Weapon2, m_input.Main.Weapon3 };

        m_interaction = m_input.Main.Move;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        for (int i = 0; i < Weapons.Length; i++)
        {
            if (i == 0)
            {
                CurrentWeapon = Weapons[i];
                CurrentWeaponController = CurrentWeapon.GetComponent<IWeapon>();
            }
            else Weapons[i].gameObject.SetActive(false);
        }

        Debug.Log("Start with " + CurrentWeapon.name);

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckTarget();
        SwitchWeapon();
    }

    void Move()
    {
        MousePosition = Mouse.current.position.value;

        if (!m_interaction.WasPressedThisFrame()) { return; }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(MousePosition), out hit, 100, mask))
        {

            string LayerName = LayerMask.LayerToName(hit.transform.gameObject.layer);

            switch (LayerName)
            {
                case "Enemy":
                    m_target = new Target(TargetType.Enemy, hit);
                    break;

                case "Floor":
                    m_target = new Target(TargetType.position, hit);
                    break;

                case "Interactable":
                    break;

                default:
                    Debug.Log("???");
                    break;
            }

            m_agent.destination = m_target.Hit.point;
            m_agent.isStopped = false;


        }
    }

    void CheckTarget()
    {
        switch (m_target.Type)
        {
            case TargetType.Enemy:
                

                if (m_target.Hit.transform == null)
                {
                    m_target = new Target(TargetType.none, new RaycastHit());
                    return;
                }

                m_agent.destination = m_target.Hit.transform.position;

                float distanceToEnemy = Vector3.Distance(transform.position, m_target.Hit.transform.position);
                

                if (distanceToEnemy <= CurrentWeaponController.GetRange())
                {
                    m_agent.isStopped = true;
                    transform.LookAt(m_target.Hit.transform.position);
                    CurrentWeaponController.Shoot(m_target.Hit.transform.GetComponent<EnemyController>());
                }
                break;
            case TargetType.position:
            default:
                break;
        }
    }


        void OnDrawGizmos()
        {

            if (m_target.Type != TargetType.none)
            {
                Gizmos.color = new Color(1f, 1f, 0f, 1f);
                Gizmos.DrawWireSphere(m_agent.destination, 0.25f);
            }
            Gizmos.color = Color.red;


        }

        void SwitchWeapon()
        {
            // Recorremos la lista de inputs para ver si se ha pulsado uno de los botones
            for (int i = 0; i < m_switchWeaponActions.Length; i++)
            {
                // Si hay más botones que armas terminaremos la ejecución para evitar problemas
                if (i >= Weapons.Length) return;

                // Comprobamos si se ha pulsado el boton
                if (m_switchWeaponActions[i].WasPressedThisFrame())
                {
                 if (Weapons[i] != null && Weapons[i] != CurrentWeapon)
                 {
                    // Desactivamos el arma actual
                    CurrentWeapon.gameObject.SetActive(false);

                    // Nuestra arma actual pasa a ser el arma i
                    CurrentWeapon = Weapons[i];

                    // Actualizamos el controlador del arma actual
                    CurrentWeaponController = CurrentWeapon.GetComponent<IWeapon>();

                    // Activamos el arma actual para que se vea
                    CurrentWeapon.gameObject.SetActive(true);

                    // Llamamos a la función cambio de arma del controlador
                    CurrentWeaponController.SwitchWeapon();
                 }
                return;
            }
            }

        }


    
}

