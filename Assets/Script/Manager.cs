using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Manager : SoundGameObject
{
    public GameObject pinPrefab;
    public GameObject[] pins;
    public Pick player;
    //GameObject frame, backplate;
    List<int> indices;
    int nrOfPins;
    float timeLeft, timeMax, timeTickLeft, timeTickMax;
    float countdownTimeLeft, countdownTime;
    bool isPlaying, started;

    protected override void Start()
    {
        base.Start();

        //frame = GameObject.Find("Frame");
        //backplate = GameObject.Find("Backplate");

        player = GameObject.FindWithTag("Player").GetComponent<Pick>();
        PlaySound("intro_controls");
    }

    void Update()
    {
        if (!started)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StopSound();
                FullReset();
                PlayOneShot("start_level1");

                // DEBUG
                //GameObject.Find("StartText").GetComponent<Text>().text = "";
            } else if (Input.GetKeyDown(KeyCode.B))
            {
                StopSound();
                PlaySound("intro_controls");
            }
        } else
        if (isPlaying & started)
        {
            UpdateTimer();

            // Controleer of de speler alle pins gelocked heeft:
            if (CheckWin())
            {
                PlayOneShot("succes");
                PlayOneShot("start_nextlevel");

                player.IncreaseScore(nrOfPins);
                player.transform.localScale += new Vector3(2, 0, 0);
                nrOfPins++;

                // DEBUG
                //frame.transform.localScale += new Vector3(0, 1, 0);
                //backplate.transform.localScale += new Vector3(2, 0, 0);

                Reset();
            }

            if (timeLeft <= 0)
            {
                //
                // Score vertellen etc.
                //

                StopPlaying();
                player.FullReset();

                // DEBUG
                //GameObject.Find("StartText").GetComponent<Text>().text = "Press enter to start";
            }

            // DEBUG
            //if (Input.GetKeyDown(KeyCode.V))
            //{
            //    Color c = frame.GetComponent<Renderer>().material.color;
            //    c.a = c.a == 1 ? 0.1f : 1;
            //    frame.GetComponent<Renderer>().material.color = c;
            //}
        } else if (!isPlaying && started)
        {
            UpdateCountdownTimer();
        }
    }
    	
    // Genereer een set pins:
    void GeneratePins()
    {
        // Verwijder de vorige set:
        foreach (GameObject obj in pins)
        {
            Destroy(obj);
        }

        // Maak een lijst van integers die gebruikt worden om een random volgorde te genereren:
        indices = new List<int>();
        for (int i = 0; i < nrOfPins; i++)
            indices.Add(i);

        // Maak een array van pins:
        pins = new GameObject[nrOfPins];
        for (int i = 0; i < nrOfPins; i++)
        {
            GameObject newPin = Instantiate(pinPrefab, new Vector3(i, 0, 0), Quaternion.identity) as GameObject;
            newPin.GetComponent<Pin>().Initialize(GetRandomIndex());
            newPin.name = "Pin" + i;

            pins [i] = newPin;
        }
    }

    void UpdateCountdownTimer()
    {
        countdownTimeLeft = Mathf.Clamp(countdownTimeLeft - Time.deltaTime, 0, countdownTime);
        isPlaying = countdownTimeLeft <= 0;
        if (isPlaying)
        {
            PlaySound("succes"); // Anders geluid zoeken onder naam 'start'
        }
    }

    void UpdateTimer()
    {
        timeLeft = Mathf.Clamp(timeLeft - Time.deltaTime, 0, timeMax);
        timeTickLeft = Mathf.Clamp(timeTickLeft - Time.deltaTime, 0, timeTickMax);

        if (timeTickLeft <= 0)
        {
            PlayRandom("heartbeat", 10, 1.5f);
            /*if (timeLeft < 15)
            {
                timeTickMax = 1f;
            } else if (timeLeft < 5)
            {
                timeTickMax = 0.5f;
            }*/

            timeTickMax = Mathf.Clamp((timeLeft / timeMax) * 2, 0.7f, 2);
            timeTickLeft = timeTickMax;
        }
    }

    // Haal een random waarde uit de lijst:
    int GetRandomIndex()
    {
        int r = Random.Range(0, indices.Count - 1);
        int i = indices [r];
        indices.RemoveAt(r);

        return i;
    }

    // Loop over alle pins en return een boolean:
    bool CheckWin()
    {
        bool won = timeLeft > 0;
        foreach (GameObject obj in pins)
        {
            won = won && obj.GetComponent<Pin>().isSet;
        }

        return won;
    }

    // Vergelijk de indices van de geselecteerde pin en welke index de speler moet doen:
    public bool MatchIndices()
    {
        return player.NextIndex == pins [player.SelectedIndex].GetComponent<Pin>().Index;
    }

    // Zet alles terug naar de begintoestand:
    public void FullReset()
    {
        started = true;
        nrOfPins = 3;
        timeMax = 60;
        timeTickMax = 2;
        countdownTime = 4;

        // DEBUG
        //frame.transform.localScale = new Vector3(5, nrOfPins - 1, 5);
        //backplate.transform.localScale = new Vector3(nrOfPins + 1, 5, 3);

        Reset();
    }

    // Genereer de set pins opnieuw:
    public void Reset()
    {
        GeneratePins();
        player.Reset();

        isPlaying = false;
        timeLeft = timeMax;
        timeTickLeft = 0;
        countdownTimeLeft = countdownTime;
    }

    public void ResetPins()
    {
        foreach (GameObject pin in pins)
        {
            pin.GetComponent<Pin>().Reset();
        }
    }

    public void StopPlaying()
    {
        started = false;
        isPlaying = false;

        PlayOneShot("fail");
        //PlayOneShot("fail_controls");
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        PlayOneShot("reached_level" + (nrOfPins - 2));

        yield return new WaitForSeconds(3);

        PlayOneShot("fail_controls");

        yield return null;
    }

    public int NrOfPins
    {
        get { return nrOfPins; }
    }

    public bool IsPlaying
    {
        get { return isPlaying; }
    }

    public bool Started
    {
        get { return started; }
    }
}
