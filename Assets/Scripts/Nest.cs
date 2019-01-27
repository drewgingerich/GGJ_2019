using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Nest : MonoBehaviour
{

    [SerializeField]
    List<NestItem> neededItems;

    [SerializeField]
    List<Transform> ornamentPoints;
    int oranmentPointIndex = 0;

    [SerializeField]
    HotBird hotBird;

    void OnTriggerEnter2D(Collider2D other) {
        NestItem item = other.GetComponent<NestItem>();
        if (item == null) {
            return;
        }

        BirdController.activeController.DropItem();
		item.transform.SetParent(transform);
        item.transform.position = ornamentPoints[oranmentPointIndex].position;

        if (neededItems.Contains(item)) {
			oranmentPointIndex++;
            neededItems.Remove(item);
            NestItem.ActiveItems.Remove(item);
            if (neededItems.Count == 0) {
                StartCoroutine(WinGameRoutine());
            } else {
                StartCoroutine(GoodItemRoutine());
            }
        } else {
            StartCoroutine(BadItemRoutine(item));
        }
		item.Rest();
    }

    IEnumerator GoodItemRoutine() {
        yield return null;
    }

    IEnumerator BadItemRoutine(NestItem item) {
        hotBird.anim.SetBool("isTalking", true);
        hotBird.anim.SetTrigger("Angered");
        Debug.Log("BOOOOO");
        yield return new WaitForSeconds(0.5f);
		item.transform.SetParent(null);
        item.Fall();
        hotBird.anim.SetBool("isTalking", false);
        yield return null;
    }

    IEnumerator WinGameRoutine() {
        Debug.Log("You win!");
        yield return null;
    }
}
