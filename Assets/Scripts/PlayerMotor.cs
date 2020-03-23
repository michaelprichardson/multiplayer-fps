﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    [SerializeField]
    private Camera cam;
    
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;
    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // Gets a movement vector
    public void Move(Vector3 _velocity) {
        velocity = _velocity;
    }

    // Gets a rotation vector
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    // Gets a rotation vector for the camera
    public void RotateCamera(Vector3 _rotation)
    {
        cameraRotation = _rotation;
    }

    // Run every physics update
    void FixedUpdate() {
        // Perform movement
        PerformMovement();
        PerformRotation();
    }

    void PerformMovement(){
        if (velocity != Vector3.zero){
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    void PerformRotation(){
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if(cam != null){
            cam.transform.Rotate(-cameraRotation);
        }
    }

}
