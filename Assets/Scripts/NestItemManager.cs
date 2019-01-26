using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestItemManager : MonoBehaviour
{

    [SerializeField]
    List<NestItem> nestItemPrefabs;

    [SerializeField]
    List<Transform> spawnPoints;

    public void SpawnNestItems(int numberToSpawn) {
        List<NestItem> availableItems = new List<NestItem>(nestItemPrefabs);
        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        for (int i = 0; i < numberToSpawn; i++) {
            int index = GetRandomIndex(availableItems.Count);
            NestItem nestItem = Instantiate(availableItems[index], transform);
            NestItem.ActiveItems.Add(nestItem);
            availableItems.RemoveAt(index);

            index = GetRandomIndex(availableSpawnPoints.Count);
            nestItem.transform.position = availableSpawnPoints[index].position;
            availableSpawnPoints.RemoveAt(index);
        }
    }

    public void RemoveItem(NestItem item) {
        NestItem.ActiveItems.Remove(item);
    }

    public void CleanNestItems() {
        foreach (NestItem item in NestItem.ActiveItems) {
            Destroy(item.gameObject);
        }
    }

    int GetRandomIndex(int collectionCount) {
        return Random.Range(0, collectionCount - 1);
    }
}
