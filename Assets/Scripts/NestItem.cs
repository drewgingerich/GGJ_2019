using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NestItem : MonoBehaviour
{
    [SerializeField]
    LandscapeConstants LandscapeConstants;

    [SerializeField]
    float speed = 1;

    [System.NonSerialized]
    public bool isHeld = false;

    public static List<NestItem> ActiveItems;

    [SerializeField]
    public LandscapeConstants.NestItemCategory Category;

    [SerializeField]
    public NestEvent onNestItemAdded;

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
        while (transform.position.y > LandscapeConstants.GroundThreshhold && !isHeld && !IsAtNest()) {
            Vector3 move = Vector3.down * speed * Time.deltaTime;
            transform.position += move;
            yield return null;
        }
        if (IsAtNest()) {
            Debug.Log("invoke!");
            onNestItemAdded.Invoke(this);
            yield return null;
        }
    }

    private bool IsAtNest(){
        return transform.position.y >= LandscapeConstants.NestPosition.y && Vector3.Distance(transform.position, LandscapeConstants.NestPosition) <  0.5;

    }
}



[System.Serializable]
public class NestEvent : UnityEvent<NestItem>{

}