using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBird : MonoBehaviour
{

    public List<NestItem> desiredItems = new List<NestItem>();

    [SerializeField]
	public AudioSource song;

    [SerializeField]
    public Animator anim;

    [SerializeField]
    public Animator thoughtsAnim;

    [SerializeField]
    public AudioSource backgroundMusic;

    [SerializeField]
    Interactable interactable;

    [SerializeField]
    new ParticleSystem particleSystem;

    private float songLength = 5;

    public void Sing() {
        StartCoroutine(SingRoutine());
    }

    public void EnterCutsceneMode() {
        particleSystem.Stop();
        interactable.SetActive(false);
        backgroundMusic.volume = 0;
    }

    public void ExitCutsceneMode() {
        particleSystem.Play();
        interactable.SetActive(true);
        backgroundMusic.volume = 1;
    }

	public IEnumerator SingRoutine(){
        EnterCutsceneMode();
        anim.SetBool("isTalking", true);
        anim.SetTrigger("Explain");
        thoughtsAnim.SetBool("isThinking", true);
        thoughtsAnim.SetTrigger("Music");
        yield return StartCoroutine(FadeVolumeRoutine(1));
        yield return new WaitForSeconds(10f);
        yield return StartCoroutine(FadeVolumeRoutine(0));
        anim.SetBool("isTalking", false);
        thoughtsAnim.SetBool("isThinking", false);
        ExitCutsceneMode();
    }

    public IEnumerator FadeVolumeRoutine(float target) {
        const float fadeTime = 1f;
        float timer = 0;
        float start = song.volume;
        while (song.volume != target) {
            timer += Time.deltaTime;
            song.volume = Mathf.Lerp(start, target, timer/fadeTime);
            yield return null;
        }
    }
}
