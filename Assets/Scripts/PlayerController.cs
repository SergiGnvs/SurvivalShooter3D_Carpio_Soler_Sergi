using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    NavMeshAgent m_agent;
    DiabloInput m_input;

    InputAction m_moveAction;

    Vector3 mousePos = new Vector3();

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

    }

    void MoveTo()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100))
        {
            m_agent.destination = hit.point;
        }
    }
     
}
