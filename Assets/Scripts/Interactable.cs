using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public static List<Interactable> activeItems;

    public UnityEvent OnInteract;
    public UnityEvent OnSelect;
    public UnityEvent OnDeselect;

    [SerializeField]
    bool active = true;

    public void Interact() {
        OnInteract.Invoke();
    }

    public void Select() {
        OnSelect.Invoke();
    }

    public void Deselect() {
        OnDeselect.Invoke();
    }

    public void SetActive(bool value) {
        if (value == active) {
            return;
        }
        if (value) {
            Activate();
        } else {
            Deactivate();
        }
        active = value;
    }

    void Activate() {
        if (activeItems == null) {
            activeItems = new List<Interactable>();
        }
        activeItems.Add(this);
    }

    void Deactivate() {
        activeItems.Remove(this);
    }

    void Awake() {
        if (active) {
            Activate();
        }
    }
}
