using UnityEngine;
using System.Collections;

public class Lock : MonoBehaviour
{
    Manager mgr;
    public float targetHeight;
    int index;
    public bool isSet, isMoving;

    public void Initialize(int index)
    {
        mgr = GameObject.Find("Manager").GetComponent<Manager>();
        this.index = index;
        isSet = false;
        GenerateHeight();
    }


    // Use this for initialization
    void Start()
    {
	
    }
	
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < targetHeight)
        {
            //Debug.Log("Loser");
            mgr.Reset();
        } else if (Mathf.Abs(transform.position.y - targetHeight) < 0.2 && !isSet)
        {
            //Debug.Log("Winner");
        }

        if (!isSet && !isMoving)
        {
            float newY = Mathf.Clamp(transform.position.y + 0.15f, targetHeight, 0);
            transform.position = new Vector3(transform.position.x, newY, 0);
        }
    }

    void GenerateHeight()
    {
        targetHeight = -Random.value * 5;
    }

    public void Reset()
    {
        transform.position = new Vector3(transform.position.x, 0, 0);
        GenerateHeight();
    }

    public bool LockLock(int i)
    {
        if (Mathf.Abs(transform.position.y - targetHeight) < 0.2 && index == i)
        {
            isSet = true;
            return true;
        }

        return false;
    }
}
