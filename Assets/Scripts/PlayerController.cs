using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    Camera camera;

    public int posX, posY, levelSize;

    [SerializeField]
    Transform levelObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //move to a space, given it's map coords
    public void MoveToSpace(int x, int y)
    {
        Debug.Log(x+ "," + y);
        Transform spaceobj = levelObject.GetChild((levelSize * x) + y);
        transform.position = new Vector3(spaceobj.position.x, .5f, spaceobj.position.z);
        transform.SetParent(spaceobj);
    }

    //move to a space, given it's transform component
    public void MoveToSpace(Transform t)
    {
        transform.position = new Vector3(t.position.x, .5f, t.position.z);
        transform.SetParent(t);
    }
}
