using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestItem : MonoBehaviour
{
    [SerializeField]
    LandscapeConstants LandscapeConstants;

    float speed = 1;

    [System.NonSerialized]
    public bool isHeld;

    public static List<NestItem> ActiveItems;

    void Awake(){
        if (ActiveItems == null) {
            ActiveItems = new List<NestItem>();
        }
        ActiveItems.Add(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Fall () {
        while (transform.position.y > LandscapeConstants.GroundThreshhold && !isHeld) {
            Vector3 move = Vector3.down * speed * Time.deltaTime;
            transform.position += move;
            yield return null;
        }
    }
}
