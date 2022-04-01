using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashEffectManager : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystemComponent;
    public void PlaySmashEffect(Vector3 pos)
    {
        this.transform.position = pos;
        particleSystemComponent.Play();
    }
    private void OnParticleSystemStopped()
    {
        Debug.Log("パーティクル終わったよ！");
    }
}
