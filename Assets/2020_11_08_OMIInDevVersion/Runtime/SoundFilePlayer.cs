using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFilePlayer : MonoBehaviour
{
    public Queue<ExecutableFile> m_nextToPlay= new Queue<ExecutableFile>();

    public AudioSource m_audioSource;

    public void PlayOneShot(ExecutableFile file) {
        m_nextToPlay.Enqueue(file);
    }
    
    private void Update()
    {
        if (m_nextToPlay.Count > 0)
        {
            ExecutableFile e = m_nextToPlay.Dequeue();
            StartCoroutine(loadFile(e.GetAbsolutePath(), e.GetFileName()));

        }
    }

    IEnumerator loadFile(string path, string name)
    {
        WWW www = new WWW("file://" + path);

        AudioClip myAudioClip = www.GetAudioClip();
       // while (myAudioClip.isReadyToPlay)
            yield return www;
        myAudioClip.name = name;

         AudioClip clip = www.GetAudioClip(false, false);
          clip.name = name;
        m_audioSource.clip = clip;
        m_audioSource.Stop();
        m_audioSource.Play();
        //m_audioSource.PlayOneShot(clip);
    }

    public void Play(ExecutableFile sound)
    {

        PlayOneShot(sound);
        m_audioSource.Play();
       // sound:play:Chopin
    }

    public void Pause(ExecutableFile sound)
    {
        m_audioSource.Pause();
    }

    public void Stop(ExecutableFile sound)
    {
        m_audioSource.Stop();
    }

    internal void Play()
    {
        m_audioSource.Play();
    }

    internal void Pause()
    {
        m_audioSource.Pause();
    }

    internal void SwitchPause()
    {
        if (m_audioSource.isPlaying)
            m_audioSource.Pause();
        else
            m_audioSource.Play();
    }

    internal void Stop()
    {
        m_audioSource.Stop();
    }
}
