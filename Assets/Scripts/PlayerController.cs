using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    Camera camera;

    public int posX, posY, levelSize;

    public Transform myspace;

    [SerializeField]
    Transform levelObject;

    public int wine, grapes, wineLoss;

    private bool m_reachedExit = false;
    public bool ReachedExit {
        get { return m_reachedExit; }
    }



    public Transform GetCurrentSpaceTransform()
    {
        return levelObject.GetChild((levelSize * posX) + posY);
    }

    //move to a space, given it's map coords
    public void MoveToSpace(int x, int y)
    {
        Debug.Log("movetospace" + x+ "," + y);
        Transform spaceobj = levelObject.GetChild((levelSize * x) + y);
        Debug.Log(levelSize * x + y);
        myspace = spaceobj;
        Debug.Log(spaceobj.GetComponent<MoveSpace>().hasPickup);
        transform.position = new Vector3(spaceobj.position.x, .5f, spaceobj.position.z);
        transform.SetParent(spaceobj);
        posX = x;
        posY = y;
        wine -= wineLoss;
        // 12:00 - the x and y are the same
        // 12:10 - myspace points to a seemingly unrelated object
        // 12:18 - it is not because the number of children has changed because of subobjects
        if (spaceobj.GetComponent<MoveSpace>().hasPickup)
        {
            GameObject pickupobj = null;
            foreach(Transform child in spaceobj)
            {
                if (child.tag == "pickup")
                {
                    pickupobj = child.gameObject;
                    break;
                }
            }
            if (pickupobj != null)
            {
                Debug.Log("has pickup!" + pickupobj.name);

                if (pickupobj.name.Contains("Grape"))
                {
                    grapes += 3;
                    Destroy(pickupobj);
                }
                else if (pickupobj.name.Contains("Hazard"))
                {
                    wine -= 1;
                }
                else if (pickupobj.name.Contains("Press"))
                {
                    wine += grapes;
                    grapes = 0;
                    // it's a berry simple formula
                }
                else if (pickupobj.name.Contains("BackTrack"))
                {
                    wine -= 1;
                }
                else if (pickupobj.name.Contains("Wine"))
                {
                    wine += 1;
                } else if (pickupobj.name.Contains("dionysus")) {
                    /// This is the 'success' condition, to exit the game loop. ^_^
                    m_reachedExit = true;
                }
            }
        }
    }

    //move to a space, given it's transform component
    public void MoveToSpace(Transform t)
    {
        MoveToSpace(t.GetComponent<MoveSpace>().posX, t.GetComponent<MoveSpace>().posY);
    }

    // Used during the phases of the game where the player should be able to control their characters.
    public void EnableControl() {
        this.enabled = true;
    }

    /// Used during the phases of the game where the player shouldn't be able to control their characters.
    public void DisableControl() {
        this.enabled = false;
        //m_CanvasGameObject.SetActive(false);

        //this.m_ForwardSwimSpeed = 0.0f;
        this.transform.Translate(0.0f, 0.5f, 0.0f);
    }
}
