using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class SoundGameObject : MonoBehaviour
{
    protected float lengthSoundPlaying;
    AudioClip clip;
    protected AudioSource source;

    protected virtual void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(string assetName, float volume = 1, float pitch = 1)
    {
        if (!source.isPlaying)
        {
            clip = Resources.Load<AudioClip>("Sounds/" + assetName);
            source.clip = clip;
            source.pitch = pitch;
            source.Play();
        }
    }

    public void PlayOneShot(string assetName, float volume = 1, float pitch = 1)
    {
        clip = Resources.Load<AudioClip>("Sounds/" + assetName);
        source.pitch = pitch;
        source.volume = volume;
        source.PlayOneShot(clip);
    }

    public void PlayRandom(string assetName, int amountOfSamples, float volume = 1)
    {
        int r = Random.Range(1, amountOfSamples + 1);
        string s = r < 10 ? "0" + r : r.ToString();
        clip = Resources.Load<AudioClip>("Sounds/" + assetName + "/" + assetName + s);
        source.volume = volume;

        lengthSoundPlaying = clip.length;

        source.PlayOneShot(clip);


    }

    public void StopSound()
    {
        source.Stop();
    }
}
