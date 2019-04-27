using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class SFXGroup
{
    public AudioSource[] sfx;
}

public class SoundController : MonoBehaviour {

    public SFXGroup[] sfxgroup;
    public AudioSource[] music;

    public AudioSource curMusicCue;
    public AudioSource nextMusicCue;
    public float cueInTime = 0.1f;
    public float cueInDelay = 1f;
    public float cueOutTime = 1f;
    public float cueProgress = 0f;

    public Text debugText;

    // Use this for initialization
    void Start () {
        cueMusic(1);
        cueProgress = cueInDelay + cueInTime;
        nextMusicCue.volume = 1f;
	}

	void CompleteCue()
    {
        if (curMusicCue != null)
            curMusicCue.Stop();
        curMusicCue = nextMusicCue;
        nextMusicCue.Play();
        nextMusicCue = null;
        cueProgress = 0f;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown("1"))
        {
            cueMusic(0);
        }
        if (Input.GetKeyDown("2"))
        {
            cueMusic(1);
        }
        if (Input.GetKeyDown("3"))
        {
            cueSFXRandom(0);
        }

        if (nextMusicCue != null)
        {
            if (nextMusicCue != curMusicCue)
            {
                if (curMusicCue == null)
                {
                    CompleteCue();
                } else
                {
                    curMusicCue.volume = Mathf.Lerp(0f, 1f, 1f - (cueProgress / cueOutTime));
                    if (cueProgress > cueInDelay)
                    {
                        nextMusicCue.Play();
                    }
                    nextMusicCue.volume = Mathf.Lerp(0f, 1f, (cueProgress - cueInDelay) / cueInTime);
                    if (curMusicCue.volume <= 0f)
                        CompleteCue();
                    cueProgress += Time.deltaTime;
                }
            }
            else
            {
                cueProgress = 0f;
                if (curMusicCue.volume < 1f)
                {
                    curMusicCue.volume = Mathf.Min(1f, curMusicCue.volume + Time.deltaTime);
                }
            }

        } else
        {
            cueProgress = 0f;
            if (curMusicCue.volume < 1f)
            {
                curMusicCue.volume = Mathf.Min(1f, curMusicCue.volume + Time.deltaTime);
            }
        }

    }

    public void cueSFXRandom(int id)
    {
        sfxgroup[id].sfx[Random.Range(0,sfxgroup[id].sfx.Length)].Play();
    }
    public void cueMusic(int id)
    {
        if (curMusicCue == null || music[id] != curMusicCue || (nextMusicCue != null && music[id] != nextMusicCue))
        {
            nextMusicCue = music[id];
            cueProgress = 0f;
            nextMusicCue.volume = 0f;
        }
    }

}
