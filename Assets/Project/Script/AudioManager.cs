using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioSource SE = null;
    [SerializeField] public AudioSource BGM = null;
    [SerializeField] public AudioSource SE2 = null;
    static AudioSource SE_smash = null;
    static AudioSource SE_audio = null;
    static AudioSource BGM_audio = null;
    public float bgmVol;
    static Sequence seq;
    //[SerializeField] AudioClip audioclip = null;
    // Start is called before the first frame update
    void Start()
    {
        SE_audio = SE;
        BGM_audio = BGM;
        SE_smash = SE2;
        //DOTween.To(() => BGM_audio.volume, (x) => BGM_audio.volume = x, 0f, 0.5f);

    }
    public static void SE_Play(AudioClip clip)
    {
        SE_audio.clip = clip;
        SE_audio.Play();
    }
    public static void SE2_Play(AudioClip clip)
    {
        SE_smash.clip = clip;
        SE_smash.Play();
    }
    public static void BGM_Play(AudioClip clip)
    {
        //if (seq == null)
        //{
        //    var tweenFadeOut = DOTween.To(() => BGM_audio.volume, x => BGM_audio.volume = x, 0f, 3f);
        //    var tweenFadeIn = DOTween.To(() => BGM_audio.volume, x => BGM_audio.volume = x, 0.5f, 3f).OnStart(() => changeBGM(clip));
        //    seq = DOTween.Sequence().Append(tweenFadeOut).Append(tweenFadeIn);
        //}
        //else
        if (seq!=null&&DOTween.IsTweening(seq))
        {
            // Š®—¹‚µ‚Ä‚¢‚È‚¢‚½‚ßRestart‚·‚é
            seq.Restart();
            Debug.Log("restart");
        }
        else
        {
            var tweenFadeOut = DOTween.To(() => BGM_audio.volume, x => BGM_audio.volume = x, 0f, 1f);
            var tweenFadeIn = DOTween.To(() => BGM_audio.volume, x => BGM_audio.volume = x, 0.5f, 1f).OnStart(() => changeBGM(clip));
            seq = DOTween.Sequence().Append(tweenFadeOut).Append(tweenFadeIn);
        }


        //seq.Append(tweenFadeOut);
        //seq.Append(tweenFadeIn);
        Debug.Log("playBGM");
    }
    private static void changeBGM(AudioClip clip)
    {
        BGM_audio.clip = clip;
        BGM_audio.Play();
    }
}
