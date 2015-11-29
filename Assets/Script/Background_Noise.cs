using UnityEngine;
using System.Collections;

public class Background_Noise : SoundGameObject
{
    Manager mgr;

    // timeMax / timeMin is de maximale/minimale tijd tussen twee geluiden vanaf begin afspelen
    float timeLeft, timeMin, timeMax;

    protected override void Start()
    {
        base.Start();

        mgr = GameObject.Find("Manager").GetComponent<Manager>();

        timeMin = 3;
        timeMax = 7;
        timeLeft = 0;
    }
	
    void Update()
    {
        if (mgr.IsPlaying)
        {
            timeLeft = Mathf.Clamp(timeLeft - Time.deltaTime, 0, timeMax);
            if (timeLeft <= 0)
            {
                PlayRandom("background", 3, .5f);

                timeLeft = Random.Range(timeMin, timeMax) + lengthSoundPlaying;
            }
        }
    }
}
