using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputMaster controls;

    [SerializeField]
    private Hexmap map;

    [SerializeField]
    private PathCreator creator;
    private AbilityManager abilitymanager;

    private HandCards cardManager;

    private void Start()
    {
        cardManager = FindObjectOfType<HandCards>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Awake()
    {
        controls = new InputMaster();
        abilitymanager = FindObjectOfType<AbilityManager>();

        // Abilities
        controls.Player.CircleF1.performed += ctx => RunAbility(PlayerManager.Instance.selectedCharacter.GetComponent<Character>(), GestureType.circle);

        controls.Player.Spacebutton.performed += ctx => map.randomizeHexmap(1000, 3);
        controls.Player.EnterPress.performed += ctx => SpawnTrap();
        

    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2))
            RunAbility(PlayerManager.Instance.selectedCharacter.GetComponent<Character>(), GestureType.verticalline);
        if (Input.GetKeyDown(KeyCode.F3))
            RunAbility(PlayerManager.Instance.selectedCharacter.GetComponent<Character>(), GestureType.horizontalline);
    }

    private void RunAbility(Character character, GestureType type)
    {
        if (PlayerManager.Instance.PlayerState == PlayerState.idle)
        {
            Debug.Log("Running ability " + type + " of: " + character.name);
            //abilitymanager.ActivateAbilityFromGesture(type, character);
            cardManager.activateCard(type);
        }
    }

    private void SpawnTrap()
    {
        map.ChangeEffect(2,2, true, ElementState.Fire, -50);
        Debug.Log("Trap spawned: " + map.map[2, 2].areaEffect.TrapElement + " with damage " + map.map[2, 2].areaEffect.healthModifier);
    }
}
