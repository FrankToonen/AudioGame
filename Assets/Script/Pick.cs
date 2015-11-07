using UnityEngine;
using System.Collections;

public class Pick : SoundGameObject
{
    Manager mgr;
    int selectedIndex, lives, nextIndex, score;
    float moveSpeed;

    protected override void Start()
    {
        base.Start();

        mgr = GameObject.Find("Manager").GetComponent<Manager>();

        selectedIndex = 0;
        moveSpeed = 0.01f;
        lives = 5;
        nextIndex = 0;
        score = 0;
    }
	
    void Update()
    {
        ProcesInput();

        for (int i = 0; i < mgr.AmountOfPins; i++)
        {
            Pin pin = mgr.pins [i].GetComponent<Pin>();
            pin.isMoving = i == selectedIndex;
        }

        if (lives <= 0)
        {
            mgr.PlaySound("Fail");
            Reset();
        }
    }

    void ProcesInput()
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
        
        if (Input.GetButtonUp("Fire3"))
        {
            StopSound();
        } 
        
        if (Input.GetButtonUp("Fire4"))
        {
            FeelPin();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool set = mgr.pins [selectedIndex].GetComponent<Pin>().LockPin(nextIndex);
            if (set)
            {
                nextIndex++;
                PlayOneShot("click");
            } else
            {
                lives --;
                PlayOneShot("break");
            }
        }
    }

    void MovePin()
    {
        Pin selectedPin = mgr.pins [selectedIndex].GetComponent<Pin>();
        if (!selectedPin.GetComponent<Pin>().isSet)
        {
            selectedPin.transform.position += new Vector3(0, moveSpeed, 0);
            //selectedPin.isMoving = true;
            PlaySound("tinkering");
        }
    }

    void ChangeIndex(int amount)
    {
        selectedIndex += amount;

        if (selectedIndex < 0 || selectedIndex >= mgr.pins.Length)
        {
            selectedIndex = Mathf.Clamp(selectedIndex, 0, 4);
            PlayOneShot("edge", 0.4f);
        } else
        {
            FeelPin();
        }

        transform.position = new Vector3(selectedIndex, -1.5f, 0);

    }

    void FeelPin()
    {
        float pitch = ((float)(mgr.pins [selectedIndex].GetComponent<Pin>().Index + 1) / mgr.AmountOfPins) + 0.5f;
        PlayOneShot("movepick", 1, pitch);
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
