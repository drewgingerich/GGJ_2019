using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{

    [SerializeField]
    List<NestItem> ItemsInNest = new List<NestItem>();

    [SerializeField]
    HotBird hotBird;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnItemAdded(NestItem item) {
        ItemsInNest.Add(item);
        Debug.Log(hotBird == null);
        Debug.Log(item.Category);
        if (hotBird.DesiredItems.Contains(item.Category)) {
            StartCoroutine(hotBird.Praise());
        } else {
            StartCoroutine(hotBird.Scoff());
        }
    }
}
