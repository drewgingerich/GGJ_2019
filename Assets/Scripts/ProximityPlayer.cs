using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityPlayer : MonoBehaviour
{
    const float minFullSoundDistance = 2;
    const float minSoundDistance = 10;
    const float minSize = 0.2f;
    const float maxSize =  1.2f;

    [SerializeField]
    Transform reference;
    [SerializeField]
    Instrument instrument;
    [SerializeField]
    new ParticleSystem particleSystem;

    float sizeRangeWidth;
    ParticleSystem.MainModule particlesMain;

    void Start() {
        particlesMain = particleSystem.main;
        sizeRangeWidth = maxSize - minSize;
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

        particlesMain.startSize = newVolume * sizeRangeWidth + minSize;

        // if (newVolume == 0 && particlesMain.loop) {
        //     particlesMain.loop = false;
        // } else if (newVolume > 0 && !particlesMain.loop) {
        //     particlesMain.loop = false;
        //     particleSystem.Play();
        // }
    }

    void OnDisable() {
        particleSystem.Stop();
        instrument.SetVolume(0);
    }
}
