using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    InputAction moveAction;
    CharacterController controller;

    void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (moveAction == null) return;

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 dir = new Vector3(input.x, 0f, input.y);

        controller.Move(dir * (speed * Time.deltaTime));
    }
}