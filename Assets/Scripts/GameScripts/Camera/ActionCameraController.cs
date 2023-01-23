using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionCameraController : MonoBehaviour
{
    public bool IsActiveCamera;
    // rotation
    [SerializeField]
    private float maxRotationSpeed = 1f;

    private CameraControlActions cameraActions;

    public Camera ActionCamera;

    public GameObject Target;

    private void Awake()
    {
        cameraActions = new CameraControlActions();
    }

    private void OnEnable()
    {
        Enable();
    }

    public void Enable()
    {
        IsActiveCamera = true;
        cameraActions.Enable();
        cameraActions.CameraRotation.Rotate.performed += RotateCamera;
    }

    private void OnDisable()
    {
        Disable();
    }

    public void Disable()
    {
        IsActiveCamera = false;
        cameraActions.Disable();
        cameraActions.CameraRotation.Rotate.performed -= RotateCamera;
    }

    private void RotateCamera(InputAction.CallbackContext obj)
    {
        if (!Mouse.current.middleButton.isPressed)
            return;

        float inputValue = obj.ReadValue<Vector2>().x;
        transform.rotation = Quaternion.Euler(0f, inputValue * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
    }

    private void Update()
    {
        if (IsActiveCamera)
        {

        }

        this.transform.localPosition = Target.transform.position;
        ActionCamera.transform.LookAt(this.transform);
    }

    private void LookAt(GameObject Target)
    {
        this.Target = Target;
    }
}
