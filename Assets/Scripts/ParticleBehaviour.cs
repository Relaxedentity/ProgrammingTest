using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehaviour : MonoBehaviour
{
    public float duration;
    public AudioSource audio;
    public AudioClip death_effect;
    void Start()
    {
        audio.PlayOneShot(death_effect, 0.2f);
    }
    void Update()
    {
        duration-=Time.deltaTime;
        if(duration <= 0)
        {
            GameObject.Destroy(gameObject);
        }
    }

}
