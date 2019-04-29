using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    Camera camera;

    public int posX, posY, levelSize;

    public Transform myspace;

    public bool Cheated;

    [SerializeField]
    public Transform levelObject;

    [SerializeField]
    GameObject backTrackPrefab;

    public int wine, grapes, wineLoss;

    private bool m_reachedExit = false;
    public bool ReachedExit {
        get { return m_reachedExit; }
        set { m_reachedExit = value; }
    }

    //variable per-level properties
    public int grapeHealAmount;
    public int wineHealAmount;
    public int backtrackDamage;
    public int hazardDamage;
    public int walkDamage;
    public int playerStartWine;
    public int playerMaxWine;

    void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            Debug.Log("cheated");
            Cheated = true;
        }
    }

    public Transform GetCurrentSpaceTransform()
    {
        return levelObject.GetChild((levelSize * posX) + posY);
    }

    public void sfx(int id)
    {
        GameObject.Find("SoundStuff").GetComponent<SoundController>().cueSFXRandom(id);
    }
    public void sfxWalk()
    {
        sfx(0);
    }
    public void sfxGrape()
    {
        sfx(1);
    }
    public void sfxHazard()
    {
        sfx(2);
    }
    public void sfxPress()
    {
        sfx(3);
    }
    public void sfxFire()
    {
        sfx(4);
    }
    public void sfxWine()
    {
        sfx(5);
    }
    public void sfxExit()
    {
        sfx(6);
    }

    //move to a space, given it's map coords
    public void MoveToSpace(int x, int y, Transform ps = null)
    {
        Debug.Log("movetospace" + x+ "," + y);
        Transform spaceobj = levelObject.GetChild((levelSize * x) + y);
        Debug.Log(levelSize * x + y);
        myspace = spaceobj;
        Debug.Log(myspace);
        Debug.Log(spaceobj.GetComponent<MoveSpace>().hasPickup);
        transform.position = new Vector3(spaceobj.position.x, .8f, spaceobj.position.z);
        //Debug.Break();
        Debug.Log(spaceobj);
        transform.SetParent(spaceobj);
        //Debug.Break();

        // turn player towards direction of travel
        if (posX < x)
        {
            transform.localEulerAngles = new Vector3(0f, 90f, 0f);
        }
        if (posX > x)
        {
            transform.localEulerAngles = new Vector3(0f, 270f, 0f);
        }
        if (posY < y)
        {
            transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
        if (posY > y)
        {
            transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        }

        // turn dionysus towards player
        foreach (GameObject pickup in GameObject.FindGameObjectsWithTag("pickup")) {
            if (pickup.name.Contains("dionysus")) {
                Vector3 direction = transform.position - pickup.transform.position;
                direction.y = 0;
                pickup.transform.localRotation = Quaternion.LookRotation(direction, pickup.transform.up);
            }
        }

        posX = x;
        posY = y;

        // 12:00 - the x and y are the same
        // 12:10 - myspace points to a seemingly unrelated object
        // 12:18 - it is not because the number of children has changed because of subobjects
        if (spaceobj.GetComponent<MoveSpace>().hasPickup)
        {
            spaceobj.GetComponent<MoveSpace>().hasPickup = false;

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

                if (pickupobj.name.Contains("Grape")) {
                    sfxGrape();
                    grapes += grapeHealAmount;
                    Destroy(pickupobj);
                    InstantiateBackTrackHazard(ps);
                } else if (pickupobj.name.Contains("Hazard")) {
                    sfxHazard();
                    spaceobj.GetComponent<MoveSpace>().hasPickup = true;

                    wine -= hazardDamage;
                } else if (pickupobj.name.Contains("Press")) {
                    sfxPress();
                    spaceobj.GetComponent<MoveSpace>().hasPickup = true;

                    wine += grapes;
                    wine = Mathf.Clamp(wine, 0, playerMaxWine + 1);
                    grapes = 0;
                    // it's a berry simple formula
                } else if (pickupobj.name.Contains("fire")) {
                    sfxFire();
                    wine -= backtrackDamage;
                } else if (pickupobj.name.Contains("Wine")) {
                    sfxWine();
                    wine += wineHealAmount;
                    wine = Mathf.Clamp(wine, 0, playerMaxWine + 1);

                    Destroy(pickupobj);
                    InstantiateBackTrackHazard(ps);
                } else if (pickupobj.name.Contains("dionysus")) {
                    sfxExit();
                    /// This is the 'success' condition, to exit the game loop. ^_^
                    m_reachedExit = true;
                }
                else
                {
                    sfxWalk();
                }
            }

        }
        //if we were passed a previous player space
        //check if we should add backtrack hazard
        if (ps != null)
        {
            wine -= spaceobj.GetComponent<MoveSpace>().wineLoss;

            //wine -= ps.GetComponent<MoveSpace>().wineLoss;
            //if no pickup, create backtrack hazard
            if (!ps.GetComponent<MoveSpace>().hasPickup)
            {
                InstantiateBackTrackHazard(ps);
            }
            //otherwise, check the pickup type, and selectively place one based on that type
            else
            {
                GameObject pickupobj = null;
                foreach (Transform child in ps)
                {
                    if (child.tag == "pickup")
                    {
                        pickupobj = child.gameObject;
                        break;
                    }
                }
                if (pickupobj != null)
                {
                    if (pickupobj.name.Contains("Grape") || pickupobj.name.Contains("Wine"))
                        InstantiateBackTrackHazard(ps);
            }
        }
        }
        Debug.Log("end of movetospace"+spaceobj);
    }

    void OnDestroy()
    {
        Debug.Log(UnityEngine.StackTraceUtility.ExtractStackTrace());
        //Debug.Break();
    }

    public void InstantiateBackTrackHazard(Transform playerSpace)
    {
        GameObject pickup = (GameObject)Instantiate(backTrackPrefab, playerSpace);
        pickup.transform.localPosition = new Vector3(0f, 4.5f, 0f);
        pickup.transform.localScale = new Vector3(1f, 1f / pickup.transform.parent.localScale.y, 1f);
        playerSpace.GetComponent<MoveSpace>().hasPickup = true;
    }

    //move to a space, given it's transform component
    //ps is previous space we are moving from
    public void MoveToSpace(Transform t, Transform ps)
    {
        MoveToSpace(t.GetComponent<MoveSpace>().posX, t.GetComponent<MoveSpace>().posY, ps);
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

    public void ResetExitStatus() {
        m_reachedExit = false;
    }
}
