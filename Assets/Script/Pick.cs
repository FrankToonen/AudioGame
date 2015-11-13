using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pick : SoundGameObject
{
    Manager mgr;
    Text scoreText, livesText;

    int selectedIndex, lives, nextIndex, score, startingIndex;
    float moveSpeed;
    bool isResetting;

    protected override void Start()
    {
        base.Start();

        mgr = GameObject.Find("Manager").GetComponent<Manager>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        livesText = GameObject.Find("LivesText").GetComponent<Text>();

        moveSpeed = 0.01f;

        FullReset();
    }
	
    void Update()
    {
        ProcesInput();

        // Laat alle pins die de speler niet geselecteerd heeft terug omlaag bewegen:
        for (int i = 0; i < mgr.NrOfPins; i++)
        {
            Pin pin = mgr.pins [i].GetComponent<Pin>();
            pin.isMoving = i == selectedIndex;
        }
    }

    void ProcesInput()
    {
        // Pin selecteren:
        if (Input.GetButtonDown("Fire1"))
        {
            startingIndex = selectedIndex;
            ChangeIndex(-1);
        } else if (Input.GetButtonDown("Fire2"))
        {
            startingIndex = selectedIndex;
            ChangeIndex(1);
        }
        
        // Pin omhoog bewegen:
        if (Input.GetButton("Fire3"))
        {
            MovePin();
        } 

        // Stop het geluid als de knop losgelaten wordt:
        if (Input.GetButtonUp("Fire3"))
        {
            StopSound();
        } 

        // Speel het geluid van de pin af:
        if (Input.GetButtonUp("Fire4"))
        {
            FeelPin();
        }

        // Probeer de pin vast te zetten:
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool set = mgr.pins [selectedIndex].GetComponent<Pin>().LockPin(nextIndex);
            if (set)
            {
                nextIndex++;
                PlayOneShot("click"); // Ander geluid
            } else
            {
                LoseLife();
                PlayOneShot("break");
            }
        }
    }

    // Beweeg de geselecteerde pin omhoog:
    void MovePin()
    {
        Pin selectedPin = mgr.pins [selectedIndex].GetComponent<Pin>();
        if (!selectedPin.GetComponent<Pin>().isSet)
        {
            Vector3 newPos = selectedPin.transform.position;
            newPos += new Vector3(0, moveSpeed, 0);
            newPos.y = Mathf.Clamp(newPos.y, 0, selectedPin.maxHeight);
            selectedPin.transform.position = newPos;

            if (selectedPin.transform.position.y != selectedPin.maxHeight)
            {
                PlaySound("tinkering");
            }
        } 
    }

    // Selecteer een andere pin:
    void ChangeIndex(int amount)
    {
        selectedIndex += amount;

        if (selectedIndex < 0 || selectedIndex >= mgr.NrOfPins)
        {
            selectedIndex = Mathf.Clamp(selectedIndex, 0, mgr.NrOfPins - 1);

            if (!isResetting)
            {
                if (mgr.pins [selectedIndex].GetComponent<Pin>().isSet)
                {
                    ChangeIndex(-amount);
                    return;
                }
            }

            /*if (!isResetting)
            {
                PlayOneShot("edge", 0.4f);
            }*/
        } else
        {
            if (mgr.pins [selectedIndex].GetComponent<Pin>().isSet)
            {
                ChangeIndex(amount);
                return;
            }
            if (amount != 0 && startingIndex != selectedIndex)
            {
                //FeelPin();
                PlayOneShot("changeindex", 0.05f);
            }
        }

        if (startingIndex == selectedIndex && !isResetting)
        {
            PlayOneShot("edge", 0.4f);
        }

        transform.position = new Vector3((1 - mgr.NrOfPins) + selectedIndex, -1.5f, 0);
    }

    // Speel het geluid van de geselecteerd pin af:
    void FeelPin()
    {
        float pitch = ((float)(mgr.pins [selectedIndex].GetComponent<Pin>().Index + 1) / mgr.NrOfPins) + 0.5f;
        PlayOneShot("movepick", 1, pitch);
    }

    // Geef de speler punten:
    public void IncreaseScore(int nrOfPins)
    {
        score += nrOfPins * 5;
        scoreText.text = "Score: " + score;
    }

    // Verlies levens. Wanneer je geen levens meer hebt, hard reset:
    public void LoseLife()
    {
        lives--;
        livesText.text = "Lives: " + lives;

        if (lives <= 0)
        {
            mgr.PlaySound("Fail");
            mgr.FullReset();
            FullReset();
        }
    }

    public void FullReset()
    {
        score = 0;
        scoreText.text = "Score: " + score;
        lives = 5;
        livesText.text = "Lives: " + lives;

        transform.localScale = new Vector3(5, 1, 1);

        Reset();
    }

    // Reset naar begin puzzel:
    public void Reset()
    {
        isResetting = true;

        selectedIndex = 0;
        ChangeIndex(0);
        nextIndex = 0;

        isResetting = false;
    }

    public int SelectedIndex
    {
        get { return selectedIndex;}
    }

    public int NextIndex
    {
        get { return nextIndex;}
    }
}
