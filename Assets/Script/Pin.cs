using UnityEngine;
using System.Collections;

public class Pin : MonoBehaviour
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

    void Start()
    {
	
    }
	
    void Update()
    {
        if (transform.position.y > targetHeight)
        {
            mgr.PlaySound("Fail");
            mgr.Reset();
        } else if (Mathf.Abs(transform.position.y - targetHeight) < 0.2 && !isSet)
        {
            // Rumble controller
            // Play sounds?
        }

        if (!isSet && !isMoving)
        {
            float newY = Mathf.Clamp(transform.position.y - 0.05f, 0, targetHeight);
            transform.position = new Vector3(transform.position.x, newY, 0);
            mgr.PlaySound("MovePinDown");
        }
    }

    void GenerateHeight()
    {
        targetHeight = Random.value * 2;
    }

    public void Reset()
    {
        transform.position = new Vector3(transform.position.x, 0, 0);
        GenerateHeight();
    }

    public bool LockPin(int i)
    {
        if (Mathf.Abs(transform.position.y - targetHeight) < 0.2 && index == i)
        {
            isSet = true;
            return true;
        }

        return false;
    }
}
