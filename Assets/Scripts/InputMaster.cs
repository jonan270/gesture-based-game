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
                    ""name"": ""Rayposition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""8dbee755-6ef0-4280-9afa-c5b6585c499c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Spacebutton"",
                    ""type"": ""Button"",
                    ""id"": ""9255cbf7-d7b0-4ed4-9a3c-4ead4dc22bee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DrawPath"",
                    ""type"": ""Button"",
                    ""id"": ""079e4293-eaa6-487c-86c9-d35ec6995cf0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EndTurn"",
                    ""type"": ""Button"",
                    ""id"": ""c8de2907-eddc-4949-b0b0-437f45e54acd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select1"",
                    ""type"": ""Button"",
                    ""id"": ""6fda314b-5120-42c1-a4e7-012243f2646b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select2"",
                    ""type"": ""Button"",
                    ""id"": ""3e96d4bb-a64c-4d60-a15f-4788f0eaee9f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
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
                },
                {
                    ""name"": """",
                    ""id"": ""86e6fff6-1b2d-42cc-b4e3-5351b93cd33a"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Spacebutton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""f55d444b-b654-4f03-97ae-a838e53b0621"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DrawPath"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""698bd2bf-a1d9-42a8-a7c2-0dc8f561905a"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""DrawPath"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f7fdc649-b9b9-4501-ba60-cce04634c0f0"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DrawPath"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2cc12beb-9c74-4402-b442-e7511bae1ae6"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DrawPath"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e8e315a7-18a2-4a6c-a52f-0c98684bca98"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DrawPath"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0672a72e-1cfc-412c-b2a5-d8484c186f3b"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""EndTurn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f814542-1df3-4d2c-bb11-e9805f8a6837"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3036820a-93d7-4080-8390-978f31e3024d"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select2"",
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
        m_Player_Rayposition = m_Player.FindAction("Rayposition", throwIfNotFound: true);
        m_Player_Spacebutton = m_Player.FindAction("Spacebutton", throwIfNotFound: true);
        m_Player_DrawPath = m_Player.FindAction("DrawPath", throwIfNotFound: true);
        m_Player_EndTurn = m_Player.FindAction("EndTurn", throwIfNotFound: true);
        m_Player_Select1 = m_Player.FindAction("Select1", throwIfNotFound: true);
        m_Player_Select2 = m_Player.FindAction("Select2", throwIfNotFound: true);
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
    private readonly InputAction m_Player_Rayposition;
    private readonly InputAction m_Player_Spacebutton;
    private readonly InputAction m_Player_DrawPath;
    private readonly InputAction m_Player_EndTurn;
    private readonly InputAction m_Player_Select1;
    private readonly InputAction m_Player_Select2;
    public struct PlayerActions
    {
        private @InputMaster m_Wrapper;
        public PlayerActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Rayposition => m_Wrapper.m_Player_Rayposition;
        public InputAction @Spacebutton => m_Wrapper.m_Player_Spacebutton;
        public InputAction @DrawPath => m_Wrapper.m_Player_DrawPath;
        public InputAction @EndTurn => m_Wrapper.m_Player_EndTurn;
        public InputAction @Select1 => m_Wrapper.m_Player_Select1;
        public InputAction @Select2 => m_Wrapper.m_Player_Select2;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Rayposition.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRayposition;
                @Rayposition.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRayposition;
                @Rayposition.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRayposition;
                @Spacebutton.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpacebutton;
                @Spacebutton.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpacebutton;
                @Spacebutton.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpacebutton;
                @DrawPath.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDrawPath;
                @DrawPath.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDrawPath;
                @DrawPath.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDrawPath;
                @EndTurn.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEndTurn;
                @EndTurn.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEndTurn;
                @EndTurn.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEndTurn;
                @Select1.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect1;
                @Select1.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect1;
                @Select1.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect1;
                @Select2.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect2;
                @Select2.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect2;
                @Select2.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect2;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Rayposition.started += instance.OnRayposition;
                @Rayposition.performed += instance.OnRayposition;
                @Rayposition.canceled += instance.OnRayposition;
                @Spacebutton.started += instance.OnSpacebutton;
                @Spacebutton.performed += instance.OnSpacebutton;
                @Spacebutton.canceled += instance.OnSpacebutton;
                @DrawPath.started += instance.OnDrawPath;
                @DrawPath.performed += instance.OnDrawPath;
                @DrawPath.canceled += instance.OnDrawPath;
                @EndTurn.started += instance.OnEndTurn;
                @EndTurn.performed += instance.OnEndTurn;
                @EndTurn.canceled += instance.OnEndTurn;
                @Select1.started += instance.OnSelect1;
                @Select1.performed += instance.OnSelect1;
                @Select1.canceled += instance.OnSelect1;
                @Select2.started += instance.OnSelect2;
                @Select2.performed += instance.OnSelect2;
                @Select2.canceled += instance.OnSelect2;
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
        void OnRayposition(InputAction.CallbackContext context);
        void OnSpacebutton(InputAction.CallbackContext context);
        void OnDrawPath(InputAction.CallbackContext context);
        void OnEndTurn(InputAction.CallbackContext context);
        void OnSelect1(InputAction.CallbackContext context);
        void OnSelect2(InputAction.CallbackContext context);
    }
}
