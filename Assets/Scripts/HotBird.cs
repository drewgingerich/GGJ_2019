using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBird : MonoBehaviour
{

    [SerializeField]
    public List<LandscapeConstants.NestItemCategory> DesiredItems = new List<LandscapeConstants.NestItemCategory>();
    // Start is called before the first frame update

    void Awake() {
        DesiredItems.Add(LandscapeConstants.NestItemCategory.YARN); 
    }
    void Start()
    {
        //add desired items to nest here
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Praise(){
        Debug.Log("YES YES IT'S BEAUTIFUL");
        yield return null;
    }

    public IEnumerator Scoff(){
        Debug.Log("Wow, you really thought I'd like THAT, huh. Interesting. Ok.");
        yield return null;
    }

    public void Sing(){

    }
}
