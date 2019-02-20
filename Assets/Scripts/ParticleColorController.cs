using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColorController : MonoBehaviour
{
    [SerializeField]
    ParticleSystem.MinMaxGradient color;
    [SerializeField]
    ParticleSystem.MinMaxGradient selectedColor;
    [SerializeField]
    ParticleSystem.MinMaxGradient interactingColor;
    [SerializeField]
    new ParticleSystem particleSystem;
    
    ParticleSystem.MainModule mainModule;

    void Start()
    {
        mainModule = particleSystem.main;
        mainModule.startColor = color;
    }

    public void Select() {
        mainModule.startColor = selectedColor;
    }

    public void Deselect() {
        mainModule.startColor = color;
    }

    public void Interact() {
        mainModule.startColor = interactingColor;
    }
}
