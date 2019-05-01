using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpaceTypePicker
{
    public const string spacetypeVarName = "spacetype";
    public SpaceType spacetype;

    public SpaceType basicType;
    

    public System.Enum Selected
    {
        get
        {
            switch (spacetype)
            {
                case SpaceType.Basic: return basicType;
                case SpaceType.NonSpace: return basicType;
                default:
                    Debug.LogError("SpaceTypePicker was passed an unimplemented type!!! " + spacetype.ToString());
                    break;
            }
            return spacetype;
        }
    }
}
