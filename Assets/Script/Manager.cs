using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    public GameObject pinPrefab;
    public GameObject[] locks;
    List<int> indices;
    AudioClip clip;
    AudioSource source;
    int amountOfPins;

    void Start()
    {
        source = GetComponent<AudioSource>();

        amountOfPins = 5;

        indices = new List<int>();
        for (int i = 0; i < amountOfPins; i++)
            indices.Add(i);

        GeneratePins();
    }
	
    void Update()
    {
	
    }

    void GeneratePins()
    {
        locks = new GameObject[amountOfPins];
        for (int i = 0; i < amountOfPins; i++)
        {
            GameObject newPin = Instantiate(pinPrefab, new Vector3(i, 0, 0), Quaternion.identity) as GameObject;
            newPin.GetComponent<Pin>().Initialize(GetRandomIndex());
            newPin.name = "Pin" + i;

            locks [i] = newPin;
        }

    }

    int GetRandomIndex()
    {
        int r = Random.Range(0, indices.Count - 1);
        int i = indices [r];
        indices.RemoveAt(r);
        return i;
    }

    public void PlaySound(string assetName, int pitch = 1)
    {
        clip = Resources.Load<AudioClip>("Sounds/" + assetName);
        source.pitch = pitch;
        source.PlayOneShot(clip);
    }

    public void Reset()
    {
        foreach (GameObject obj in locks)
        {
            obj.GetComponent<Pin>().Reset();
        }
    }
}
