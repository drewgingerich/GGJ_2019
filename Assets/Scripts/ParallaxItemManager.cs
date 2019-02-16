using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxItemManager : MonoBehaviour
{
    [SerializeField]
    ParallaxDriver parallaxDriver;
    [SerializeField]
    List<Transform> parallaxItems;

    void Start() {
        foreach (Transform item in parallaxItems) {
            parallaxDriver.AddParallaxItem(item);
        }
    }
}
