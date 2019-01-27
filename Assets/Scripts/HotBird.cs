using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBird : MonoBehaviour
{

    [SerializeField]
    public List<LandscapeConstants.NestItemCategory> DesiredItems = new List<LandscapeConstants.NestItemCategory>();

    [SerializeField]
	AudioSource song;

    private float songLength = 5;

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
        var elapsedTime = 0f;
        while (elapsedTime < songLength) {
            song.volume = 1;
            Debug.Log("YES YES IT'S BEAUTIFUL");
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        song.volume = 0;

    }

    public IEnumerator Scoff(){
         var elapsedTime = 0f;
        while (elapsedTime < songLength) {
            song.volume = 1;
            Debug.Log("Wow, you really thought I'd like THAT, huh. Interesting. Ok.");
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        song.volume = 0;
    }

    public void Sing(){

    }
}
