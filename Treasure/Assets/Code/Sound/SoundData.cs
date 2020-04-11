using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundPair
{
    public string soundName;
    public AudioClip clip;
}

public class SoundData : ScriptableObject
{
    public List<SoundPair> audioClips;
}