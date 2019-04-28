using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CardControl : MonoBehaviour
{

    public Button[] m_button;
    private Vector2[] m_dest;

    public bool tweenUse = false;
    private float tweenPos = 0f;
    public float tweenLen = 0.1f;

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

    // Update is called once per frame
    void Update()
    {
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
