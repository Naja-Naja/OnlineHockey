using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioSource SE = null;
    static AudioSource SE_audio = null;
    //[SerializeField] AudioClip audioclip = null;
    // Start is called before the first frame update
    void Start()
    {
        SE_audio = SE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void SE_Play(AudioClip clip)
    {
        SE_audio.clip = clip;
        SE_audio.Play();
    }
}
