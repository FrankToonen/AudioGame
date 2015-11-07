using UnityEngine;
using System.Collections;

public class Pin : SoundGameObject
{
    Manager mgr;
    public float maxHeight, targetHeight;
    int index;
    public bool isSet, isMoving;
    bool pastTarget;

    public void Initialize(int index)
    {
        mgr = GameObject.Find("Manager").GetComponent<Manager>();
        this.index = index;
        targetHeight = 0.2f;
        isSet = false;
        pastTarget = false;
        GenerateHeight();
    }
	
    void Update()
    {
        if (transform.position.y > maxHeight)
        {
            PlayOneShot("Fail");
            mgr.Reset();
        } else if (Mathf.Abs(transform.position.y - maxHeight) < targetHeight && !isSet && !pastTarget && mgr.MatchIndices())
        {
            PlaySound("click");
            pastTarget = true;
        }

        if (!isSet && !isMoving)
        {
            float newY = Mathf.Clamp(transform.position.y - 0.05f, 0, maxHeight);
            transform.position = new Vector3(transform.position.x, newY, 0);
            PlaySound("MovePinDown");
        }
    }

    void GenerateHeight()
    {
        maxHeight = Random.value * 2;
    }

    public void Reset()
    {
        transform.position = new Vector3(transform.position.x, 0, 0);
        GenerateHeight();
    }

    public bool LockPin(int i)
    {
        if (Mathf.Abs(transform.position.y - maxHeight) < 0.2 && index == i)
        {
            isSet = true;
            return true;
        }

        return false;
    }

    public int Index
    {
        get { return index;  }
    }
}
