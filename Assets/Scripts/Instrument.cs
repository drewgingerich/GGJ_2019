using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instrument : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    float actualVolume;
    bool volumeOverride;

    bool FullVolume {
        set {
            actualVolume = audioSource.volume;
            audioSource.volume = 1;
            volumeOverride = true;
        }    
    }

    bool Mute {
        set { audioSource.mute = value; }
    }

    public void SetVolume(float volume) {
        if (volumeOverride) {
            return;
        }
        volume = Mathf.Clamp(volume, 0, 1);
        audioSource.volume = volume;
    }
}
