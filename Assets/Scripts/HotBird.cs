using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBird : MonoBehaviour
{
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

    public void EnterCutsceneMode() {
        particleSystem.Stop();
        particleSystem.Clear();
        interactable.SetActive(false);
        backgroundMusic.volume = 0;
    }

    public void ExitCutsceneMode() {
        particleSystem.Play();
        interactable.SetActive(true);
        backgroundMusic.volume = 1;
    }

	public IEnumerator SingRoutine(float time = 5.5f){
        yield return StartCoroutine(FadeVolumeRoutine(1));
        yield return new WaitForSeconds(time);
        yield return StartCoroutine(FadeVolumeRoutine(0));
    }

    IEnumerator FadeVolumeRoutine(float target) {
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
