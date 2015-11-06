using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    public GameObject lockPrefab;
    public GameObject[] locks;
    public List<int> indices;

    void Start()
    {
        indices = new List<int>() { 0,1,2,3,4};
        GenerateLocks();
    }
	
    void Update()
    {
	
    }

    void GenerateLocks()
    {
        int amountOfLocks = 5;
        locks = new GameObject[amountOfLocks];
        for (int i = 0; i < amountOfLocks; i++)
        {
            GameObject newLock = Instantiate(lockPrefab, new Vector3(i, 0, 0), Quaternion.identity) as GameObject;
            newLock.GetComponent<Lock>().Initialize(GetRandomIndex());
            newLock.name = "Lock" + i;


            locks [i] = newLock;
        }

    }

    int GetRandomIndex()
    {
        int r = Random.Range(0, indices.Count - 1);
        int i = indices [r];
        indices.RemoveAt(r);
        return i;
    }

    public void Reset()
    {
        foreach (GameObject obj in locks)
        {
            obj.GetComponent<Lock>().Reset();
        }
    }
}
