// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""66c72468-5f38-4f4c-83d7-cc13d5c5be2a"",
            ""actions"": [
                {
                    ""name"": ""Tilespin"",
                    ""type"": ""Button"",
                    ""id"": ""60a427e7-0234-4275-b584-d471fc38b71f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rayposition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""8dbee755-6ef0-4280-9afa-c5b6585c499c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b48b87f4-cd4e-4932-8426-34b849e0db35"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Tilespin"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b703c0f5-3ee8-40cf-aeb0-5d19f4e1d7fb"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Rayposition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and mouse"",
            ""bindingGroup"": ""Keyboard and mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Tilespin = m_Player.FindAction("Tilespin", throwIfNotFound: true);
        m_Player_Rayposition = m_Player.FindAction("Rayposition", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Tilespin;
    private readonly InputAction m_Player_Rayposition;
    public struct PlayerActions
    {
        private @InputMaster m_Wrapper;
        public PlayerActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Tilespin => m_Wrapper.m_Player_Tilespin;
        public InputAction @Rayposition => m_Wrapper.m_Player_Rayposition;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Tilespin.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTilespin;
                @Tilespin.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTilespin;
                @Tilespin.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTilespin;
                @Rayposition.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRayposition;
                @Rayposition.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRayposition;
                @Rayposition.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRayposition;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Tilespin.started += instance.OnTilespin;
                @Tilespin.performed += instance.OnTilespin;
                @Tilespin.canceled += instance.OnTilespin;
                @Rayposition.started += instance.OnRayposition;
                @Rayposition.performed += instance.OnRayposition;
                @Rayposition.canceled += instance.OnRayposition;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyboardandmouseSchemeIndex = -1;
    public InputControlScheme KeyboardandmouseScheme
    {
        get
        {
            if (m_KeyboardandmouseSchemeIndex == -1) m_KeyboardandmouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and mouse");
            return asset.controlSchemes[m_KeyboardandmouseSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnTilespin(InputAction.CallbackContext context);
        void OnRayposition(InputAction.CallbackContext context);
    }
}
