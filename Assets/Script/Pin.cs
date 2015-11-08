using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Pin : SoundGameObject
{
    Manager mgr;
    public float maxHeight, targetHeight;
    int index;
    public bool isSet, isMoving, hitTop;
    bool pastTarget;

    public void Initialize(int index)
    {
        mgr = GameObject.Find("Manager").GetComponent<Manager>();
        this.index = index;
        targetHeight = 0.2f;
        Reset();
    }
	
    void Update()
    {
        // Als de juiste pin te ver omhoog bewogen wordt:
        if (transform.position.y >= maxHeight)
        {
            // Als de juiste pin geselecteerd is, verlies een leven en begin opnieuw:
            if (mgr.MatchIndices())
            {
                mgr.player.LoseLife();
                mgr.PlayOneShot("break");
                mgr.Reset();
            } // Anders speel geluid af:
            else
            {
                if (!hitTop)
                {
                    hitTop = true;
                    PlayOneShot("edge");
                }
            }
        } // Anders als hij in een bepaald bereik van de maximale hoogte is:
        else if (Mathf.Abs(transform.position.y - maxHeight) < targetHeight && !isSet && !pastTarget && mgr.MatchIndices())
        {
            PlaySound("click");
            pastTarget = true;
        } 

        // Als de pin niet geselecteerd is, beweeg hem dan terug naar de startpositie:
        if (!isSet && !isMoving && transform.position.y != 0)
        {
            float newY = Mathf.Clamp(transform.position.y - 0.05f, 0, maxHeight);
            transform.position = new Vector3(transform.position.x, newY, 0);
            hitTop = false;

            // Speel geluid als de pin de bodem raakt?:
            if (transform.position.y == 0)
            {
                PlaySound("edge");
            }
        }
    }

    // Genereer een random waarde voor de hoogte:
    void GenerateHeight()
    {
        maxHeight = Random.value /** 2*/ + targetHeight;
    }

    // Zet het slot vast onder bepaalde voorwaarde:
    public bool LockPin(int i)
    {
        if (Mathf.Abs(transform.position.y - maxHeight) < 0.2 && index == i)
        {
            isSet = true;
            return true;
        }

        return false;
    }

    public void Reset()
    {
        isSet = false;
        pastTarget = false;
        hitTop = false;
        GenerateHeight();
    }

    public int Index
    {
        get { return index;  }
    }
}
