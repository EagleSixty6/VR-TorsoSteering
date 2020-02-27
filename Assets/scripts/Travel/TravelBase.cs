using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class TravelBase : MonoBehaviour
{
    protected string _methodName;
    protected float _yRotation;
    protected Vector2 _input;
    protected CharacterController _characterController;
    protected float _stepCycle;
    protected float _nextStep;
    protected AudioSource _audioSource;

    protected float m_WalkSpeed = 3f;

    private bool _initialized = false;

    // Rot
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;

    protected virtual void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _characterController.detectCollisions = false;
        _stepCycle = 0f;
        _nextStep = _stepCycle / 2f;
        _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if(_initialized)
        {
            // manually update the character controllers player position to the camera and not the player platform, it manupulates
            gameObject.GetComponent<CharacterController>().center = new Vector3(Camera.main.transform.localPosition.x, 0, Camera.main.transform.localPosition.z);

            Vector3 desiredMove = GetDisiredMovementDirection();
            GetInput();
            Vector3 moveDir = desiredMove * m_WalkSpeed;
            _characterController.Move(moveDir * Time.fixedDeltaTime);

            // TODO frmae rate independent
            RotateView();
        }        
    }

    protected virtual void RotateView(){}

    public virtual void Init()
    {
        _initialized = true;
    }

    public virtual void CleanUp(){}

    protected virtual Vector3 GetDisiredMovementDirection()
    {
        return Vector3.zero;
    }

    protected virtual void GetInput()
    {
        // Read input
       
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        _input = new Vector2(horizontal, vertical);
        // normalize input if it exceeds 1 in combined length:
        if (_input.sqrMagnitude > 1)
        {
            _input.Normalize();
        }
    }


    public bool IsMovingForward()
    {
        return _input.y > 0;
    }

    public bool IsMovingBackward()
    {
        return _input.y < 0;
    }
    public string GetName()
    {
        return _methodName;
    }
}

