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

    [SerializeField]
    CutscenePlayer cutscenePlayer;

    void OnTriggerEnter2D(Collider2D other) {
        NestItem item = other.GetComponent<NestItem>();
        if (item == null || item.nestCooldown) {
            return;
        }

        BirdController.activeController.carriedItem = null;
        item.Fall();
		item.Land();
        item.nestCooldown = true;
        item.transform.position = ornamentPoints[oranmentPointIndex].position;

        if (neededItems.Contains(item)) {
			oranmentPointIndex++;
            neededItems.Remove(item);
            item.Nest();
            if (neededItems.Count == 0) {
                cutscenePlayer.PlayEndingCutscene();
            } else {
                cutscenePlayer.PlayGoodItemCutscene();
            }
        } else {
            cutscenePlayer.PlayBadItemCutscene(item);
			StartCoroutine(DropItemWithDelayRoutine(item));
        }
        item.StopAllCoroutines();
    }

    IEnumerator DropItemWithDelayRoutine(NestItem badItem) {
        yield return new WaitForSeconds(0.5f);
        badItem.Fall();
    }
}
