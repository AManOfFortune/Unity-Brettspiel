//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/InputSettings/CameraControlActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @CameraControlActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @CameraControlActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CameraControlActions"",
    ""maps"": [
        {
            ""name"": ""CameraActionMap"",
            ""id"": ""e30fb7d0-1de9-4cb5-a577-67749545d3e7"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""b9e35604-8823-4cf3-9f16-cb0f2bfafdb6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""4d930956-6c21-4f40-bf53-022a5dcbc50d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""19af364e-3d0f-43e7-aa52-f24fd8c7398c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Swap"",
                    ""type"": ""Button"",
                    ""id"": ""918306d7-364d-49a0-a744-b19873e39de4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Zoom2"",
                    ""type"": ""Button"",
                    ""id"": ""ef6069ba-50fb-41ca-a80d-7b012964ebff"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold"",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""b77dc5b2-57a1-432e-b7c8-6e2b6d21a973"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6171f3b3-d732-4200-b6b8-4c7e37d7b5a5"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f58227c2-3696-48f5-a715-2c1954a645b5"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6f4bd2fb-d229-44f7-a91a-cc5409cf1daf"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""92dddf5f-13c9-4062-88da-2334043badbf"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a621428f-6ae6-4655-8bcb-6e8ed51fc615"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e8a94714-d65c-40c4-a9fd-cba38dbd77d3"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d8e6930-1d09-4bf3-8f49-93804da683a7"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""cb4ace40-f2d5-4f3c-8a2f-eb498fe6e096"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom2"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""5ead2976-70bb-492c-8acb-b2d4687deb59"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""bd19a6af-7806-41b8-bf5f-a7b7f3c11f2d"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // CameraActionMap
        m_CameraActionMap = asset.FindActionMap("CameraActionMap", throwIfNotFound: true);
        m_CameraActionMap_Movement = m_CameraActionMap.FindAction("Movement", throwIfNotFound: true);
        m_CameraActionMap_Rotate = m_CameraActionMap.FindAction("Rotate", throwIfNotFound: true);
        m_CameraActionMap_Zoom = m_CameraActionMap.FindAction("Zoom", throwIfNotFound: true);
        m_CameraActionMap_Swap = m_CameraActionMap.FindAction("Swap", throwIfNotFound: true);
        m_CameraActionMap_Zoom2 = m_CameraActionMap.FindAction("Zoom2", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // CameraActionMap
    private readonly InputActionMap m_CameraActionMap;
    private ICameraActionMapActions m_CameraActionMapActionsCallbackInterface;
    private readonly InputAction m_CameraActionMap_Movement;
    private readonly InputAction m_CameraActionMap_Rotate;
    private readonly InputAction m_CameraActionMap_Zoom;
    private readonly InputAction m_CameraActionMap_Swap;
    private readonly InputAction m_CameraActionMap_Zoom2;
    public struct CameraActionMapActions
    {
        private @CameraControlActions m_Wrapper;
        public CameraActionMapActions(@CameraControlActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_CameraActionMap_Movement;
        public InputAction @Rotate => m_Wrapper.m_CameraActionMap_Rotate;
        public InputAction @Zoom => m_Wrapper.m_CameraActionMap_Zoom;
        public InputAction @Swap => m_Wrapper.m_CameraActionMap_Swap;
        public InputAction @Zoom2 => m_Wrapper.m_CameraActionMap_Zoom2;
        public InputActionMap Get() { return m_Wrapper.m_CameraActionMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActionMapActions set) { return set.Get(); }
        public void SetCallbacks(ICameraActionMapActions instance)
        {
            if (m_Wrapper.m_CameraActionMapActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnMovement;
                @Rotate.started -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnRotate;
                @Zoom.started -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnZoom;
                @Swap.started -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnSwap;
                @Swap.performed -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnSwap;
                @Swap.canceled -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnSwap;
                @Zoom2.started -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnZoom2;
                @Zoom2.performed -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnZoom2;
                @Zoom2.canceled -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnZoom2;
            }
            m_Wrapper.m_CameraActionMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
                @Swap.started += instance.OnSwap;
                @Swap.performed += instance.OnSwap;
                @Swap.canceled += instance.OnSwap;
                @Zoom2.started += instance.OnZoom2;
                @Zoom2.performed += instance.OnZoom2;
                @Zoom2.canceled += instance.OnZoom2;
            }
        }
    }
    public CameraActionMapActions @CameraActionMap => new CameraActionMapActions(this);
    public interface ICameraActionMapActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
        void OnSwap(InputAction.CallbackContext context);
        void OnZoom2(InputAction.CallbackContext context);
    }
}
