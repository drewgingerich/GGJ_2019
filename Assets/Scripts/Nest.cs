using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Nest : MonoBehaviour
{

    [SerializeField]
    List<NestItem> ItemsInNest;

    [SerializeField]
    HotBird hotBird;

    // Start is called before the first frame update
    void Start()
    {
        ItemsInNest = new List<NestItem>();
        Debug.Log("Items in Nest:" + ItemsInNest.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void OnItemAdded(NestItem item) {
    //     ItemsInNest.Add(item);
    //     var categoryList = ItemsInNest.Select(i => i.Category);

    //     foreach (var c in categoryList) {
    //         Debug.Log(c.ToString());
    //     }
    //     Debug.Log(categoryList.Except(hotBird.DesiredItems).Count());
    //     Debug.Log(hotBird.DesiredItems.Except(categoryList).Count());

    //     if (categoryList.Except(hotBird.DesiredItems).Count() == 0 && hotBird.DesiredItems.Except(categoryList).Count() == 0) {
    //         WinGame();
    //     }
    //     else if (hotBird.DesiredItems.Contains(item.Category)) {
    //         StartCoroutine(hotBird.Praise());
    //     } else {
    //         StartCoroutine(hotBird.Scoff());
    //     }
    // }

    public void WinGame() {
        Debug.Log("What a perfect nest! You win!");
    }
}
