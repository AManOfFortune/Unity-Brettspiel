using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwapper : MonoBehaviour
{
    public GameObject ActionCameraPivot;
    public GameObject MapCameraPivot;

    private Camera ActionCamera;
    private Camera MapCamera;

    private ActionCameraController ActionController;
    private MapCameraController MapController;

    private CameraControlActions cameraActions;

    bool MapIsActive;

    private void Awake()
    {
        ActionCamera = ActionCameraPivot.GetComponentInChildren<Camera>();
        MapCamera = MapCameraPivot.GetComponentInChildren<Camera>();

        ActionController = ActionCameraPivot.GetComponent<ActionCameraController>();
        MapController = MapCameraPivot.GetComponent<MapCameraController>();

        cameraActions = new CameraControlActions();

        MapIsActive = false;
        ShrinkCamera(MapCamera);
        GrowCamera(ActionCamera);
    }

    private void OnEnable()
    {
        cameraActions.CameraSwapping.Enable();
        cameraActions.CameraSwapping.Swap.performed += SwapCamera;
    }

    private void OnDisable()
    {
        cameraActions.CameraSwapping.Disable();
        cameraActions.CameraSwapping.Swap.performed -= SwapCamera;
    }

    public void SwapCamera(InputAction.CallbackContext obj)
    {
        Debug.Log("Swapping Camera");

        if (MapIsActive)
        {
            ShrinkCamera(MapCamera);
            MapController.Disable();
            GrowCamera(ActionCamera);
            ActionController.Enable();
        } else
        {
            ShrinkCamera(ActionCamera);
            MapController.Enable();
            GrowCamera(MapCamera);
            ActionController.Disable();
        }

        MapIsActive = !MapIsActive;
    }

    private void ShrinkCamera(Camera Camera)
    {
        Camera.rect = new Rect(0, 0.65f, 0.35f, 0.35f);
    }

    private void GrowCamera(Camera Camera)
    {
        Camera.rect = new Rect(0, 0, 1, 1);
    }
}
