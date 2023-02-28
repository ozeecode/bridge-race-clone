using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;

    private JoystickTestInput testInput;
    private Vector3 _input;
    private void Awake()
    {
        testInput = new JoystickTestInput();
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        testInput.Enable();
        testInput.JoystickTest.Movement.performed += Movement_performed;
        testInput.JoystickTest.Movement.canceled += Movement_canceled;

    }

    
    private void Movement_performed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Movement!");
        _input = ctx.ReadValue<Vector2>();
    }
    private void Movement_canceled(InputAction.CallbackContext ctx)
    {
        _input = Vector3.zero;
    }



    public void FixedUpdate()
    {
        Vector3 direction = Vector3.right * _input.x + Vector3.forward * _input.y ;
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}
