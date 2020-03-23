using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    private float lookSensitivity = 5f;

    private PlayerMotor playerMotor;

    void Start() {
        playerMotor = GetComponent<PlayerMotor>();
    }

    void Update(){
        // Calculate movement velocity as 3D vector
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        // Final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

        // Apply movement
        playerMotor.Move(_velocity);

        // Calculate rotation as 3D vector
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        // Apply rotation
        playerMotor.Rotate(_rotation);

        // Calculate camera rotation as 3D vector
        float _xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 _camerRotation = new Vector3(_xRot, 0f, 0f) * lookSensitivity;

        // Apply rotation
        playerMotor.RotateCamera(_camerRotation);
    }
}
