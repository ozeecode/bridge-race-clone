using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    public float rotationSpeed;
    private Vector3 _movement, _forward, _right, _animatorDelta;
    private Transform _camTransform;
    private PlayerControls _playerControls;
    private Vector2 _input;
    private float _movementMagnitude;
    private Quaternion _qTo;




    protected override void Awake()
    {
        base.Awake();
        _camTransform = Camera.main.transform;
        _forward = Vector3.forward;
        _right = Vector3.right;

        _playerControls = new PlayerControls();
        _playerControls.Player.Movement.performed += Movement_performed;
        _playerControls.Player.Movement.canceled += Movement_canceled;

        agent.updateRotation = false;


    }

    public override void Init(Color color)
    {
        base.Init(color);
        _playerControls.Enable();
        _qTo = Quaternion.identity;
        transform.forward = Vector3.forward;
        transform.position = Vector3.zero;
    }

    public override void Stop()
    {
        base.Stop();
        _playerControls.Disable();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }
    private void OnDisable()
    {
        _playerControls.Disable();
    }


    private void Update()
    {
        if (!IsInit) return;
        Vector3 scaledMovement = Vector3.zero;

        if (_movementMagnitude > 0)
        {
            UpdateInputDirection();
            LookDirection(_movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, _qTo, rotationSpeed);
            if (CheckSteps())
            {
                scaledMovement = agent.speed * Time.deltaTime * _movement;
                agent.Move(scaledMovement);
            }

        }

    }



    private void Movement_canceled(InputAction.CallbackContext ctx)
    {
        _input = Vector2.zero;
        _movementMagnitude = 0;
        animator.SetBool(GameStatic.RUN, false);
    }

    private void Movement_performed(InputAction.CallbackContext ctx)
    {
        _input = ctx.ReadValue<Vector2>();
        _movementMagnitude = _input.magnitude;
        if (_movementMagnitude > .1f)
        {
            animator.SetBool(GameStatic.RUN, true);
        }
        else
        {
            animator.SetBool(GameStatic.RUN, false);
            _movementMagnitude = 0;
        }
    }

    public void LookDirection(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        _qTo = Quaternion.Euler(new Vector3(0, angle, 0));
    }

    private void UpdateInputDirection()
    {
        Vector3 verticalInput = _forward * _input.y;
        Vector3 horizontalInput = _right * _input.x;

        _forward = _camTransform.forward;
        _forward.y = 0;
        _forward = _forward.normalized;
        _right = Quaternion.Euler(new Vector3(0, 90, 0)) * _forward;

        _movement = (verticalInput + horizontalInput).normalized;
    }


}
