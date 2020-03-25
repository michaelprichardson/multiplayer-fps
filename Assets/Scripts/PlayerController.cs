using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 5f;
    [SerializeField]
    private float thrusterForce = 1000f;

    [Header("Spring Settings:")]
    [SerializeField]
    private float jointSpring = 20;
    [SerializeField]
    private float jointMaxForce = 40;

    // Component caching
    private PlayerMotor playerMotor;
    private ConfigurableJoint joint;
    private Animator animator;

    void Start() {
        playerMotor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();

        SetJointSettings(jointSpring);
    }

    void Update(){
        // Calculate movement velocity as 3D vector
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        // Final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

        // Animate movement
        animator.SetFloat("ForwardVelocity", _zMov);

        // Apply movement
        playerMotor.Move(_velocity);

        // Calculate rotation as 3D vector
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        // Apply rotation
        playerMotor.Rotate(_rotation);

        // Calculate camera rotation as 3D vector
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _camerRotationX = _xRot * lookSensitivity;

        // Apply rotation
        playerMotor.RotateCamera(_camerRotationX);

        Vector3 _thrusterForce = Vector3.zero;

        // Apply thruster force
        if(Input.GetButton("Jump")){
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        }
        else{
            SetJointSettings(jointSpring);
        }

        playerMotor.ApplyThruster(_thrusterForce);
    }

    private void SetJointSettings(float _jointSpring){
        joint.yDrive = new JointDrive {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }
}
