using UnityEngine;
using System.Collections;

public class Pick : MonoBehaviour
{
    Manager mgr;
    int selectedIndex, lives, nextIndex, score;
    float moveSpeed;

    void Start()
    {
        mgr = GameObject.Find("Manager").GetComponent<Manager>();

        selectedIndex = 0;
        moveSpeed = 0.01f;
        lives = 5;
        nextIndex = 0;
        score = 0;
    }
	
    void Update()
    {
        // Horizontaal
        if (Input.GetButtonDown("Fire1"))
        {
            ChangeIndex(-1);
        } else if (Input.GetButtonDown("Fire2"))
        {
            ChangeIndex(1);
        }

        // Verticaal
        if (Input.GetButton("Fire3"))
        {
            MovePin();
        }

        mgr.locks [selectedIndex].GetComponent<Pin>().isMoving = Input.GetButton("Fire3");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool set = mgr.locks [selectedIndex].GetComponent<Pin>().LockPin(nextIndex);
            if (set)
            {
                nextIndex++;
                mgr.PlaySound("LockPin");
            } else
            {
                lives --;
                mgr.PlaySound("BreakPick");
            }
        }

        if (lives <= 0)
        {
            mgr.PlaySound("Fail");
            Reset();
        }
    }

    void MovePin()
    {
        GameObject selectedPin = mgr.locks [selectedIndex];
        if (!selectedPin.GetComponent<Pin>().isSet)
        {
            selectedPin.transform.position += new Vector3(0, moveSpeed, 0);
            selectedPin.GetComponent<Pin>().isMoving = true;
            mgr.PlaySound("MovePinUp");
        }
    }

    void ChangeIndex(int amount)
    {
        selectedIndex += amount;

        if (selectedIndex < 0 || selectedIndex >= mgr.locks.Length)
        {
            selectedIndex = Mathf.Clamp(selectedIndex, 0, 4);
            mgr.PlaySound("edge");
        }

        transform.position = new Vector3(selectedIndex, -1.5f, 0);

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
