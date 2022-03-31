using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioSource SE = null;
    [SerializeField] public AudioSource BGM = null;
    static AudioSource SE_audio = null;
    static AudioSource BGM_audio = null;
    //[SerializeField] AudioClip audioclip = null;
    // Start is called before the first frame update
    void Start()
    {
        SE_audio = SE;
    }
    public static void SE_Play(AudioClip clip)
    {
        SE_audio.clip = clip;
        SE_audio.Play();
    }
    public static void BGM_Play(AudioClip clip)
    {
        BGM_audio.clip = clip;
        SE_audio.Play();
    }
}
