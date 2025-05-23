using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioSource AudioSource { get { return audioSource; } }
    /// <summary>
    /// 오디오 세팅
    /// </summary>
    /// <param name="mixerGroup"></param>
    /// <param name="clip"></param>
    /// <param name="isLoop"></param>
    public void Setting(AudioMixerGroup mixerGroup, AudioClip clip, bool isLoop)
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.clip = clip;
        audioSource.loop = isLoop;

    }
    /// <summary>
    /// 오디오 재생
    /// </summary>
    public void Play()
    {
        audioSource.Play();
        if (!audioSource.loop)
            StartCoroutine(DestroyWhenEndSound(audioSource.clip.length));
    }
    /// <summary>
    /// 오디오 멈춤
    /// </summary>
    public void Stop()
    {
        string typeName = audioSource.outputAudioMixerGroup.ToString();
        SoundManager.Instance.soundPlayerDic[(SoundType)Enum.Parse((typeof(SoundType)), typeName)].Remove(this);
        Destroy(this.gameObject);
    }

    IEnumerator DestroyWhenEndSound(float time)
    {
        yield return new WaitForSeconds(time);
        string typeName = audioSource.outputAudioMixerGroup.ToString();
        SoundManager.Instance.soundPlayerDic[(SoundType)Enum.Parse((typeof(SoundType)), typeName)].Remove(this);
        Destroy(this.gameObject);
    }
}
