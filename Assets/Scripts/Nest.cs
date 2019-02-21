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
        if (item == null) {
            return;
        }

        BirdController.activeController.DropItem();
		item.transform.SetParent(transform);
        item.transform.position = ornamentPoints[oranmentPointIndex].position;

        if (neededItems.Contains(item)) {
			oranmentPointIndex++;
            neededItems.Remove(item);
            item.SwitchToLongInstrument();
            if (neededItems.Count == 0) {
                cutscenePlayer.PlayEndingCutscene();
            } else {
                cutscenePlayer.PlayGoodItemCutscene();
            }
        } else {
            cutscenePlayer.PlayBadItemCutscene(item);
        }
        item.StopAllCoroutines();
    }
}
