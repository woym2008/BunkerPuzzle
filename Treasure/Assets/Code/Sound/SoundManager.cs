using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bunker.Module;

/* 因为是2D游戏，简单写一个 */
public class SoundManager : ServicesModule<SoundManager>
{
    const int SOUNDPLAYER_NUM = 3;
    const int BGM_IDX = 0;
    const int GAME_SFX_IDX = 1;
    const int UI_SFX_IDX = 2;

    List<AudioSource> soundPlayers;
    Dictionary<string, AudioClip> soundsDict;
    GameObject _gameObject;
    public void Init()
    {
        soundsDict = new Dictionary<string, AudioClip>();
        var sd = Resources.Load<SoundData>("Sounds/SoundData");
        foreach (var sp in sd.audioClips)
        {
            soundsDict.Add(sp.soundName, sp.clip);
        }

        soundPlayers = new List<AudioSource>(SOUNDPLAYER_NUM);
        _gameObject = MonoBehaviourHelper.CreateObject("SoundManager").gameObject;
        for (int i = 0;i< SOUNDPLAYER_NUM;++i)
        {
            soundPlayers.Add(_gameObject.AddComponent<AudioSource>());
        }
    }

    public void SetVolume(float v)
    {
        foreach (var sp in soundPlayers)
        {
            sp.volume = v;
        }
    }

    public void SetMute(bool isMute)
    {
        foreach (var sp in soundPlayers)
        {
            sp.mute = isMute;
        }
    }

    public void PlayBGM(string bgm_name,bool is_loop = true)
    {
        soundPlayers[BGM_IDX].clip = soundsDict[bgm_name];
        soundPlayers[BGM_IDX].loop = is_loop;
        soundPlayers[BGM_IDX].Play();
    }

    public void StopBGM()
    {
        soundPlayers[BGM_IDX].Stop();
    }

    public void PlaySound(string sound_name)
    {
        soundPlayers[GAME_SFX_IDX].PlayOneShot(soundsDict[sound_name]);
    }

    public void PlayUISound(string sound_name)
    {
        soundPlayers[UI_SFX_IDX].PlayOneShot(soundsDict[sound_name]);
    }
}