using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.priority = s.priority;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void PlayDelayed(string name, float delay = 0){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.volume = s.volume;
        s.source.PlayDelayed(delay);
    }

    public void StopAllExcept(string[] names = null, float duration = 0){
        if(names == null) names = new string[]{""};
        foreach(Sound s in sounds){
            if(!names.Contains(s.name)){
                if(s.source.isPlaying){
                    StartCoroutine(LerpFunction(s.source,0,duration));
                }
            }
        }
    }

    public void Stop(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public float GetClipLength(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.source.clip.length;
    }

    public void IncreaseVolume(string name, float endVolume, float duration){
        Sound s = Array.Find(sounds, sound => sound.name == name);
            StartCoroutine(LerpVolume(s.source,endVolume,duration));
    }
    
    IEnumerator LerpFunction(AudioSource source,float endValue, float duration)
    {
        float time = 0;
        float startValue = source.volume;

        while (time < duration)
        {
            source.volume = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        source.volume = endValue;
        source.Stop();
    }
    IEnumerator LerpVolume(AudioSource source, float endValue, float duration)
    {
        float time = 0;
        float startValue = source.volume;

        while (time < duration)
        {
            source.volume = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        source.volume = endValue;
    }
}
