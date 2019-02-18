using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    public float speed = 1f;

    private bool done;

    private float progress;

    [SerializeField]
    private Image image;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Transition() {
        Debug.Log("transitioning!");
        done = false;
        progress = 0f;
        while (progress < 1 ) {
            progress += Time.deltaTime * (1f /speed);
            image.fillAmount = progress;
            yield return null;
        }
        done = true;
    }
}
