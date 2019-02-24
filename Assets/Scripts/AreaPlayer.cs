using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaPlayer : MonoBehaviour
{
    [SerializeField]
    Instrument instrument;
    [SerializeField]
    BirdController birdController;

    void Update() {
        if (birdController.foraging) {
            instrument.SetVolume(0);
        } else {
            instrument.SetVolume(1);
        }

    }
}
