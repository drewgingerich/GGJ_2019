using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBird : MonoBehaviour
{

    public List<NestItem> desiredItems = new List<NestItem>();

    [SerializeField]
	AudioSource song;

    [SerializeField]
    public Animator anim;

    [SerializeField]
    public Animator thoughtsAnim;

    [SerializeField]
    public AudioSource backgroundMusic;

    private float songLength = 5;

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

	public IEnumerator Sing(){
        backgroundMusic.volume = 0;
        anim.SetBool("isTalking", true);
        anim.SetTrigger("Explain");
        thoughtsAnim.SetBool("Singing", true);
        song.volume = 1;
        yield return new WaitForSeconds(10f);
        Debug.Log("and we're back");
		song.volume = 0;
        anim.SetTrigger("Chill");
        thoughtsAnim.SetTrigger("Chill");
        backgroundMusic.volume = 1;
    }
}
