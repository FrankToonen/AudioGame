using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : SoundGameObject
{
    public GameObject pinPrefab;
    public GameObject[] pins;
    public Pick player;
    List<int> indices;
    int amountOfPins;

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindWithTag("Player").GetComponent<Pick>();

        amountOfPins = 5;
        indices = new List<int>();
        for (int i = 0; i < amountOfPins; i++)
            indices.Add(i);
        GeneratePins();
    }
	
    void GeneratePins()
    {
        pins = new GameObject[amountOfPins];
        for (int i = 0; i < amountOfPins; i++)
        {
            GameObject newPin = Instantiate(pinPrefab, new Vector3(i, 0, 0), Quaternion.identity) as GameObject;
            newPin.GetComponent<Pin>().Initialize(GetRandomIndex());
            newPin.name = "Pin" + i;

            pins [i] = newPin;
        }
    }

    int GetRandomIndex()
    {
        int r = Random.Range(0, indices.Count - 1);
        int i = indices [r];
        indices.RemoveAt(r);
        return i;
    }

    public bool MatchIndices()
    {
        return player.NextIndex == pins [player.SelectedIndex].GetComponent<Pin>().Index;
    }

    public void Reset()
    {
        foreach (GameObject obj in pins)
        {
            obj.GetComponent<Pin>().Reset();
        }
    }

    public int AmountOfPins
    {
        get { return amountOfPins; }
    }
}
