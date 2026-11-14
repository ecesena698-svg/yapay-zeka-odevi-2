using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshObstacle))]
public class SlidingDoor : MonoBehaviour
{
    public float closedY = 1f;
    public float openY = -1.1f;
    public float speed = 2f;
    public bool startOpen = true;

    NavMeshObstacle obstacle;
    InputAction toggle;
    bool isOpen;

    void Awake()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        toggle = InputSystem.actions.FindAction("ToggleDoor");
        isOpen = startOpen;
        SetY(isOpen ? openY : closedY);
        obstacle.enabled = !isOpen;
    }

    void Update()
    {
        if (toggle != null && toggle.WasPressedThisFrame())
        {
            isOpen = !isOpen;
            obstacle.enabled = !isOpen;
        }
        float targetY = isOpen ? openY : closedY;
        SetY(Mathf.MoveTowards(transform.position.y, targetY, speed * Time.deltaTime));
    }

    void SetY(float y)
    {
        Vector3 p = transform.position;
        p.y = y;
        transform.position = p;
    }
}