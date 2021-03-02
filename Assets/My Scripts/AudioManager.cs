using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource _audioSource;

    [SerializeField] private AudioClip[] _clips;

    private void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int index)
    {
        if(!_audioSource.isPlaying)
            _audioSource.PlayOneShot(_clips[index]);
    }
}
