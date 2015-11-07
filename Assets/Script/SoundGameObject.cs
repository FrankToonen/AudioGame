using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class SoundGameObject : MonoBehaviour
{
    AudioClip clip;
    AudioSource source;

    protected virtual void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(string assetName, float pitch = 1)
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

    public void StopSound()
    {
        source.Stop();
    }
}
