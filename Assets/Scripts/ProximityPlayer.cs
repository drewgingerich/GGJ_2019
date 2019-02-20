using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityPlayer : MonoBehaviour
{
    const float minFullSoundDistance = 2;
    const float minSoundDistance = 10;

    [SerializeField]
    Transform reference;
    [SerializeField]
    Instrument instrument;
    [SerializeField]
    new ParticleSystem particleSystem;

    ParticleSystem.MainModule particlesMain;

    void Start() {
        particlesMain = particleSystem.main;
    }

    void Update()
    {
        float newVolume;
		float distance = (transform.position - reference.position).magnitude;
		if (distance < minFullSoundDistance) {
            newVolume = 1;
		} else if (distance < minSoundDistance) {
			float adjustedDistance = (distance - minFullSoundDistance) / minSoundDistance;
            newVolume = 1 - Mathf.Sqrt(adjustedDistance);
            // Other implementations of falloff:
			// instrument.SetVolume(1 - Mathf.Pow(adjustedDistance, 0.3f));
			// instrument.SetVolume(1 - Mathf.Log10(adjustedDistance));
		} else {
            newVolume = 0;
		}
        instrument.SetVolume(newVolume);

        if (newVolume == 0 && particlesMain.loop) {
            particlesMain.loop = false;
        } else if (newVolume > 0 && !particlesMain.loop) {
            particlesMain.loop = false;
            particleSystem.Play();
        }
    }
}
