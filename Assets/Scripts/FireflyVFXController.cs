using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class FireflyVFXController : MonoBehaviour
{
    [SerializeField]
    VisualEffect effect;
    [SerializeField]
    SpriteRenderer background;
    [SerializeField]
    float spawnSize = 1;

    const string SPAWN_AREA_WIDTH = "SpawnAreaWidth";
    const string SPAWN_AREA_HEIGHT = "SpawnAreaWidth";
    const string SPAWN_SIZE = "SpawnSize";

    void Start() {
        float spawnAreaWidth = background.sprite.bounds.max.x * 2;
        float spawnAreaHeight = background.sprite.bounds.max.y * 2;
        effect.SetFloat(SPAWN_AREA_WIDTH, spawnAreaWidth);
        effect.SetFloat(SPAWN_AREA_HEIGHT, spawnAreaHeight);
        effect.SetFloat(SPAWN_SIZE, spawnSize);
    }
}
