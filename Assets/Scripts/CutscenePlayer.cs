using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutscenePlayer : MonoBehaviour
{
	[SerializeField]
	bool skipCutscenes;

    [SerializeField]
    BirdController player;

	[SerializeField]
	NestItem fallingLeaf;

    [SerializeField]
    HotBird bird;
    [SerializeField]
    Animator birdAnimator;

    [SerializeField]
    HotBird hotBird;
    [SerializeField]
    Animator hotBirdAnimator;

    [SerializeField]
    Transform branchSitPosition;

	const float THOUGHT_BUBBLE_APPEARANCE_TIME = 0.2f;

	const string TALKING = "isTalking";
	const string THINKING = "isThinking";

	const string BIRD_TALK_HAPPY = "Pleased";
	const string BIRD_TALK_ANGRY = "Angered";
	const string BIRD_SING = "Explain";
	const string BIRD_DANCE = "Dance";

	const string THOUGHT_LOVE = "Heart";
	const string THOUGHT_SCRIBBLE = "Scribbles";
	const string THOUGHT_MUSIC = "Music";

    void Start() {
        StartCoroutine(OpeningCutsceneRoutine());
    }

	public void PlaySampleSongCutscene() {
		StartCoroutine(SampleSongCutsceneRoutine());
	}

	public void PlayGoodItemCutscene() {
		StartCoroutine(GoodItemCutsceneRoutine());
	}

	public void PlayBadItemCutscene(NestItem item) {
		StartCoroutine(BadItemCutsceneRoutine(item));
	}

    public void PlayEndingCutscene() {
        StartCoroutine(EndingCutsceneRoutine());
    }

	IEnumerator InteractionRoutine(HotBird bird, string birdStateTrigger, string thoughtsStateTrigger) {
		bird.thoughtsAnim.SetBool(THINKING, true);
		bird.thoughtsAnim.SetTrigger(thoughtsStateTrigger);
		yield return new WaitForSeconds(THOUGHT_BUBBLE_APPEARANCE_TIME);

		if (birdStateTrigger == BIRD_SING || birdStateTrigger == BIRD_DANCE) {
			bird.backgroundMusic.volume = 0;
			bird.song.volume = 1;
		}

		bird.anim.SetBool(TALKING, true);
		bird.anim.SetTrigger(birdStateTrigger);
		if (birdStateTrigger == BIRD_SING || birdStateTrigger == BIRD_DANCE) {
			yield return new WaitForSeconds(5f);
		} else {
			yield return new WaitForSeconds(1.5f);
		}

		bird.thoughtsAnim.SetBool(THINKING, false);
		bird.anim.SetBool(TALKING, false);

		if (birdStateTrigger == BIRD_SING || birdStateTrigger == BIRD_DANCE) {
			bird.backgroundMusic.volume = 0.2f;
			bird.song.volume = 0;
		}

		yield return new WaitForSeconds(0.5f);
	}

	IEnumerator SampleSongCutsceneRoutine() {
		if (skipCutscenes) {
			yield break;
		}
		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_SING, THOUGHT_MUSIC));	
	}

    IEnumerator OpeningCutsceneRoutine() {
		if (skipCutscenes) {
			yield break;
		}

        yield return StartCoroutine(FlyToBranch());

		yield return StartCoroutine(InteractionRoutine(bird, BIRD_TALK_HAPPY, THOUGHT_LOVE));
		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_SING, THOUGHT_MUSIC));
		yield return StartCoroutine(InteractionRoutine(bird, BIRD_SING, THOUGHT_SCRIBBLE));
		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_TALK_ANGRY, THOUGHT_SCRIBBLE));
		fallingLeaf.Fall();
		yield return new WaitForSeconds(5f);
		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_TALK_HAPPY, THOUGHT_LOVE));
		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_SING, THOUGHT_MUSIC));

        ReleasePlayer();
    }

	IEnumerator GoodItemCutsceneRoutine() {
		if (skipCutscenes) {
			yield break;
		}

		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_TALK_HAPPY, THOUGHT_LOVE));

		//ReleasePlayer();
	}

    IEnumerator BadItemCutsceneRoutine(NestItem item) {
		if (skipCutscenes) {
			yield break;
		}

		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_TALK_ANGRY, THOUGHT_SCRIBBLE));
		item.transform.SetParent(null);
		item.Fall();

       // ReleasePlayer();
    }

	IEnumerator EndingCutsceneRoutine() {
		if (skipCutscenes) {
			yield break;
		}

		yield return StartCoroutine(FlyToBranch());

		yield return StartCoroutine(InteractionRoutine(bird, BIRD_TALK_HAPPY, THOUGHT_LOVE));
		bird.song = hotBird.song;
		yield return StartCoroutine(InteractionRoutine(bird, BIRD_SING, THOUGHT_MUSIC));
		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_TALK_HAPPY, THOUGHT_LOVE));
		StartCoroutine(InteractionRoutine(bird, BIRD_DANCE, THOUGHT_MUSIC));
		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_DANCE, THOUGHT_MUSIC));

		SceneManager.LoadScene("Credits", LoadSceneMode.Single);
	}

    IEnumerator FlyToBranch() {
		player.animating = true;
		Animator playerAnimator = player.GetComponent<Animator>();
		playerAnimator.SetBool("Stopped", false);

        Vector3 startingPosition = player.transform.position;
        float distance = (branchSitPosition.position - startingPosition).magnitude;
        float travelTime = 0.2f * distance;
        float timer = 0;
        while (timer < travelTime) {
            timer += Time.deltaTime;
            Vector3 newPosition = Vector3.Lerp(startingPosition, branchSitPosition.position, timer / travelTime);
            player.transform.position = new Vector3(newPosition.x, newPosition.y, player.transform.position.z);
            yield return null;
        }
        player.gameObject.SetActive(false);
        bird.gameObject.SetActive(true);
    }

    void ReleasePlayer() {
		Animator playerAnimator = player.GetComponent<Animator>();
		playerAnimator.SetBool("Stopped", true);		
		player.animating = false;
        bird.gameObject.SetActive(false);
		player.gameObject.SetActive(true);
    }
}
