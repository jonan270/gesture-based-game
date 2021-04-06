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
        controls.Player.DrawPath.performed += ctx => addShit();
        controls.Player.EnterPress.performed += ctx => SpawnTrap();

        controls.Player.Select1.performed += ctx => addShit();
        controls.Player.Select2.performed += ctx => addOtherShit();

    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2))
            RunAbility(PlayerManager.Instance.selectedCharacter.GetComponent<Character>(), GestureType.cross);
        if (Input.GetKeyDown(KeyCode.F3))
            RunAbility(PlayerManager.Instance.selectedCharacter.GetComponent<Character>(), GestureType.square);
    }

    private void addShit() 
    {
        if (PlayerManager.Instance.PlayerState != PlayerState.waitingForMyTurn && PlayerManager.Instance.selectedCharacter != null)
        {
            creator.AddTile(map.map[0, 0]);
            creator.AddTile(map.map[1, 1]);
            creator.AddTile(map.map[2, 2]);
            creator.AddTile(map.map[3, 4]);
            creator.AddTile(map.map[3, 5]);
            creator.AddTile(map.map[4, 4]);

            creator.FinishPath(PlayerManager.Instance.selectedCharacter);

        }
    }

    private void addOtherShit() 
    {
        if (PlayerManager.Instance.PlayerState != PlayerState.waitingForMyTurn && PlayerManager.Instance.selectedCharacter != null)
        {
            creator.AddTile(map.map[5, 0]);
            creator.AddTile(map.map[6, 1]);
            creator.AddTile(map.map[7, 2]);
            creator.AddTile(map.map[8, 3]);
            creator.AddTile(map.map[9, 4]);
            creator.AddTile(map.map[10, 5]);

            creator.FinishPath(PlayerManager.Instance.selectedCharacter);
        }
    }

    private void RunAbility(Character character, GestureType type)
    {
        if (PlayerManager.Instance.PlayerState == PlayerState.idle)
        {
            Debug.Log("Running ability " + type + " of: " + character.name);
            abilitymanager.ActivateAbilityFromGesture(type, character);
        }
    }

    private void SpawnTrap()
    {
        map.ChangeEffect(2,2, true, ElementState.Fire, -50);
        Debug.Log("Trap spawned: " + map.map[2, 2].areaEffect.TrapElement + " with damage " + map.map[2, 2].areaEffect.healthModifier);
    }
}
