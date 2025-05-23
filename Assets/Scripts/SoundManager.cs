using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private List<AudioClip> audioClipList;
    [HideInInspector] public Dictionary<SoundType, List<SoundPlayer>> soundPlayerDic;

    public AudioMixer AudioMixer { get { return audioMixer; } }

    protected override void Awake()
    {
        base.Awake();
        soundPlayerDic = new Dictionary<SoundType, List<SoundPlayer>>();
    }
    /// <summary>
    /// 음악 볼륨 설정
    /// </summary>
    /// <param name="type"></param>
    /// <param name="volume"></param>
    public void SetVolume(SoundType type, float volume)
    {
        audioMixer.SetFloat(type.ToString(), Mathf.Log10(volume) * 20);
    }
    /// <summary>
    /// 음악 실행
    /// </summary>
    /// <param name="type">사운드 타입</param>
    /// <param name="name">사운드 클립 명</param>
    /// <param name="isLoop">반복 여부</param>
    public void PlaySound(SoundType type, string name, bool isLoop = false)
    {
        if (type == SoundType.BGM && soundPlayerDic.ContainsKey(type) && soundPlayerDic[type].Count >= 1)
        {
            soundPlayerDic[type][0].Stop();
        }
        //Todo : 추후 오브젝트 풀로 관리
        GameObject go = new GameObject();
        go.transform.parent = transform;
        SoundPlayer sp = go.AddComponent<SoundPlayer>();
        AudioMixerGroup mixerGroup = audioMixer.FindMatchingGroups(type.ToString())[0];
        AudioClip clip = audioClipList.Find(x => x.name == name);

        sp.Setting(mixerGroup, clip, isLoop);
        sp.Play();

        if (soundPlayerDic.ContainsKey(type))
        {
            soundPlayerDic[type].Add(sp);
        }
        else
        {
            soundPlayerDic.Add(type, new List<SoundPlayer> { sp });
        }
    }
}
