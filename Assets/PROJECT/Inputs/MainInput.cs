//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/PROJECT/Inputs/Main.inputactions
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

public partial class @MainInput : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @MainInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Main"",
    ""maps"": [
        {
            ""name"": ""Map"",
            ""id"": ""3f315e98-3ea5-4f2a-a3e5-9837a1262b8f"",
            ""actions"": [
                {
                    ""name"": ""Scroll"",
                    ""type"": ""Value"",
                    ""id"": ""e50e5120-3d70-4742-a973-b42db2216146"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Tab"",
                    ""type"": ""Button"",
                    ""id"": ""79dadf00-e04d-48a9-8008-ae3cde0426de"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TabPosition"",
                    ""type"": ""Value"",
                    ""id"": ""b10cdfdd-0e8b-4328-8c62-9f85f8040516"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6e561a21-c057-4771-88b1-afe4909a3786"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7723b8c-ceb3-4312-a510-8c873a6f093d"",
                    ""path"": ""<Touchscreen>/primaryTouch/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc6676cb-e220-4d5d-871c-48b9113e0810"",
                    ""path"": ""<Pointer>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04d53622-eb2f-4839-853d-31f41187738d"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TabPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Map
        m_Map = asset.FindActionMap("Map", throwIfNotFound: true);
        m_Map_Scroll = m_Map.FindAction("Scroll", throwIfNotFound: true);
        m_Map_Tab = m_Map.FindAction("Tab", throwIfNotFound: true);
        m_Map_TabPosition = m_Map.FindAction("TabPosition", throwIfNotFound: true);
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

    // Map
    private readonly InputActionMap m_Map;
    private IMapActions m_MapActionsCallbackInterface;
    private readonly InputAction m_Map_Scroll;
    private readonly InputAction m_Map_Tab;
    private readonly InputAction m_Map_TabPosition;
    public struct MapActions
    {
        private @MainInput m_Wrapper;
        public MapActions(@MainInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Scroll => m_Wrapper.m_Map_Scroll;
        public InputAction @Tab => m_Wrapper.m_Map_Tab;
        public InputAction @TabPosition => m_Wrapper.m_Map_TabPosition;
        public InputActionMap Get() { return m_Wrapper.m_Map; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MapActions set) { return set.Get(); }
        public void SetCallbacks(IMapActions instance)
        {
            if (m_Wrapper.m_MapActionsCallbackInterface != null)
            {
                @Scroll.started -= m_Wrapper.m_MapActionsCallbackInterface.OnScroll;
                @Scroll.performed -= m_Wrapper.m_MapActionsCallbackInterface.OnScroll;
                @Scroll.canceled -= m_Wrapper.m_MapActionsCallbackInterface.OnScroll;
                @Tab.started -= m_Wrapper.m_MapActionsCallbackInterface.OnTab;
                @Tab.performed -= m_Wrapper.m_MapActionsCallbackInterface.OnTab;
                @Tab.canceled -= m_Wrapper.m_MapActionsCallbackInterface.OnTab;
                @TabPosition.started -= m_Wrapper.m_MapActionsCallbackInterface.OnTabPosition;
                @TabPosition.performed -= m_Wrapper.m_MapActionsCallbackInterface.OnTabPosition;
                @TabPosition.canceled -= m_Wrapper.m_MapActionsCallbackInterface.OnTabPosition;
            }
            m_Wrapper.m_MapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Scroll.started += instance.OnScroll;
                @Scroll.performed += instance.OnScroll;
                @Scroll.canceled += instance.OnScroll;
                @Tab.started += instance.OnTab;
                @Tab.performed += instance.OnTab;
                @Tab.canceled += instance.OnTab;
                @TabPosition.started += instance.OnTabPosition;
                @TabPosition.performed += instance.OnTabPosition;
                @TabPosition.canceled += instance.OnTabPosition;
            }
        }
    }
    public MapActions @Map => new MapActions(this);
    public interface IMapActions
    {
        void OnScroll(InputAction.CallbackContext context);
        void OnTab(InputAction.CallbackContext context);
        void OnTabPosition(InputAction.CallbackContext context);
    }
}
