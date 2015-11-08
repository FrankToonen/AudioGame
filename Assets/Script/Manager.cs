using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : SoundGameObject
{
    public GameObject pinPrefab;
    public GameObject[] pins;
    public Pick player;
    GameObject frame, backplate;
    List<int> indices;
    int nrOfPins;

    protected override void Start()
    {
        base.Start();

        frame = GameObject.Find("Frame");
        backplate = GameObject.Find("Backplate");
        player = GameObject.FindWithTag("Player").GetComponent<Pick>();
        FullReset();
    }

    void Update()
    {
        // Controleer of de speler alle pins gelocked heeft:
        if (CheckWin())
        {
            PlayOneShot("succes");
            player.IncreaseScore(nrOfPins);
            player.transform.localScale += new Vector3(2, 0, 0);
            nrOfPins++;

            frame.transform.localScale += new Vector3(0, 1, 0);
            backplate.transform.localScale += new Vector3(2, 0, 0);

            Reset();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Color c = frame.GetComponent<Renderer>().material.color;
            c.a = c.a == 1 ? 0.1f : 1;
            frame.GetComponent<Renderer>().material.color = c;
        }
    }

    // Loop over alle pins en return een boolean:
    bool CheckWin()
    {
        bool won = true;
        foreach (GameObject obj in pins)
        {
            won = won && obj.GetComponent<Pin>().isSet;
        }
        return won;
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

    // Haal een random waarde uit de lijst:
    int GetRandomIndex()
    {
        int r = Random.Range(0, indices.Count - 1);
        int i = indices [r];
        indices.RemoveAt(r);
        return i;
    }


    // Vergelijk de indices van de geselecteerde pin en welke index de speler moet doen:
    public bool MatchIndices()
    {
        return player.NextIndex == pins [player.SelectedIndex].GetComponent<Pin>().Index;
    }

    // Zet alles terug naar de begintoestand:
    public void FullReset()
    {
        nrOfPins = 3;
        frame.transform.localScale = new Vector3(5, nrOfPins - 1, 5);
        backplate.transform.localScale = new Vector3(nrOfPins + 1, 5, 3);
        Reset();
    }

    // Genereer de set pins opnieuw:
    public void Reset()
    {
        GeneratePins();
        player.Reset();
    }

    public int NrOfPins
    {
        get { return nrOfPins; }
    }
}
