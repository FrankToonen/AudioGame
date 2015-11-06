using UnityEngine;
using System.Collections;

public class Pick : MonoBehaviour
{
    Manager mgr;
    int selectedIndex, lives, nextIndex, score;
    float moveSpeed;

    // Use this for initialization
    void Start()
    {
        mgr = GameObject.Find("Manager").GetComponent<Manager>();
        selectedIndex = 0;
        moveSpeed = 0.01f;
        lives = 5;
        nextIndex = 0;
        score = 0;
    }
	
    // Update is called once per frame
    void Update()
    {
        // Horizontaal
        if (Input.GetButtonDown("Fire1"))
        {
            ChangeIndex(1);
        } else if (Input.GetButtonDown("Fire2"))
        {
            ChangeIndex(-1);
        }

        // Verticaal
        if (Input.GetButton("Fire3"))
        {
            MoveLock(1);
        } else if (Input.GetButton("Fire4"))
        {
            MoveLock(-1);
        }

        mgr.locks [selectedIndex].GetComponent<Lock>().isMoving = Input.GetButton("Fire3") || Input.GetButton("Fire4");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool set = mgr.locks [selectedIndex].GetComponent<Lock>().LockLock(nextIndex);
            if (set)
                nextIndex++;
            else
                lives --;
        }

        if (lives <= 0)
        {
            Reset();
        }
    }

    void MoveLock(int dir)
    {
        GameObject selectedLock = mgr.locks [selectedIndex];
        if (!selectedLock.GetComponent<Lock>().isSet)
        {
            selectedLock.transform.position += new Vector3(0, dir * moveSpeed, 0);
            selectedLock.GetComponent<Lock>().isMoving = true;
        }
    }

    void ChangeIndex(int amount)
    {
        selectedIndex += amount;
        selectedIndex = Mathf.Clamp(selectedIndex, 0, 4);
        // Speel geluid af als clamp nodig is

        transform.position = new Vector3(selectedIndex, 1.5f, 0);
    }

    void Reset()
    {
        lives = 5;
        nextIndex = 0;
        ChangeIndex(-5);
        score = 0;
        mgr.Reset();
    }
}
