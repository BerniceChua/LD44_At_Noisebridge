using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{

    public AbilityType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

}

public enum AbilityType
{
    Basic,
    Dash,
    Diagonal
}
