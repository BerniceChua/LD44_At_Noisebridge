using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardControl : MonoBehaviour
{

    public Button[] m_button;
    private Vector2[] m_dest;

    [SerializeField]
    public Transform levelObject;
    [SerializeField]
    public PlayerController player;

    int levelSize = 7;

    public int dashLoss, diagLoss;

    public bool tweenUse = false;
    private float tweenPos = 0f;
    public float tweenLen = 0.1f;

    public GameObject hudtext;

    // Start is called before the first frame update
    void Start()
    {
        SyncButtons();
    }

    void SyncButtons()
    {
        m_dest = new Vector2[m_button.Length];
        for (int i=0; i<m_button.Length; i++)
        {
            m_button[i].onClick.RemoveAllListeners();
            m_button[i].onClick.AddListener(TaskOnClick);
            AbilityType abilityType = m_button[i].GetComponent<Ability>().type;
            m_button[i].onClick.AddListener(delegate { HighlightClickables(abilityType); });
            m_dest[i] = new Vector2(m_button[i].transform.position.x, m_button[i].transform.position.y);
        }
    }

    void TaskOnClick()
    {
        //Output this to console when Button1 or Button3 is clicked
        Debug.Log("You have clicked the button!");
        tweenPos = 0f;
        tweenUse = true;
    }

    MoveSpace getGrid(int x, int y)
    {
        if (x < 0 || x >= levelSize || y < 0 || y >= levelSize)
            return null;
        int idx = levelSize * x + y;
        if (idx >= 0 && idx < levelObject.childCount)
        {
            Transform tf = levelObject.GetChild(idx);
            if (tf != null && tf.GetComponent<MoveSpace>() != null)
                return tf.GetComponent<MoveSpace>();
            else return null;
        } else
        {
            return null;
        }
    }

    public bool Blocked(int x0, int y0, int x1, int y1)
    {
        MoveSpace space0 = getGrid(x0, y0);
        MoveSpace space1 = getGrid(x1, y1);
        int dirx = x1 - x0;
        int diry = y1 - y0;

        if (space0 == null || space1 == null) return true;

        if (dirx > 0)
        {
            // right
            return (space0.moveSpaceType == SpaceType.WallRight || space1.moveSpaceType == SpaceType.WallLeft || space1.moveSpaceType == SpaceType.WallTopLeft);
        }
        if (dirx < 0)
        {
            // left
            return (space0.moveSpaceType == SpaceType.WallLeft || space1.moveSpaceType == SpaceType.WallRight || space0.moveSpaceType == SpaceType.WallTopLeft);
        }
        if (diry > 0)
        {
            //2019-04-28 3:08 PM - if I flip down/up it fixes one bug and surfaces a different one.

            // down
            return (space0.moveSpaceType == SpaceType.WallTop || space1.moveSpaceType == SpaceType.WallBottom || space0.moveSpaceType == SpaceType.WallTopLeft);
        }
        if (diry < 0)
        {
            // up
            return (space0.moveSpaceType == SpaceType.WallBottom || space1.moveSpaceType == SpaceType.WallTop || space1.moveSpaceType == SpaceType.WallTopLeft);
        }
        return false;
    }

    void HighlightClickables(AbilityType at)
    {
        Debug.Log(at);

        // clear suggested moves
        GameObject.FindObjectOfType<LevelViewController>().ClearSuggestedMoves();

        // highlight by type
        //if (at == AbilityType.Basic)
        if (false) {
            int[,] offsets = new int[,] { { -1, 0 }, { 0, -1 }, { 1, 0 }, { 0, 1 } };
            Debug.Log(player.posX + ","+player.posY);
            for (int i=0; i<4; i++)
            {
                MoveSpace space = getGrid(offsets[i,0] + player.posX, offsets[i,1] + player.posY);
                int nx = player.posX;
                int ny = player.posY;
                if (space != null && !Blocked(nx, ny, nx + offsets[i, 0], ny + offsets[i, 1]))
                {
                    space.hasSuggestedMove = true;
                    space.wineLoss = 1;
                }
            }
        }
        if (at == AbilityType.Dash)
        {
            int[,] offsets = new int[,] { { -1, 0 }, { 0, -1 }, { 1, 0 }, { 0, 1 } };
            Debug.Log(player.posX + "," + player.posY);
            for (int i = 0; i < 4; i++)
            {
                int nx = player.posX;
                int ny = player.posY;
                MoveSpace ans = null;
                while (!Blocked(nx, ny, nx + offsets[i, 0], ny + offsets[i, 1]))
                {
                    nx += offsets[i, 0];
                    ny += offsets[i, 1];
                    ans = getGrid(nx, ny);
                }
                if (ans != null)
                {
                    ans.hasSuggestedMove = true;
                    ans.wineLoss = dashLoss;
                }
            }
        }
        else if (at == AbilityType.Diagonal)
        {
            int[,] offsets = new int[,] { { -1, -1 }, { 1, -1 }, { 1, 1 }, { -1, 1 } };
            Debug.Log(player.posX + "," + player.posY);
            for (int i = 0; i < 4; i++)
            {
                int nx0 = player.posX;
                int ny0 = player.posY;
                int nx1 = offsets[i, 0] + player.posX;
                int ny1 = offsets[i, 1] + player.posY;
                MoveSpace space = getGrid(nx1, ny1);
                if (space != null && ((!Blocked(nx0,ny0,nx1,ny0) && !Blocked(nx1,ny0,nx1,ny1)) || (!Blocked(nx0,ny0,nx0,ny1) && !Blocked(nx0,ny1,nx1,ny1)))) { 
                    space.hasSuggestedMove = true;
                    space.wineLoss = diagLoss;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        hudtext.GetComponent<Text>().text = "Grapes="+player.grapes+"  Wine="+player.wine+"/"+player.playerMaxWine  + "  PlX="+player.posX+"  PlY="+player.posY;
        if (tweenUse)
        {
            tweenPos += Time.deltaTime;
            for (int i = 0; i < m_button.Length; i++)
            {
                float pct = ((tweenPos - i * 0.1f) / tweenLen);
                if (pct > 1f) {
                    if (i == m_button.Length - 1)
                        tweenUse = false;
                    pct = 1f;
                }
                Vector3 pos = m_button[i].transform.position;
                pos.x = Mathf.SmoothStep(-100, m_dest[i].x, pct);
                pos.y = Mathf.SmoothStep(-100, m_dest[i].y, pct);
                m_button[i].transform.position = pos;
            }
        }
    }
}

