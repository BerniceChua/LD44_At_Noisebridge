using UnityEngine;

public enum ability_type
{
    AbilityType_Invalid,
    
	AbilityType_Dash,
    AbilityType_Teleport
};

[System.Serializable]
public class ability_specification
{
    //the gameobject with script that interacts with the play area and player objects
    public GameObject Prefab_InteractiveAbility;
    //the gameobject with script that represents the ability choice in the user interface
    public GameObject Prefab_InterfaceAbility;
    //the type of ability
    public ability_type AbilityType;
}

[CreateAssetMenu (menuName="Ability Specification Group")]
public class ability_specification_group : ScriptableObject
{
    public ability_specification[] Abilities;
    
    public int IndexOfSpecification (ability_type Type)
    {
        int Result = -1;
        for (int i = 0; i < Abilities.Length; i++)
        {
            if (Abilities[i].AbilityType == Type)
            {
                Result = i;
                break;
            }
        }
        return Result;
    }
}