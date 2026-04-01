using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{

    enum TargetType
    {
        Floor, Enemy
    }

    struct Target
    {
        public TargetType type;
        public GameObject body;
        public Vector3 pos;
    }

    NavMeshAgent m_agent;
    DiabloInput m_input;

    InputAction m_moveAction;

    Vector3 mousePos = new Vector3();

    [SerializeField] LayerMask Mask; //string[] Layers;

    void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();

        m_input = new DiabloInput();

        m_input.Main.Enable();

        m_moveAction = m_input.Main.Move;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Mouse.current.position.value;

        if (m_moveAction.WasPressedThisFrame())
        {
            MoveTo();
        }
        /*
        else
        {
            switch (m_target.type)
            {
                
                case TargetType.Enemy:
                    m_agent.destination = m_target.body;
                        brake;

                case TargetType.Floor:
                    break;

                default:
                    break;
            }
        }
        */

    }

    void MoveTo()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100, LayerMask.GetMask("Floor" )))
        {
            string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
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
     
}
