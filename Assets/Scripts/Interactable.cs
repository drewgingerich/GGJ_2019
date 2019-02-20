using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public static List<Interactable> interactables;

    public UnityEvent OnInteract;
    public UnityEvent OnSelect;
    public UnityEvent OnDeselect;

    public void Interact() {
        OnInteract.Invoke();
    }

    public void Select() {
        OnSelect.Invoke();
    }

    public void Deselect() {
        OnDeselect.Invoke();
    }

    void OnEnable() {
        if (interactables == null) {
            interactables = new List<Interactable>();
        }
        interactables.Add(this);
    }

    void OnDisable() {
        interactables.Remove(this);
    }
}
