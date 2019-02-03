using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutscenePlayer : MonoBehaviour
{
    [SerializeField]
    BirdController player;

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

	const string talkingBool = "isTalking";

	const string BIRD_TALK_HAPPY = "Pleased";
	const string BIRD_TALK_ANGRY = "Angered";
	const string BIRD_SING = "Explain";

	const string THOUGHT_LOVE = "Happy";
	const string THOUGHT_SCRIBBLE = "Angry";
	const string THOUGHT_MUSIC = "Singing";

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

	IEnumerator InteractionRoutine(HotBird bird, string birdStateTrigger, string thoughtsStateBool) {
		if (birdStateTrigger == BIRD_SING) {
			bird.backgroundMusic.volume = 0;
			bird.song.volume = 1;
		}

		bird.anim.SetBool(talkingBool, true);
		bird.anim.SetTrigger(birdStateTrigger);
		bird.thoughtsAnim.SetBool(thoughtsStateBool, true);
		if (birdStateTrigger == BIRD_SING) {
			yield return new WaitForSeconds(5f);
		} else {
			yield return new WaitForSeconds(1.5f);
		}

		bird.anim.SetBool(talkingBool, false);
		bird.thoughtsAnim.SetBool(thoughtsStateBool, false);

		if (birdStateTrigger == BIRD_SING) {
			bird.backgroundMusic.volume = 0.2f;
			bird.song.volume = 0;
		}

		yield return new WaitForSeconds(0.5f);
	}

	IEnumerator SampleSongCutsceneRoutine() {
		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_SING, THOUGHT_MUSIC));	
	}

    IEnumerator OpeningCutsceneRoutine() {
        yield return StartCoroutine(FlyToBranch());

		yield return StartCoroutine(InteractionRoutine(bird, BIRD_TALK_HAPPY, THOUGHT_LOVE));
		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_SING, THOUGHT_MUSIC));
		yield return StartCoroutine(InteractionRoutine(bird, BIRD_SING, THOUGHT_SCRIBBLE));
		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_TALK_ANGRY, THOUGHT_SCRIBBLE));
		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_SING, THOUGHT_MUSIC));

        ReleasePlayer();
    }

	IEnumerator GoodItemCutsceneRoutine() {
		yield return StartCoroutine(FlyToBranch());

		yield return StartCoroutine(InteractionRoutine(bird, BIRD_TALK_HAPPY, THOUGHT_LOVE));
		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_TALK_HAPPY, THOUGHT_MUSIC));

		ReleasePlayer();
	}

    IEnumerator BadItemCutsceneRoutine(NestItem item) {
		yield return StartCoroutine(FlyToBranch());

		yield return StartCoroutine(InteractionRoutine(bird, BIRD_TALK_HAPPY, THOUGHT_LOVE));
		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_TALK_ANGRY, THOUGHT_SCRIBBLE));
		item.transform.SetParent(null);
		item.Fall();
		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_SING, THOUGHT_MUSIC));

        ReleasePlayer();
    }

	IEnumerator EndingCutsceneRoutine() {
		yield return StartCoroutine(FlyToBranch());

		yield return StartCoroutine(InteractionRoutine(bird, BIRD_TALK_HAPPY, THOUGHT_LOVE));
		bird.song = hotBird.song;
		yield return StartCoroutine(InteractionRoutine(bird, BIRD_SING, THOUGHT_MUSIC));
		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_TALK_HAPPY, THOUGHT_LOVE));
		StartCoroutine(InteractionRoutine(bird, BIRD_SING, THOUGHT_MUSIC));
		yield return StartCoroutine(InteractionRoutine(hotBird, BIRD_SING, THOUGHT_MUSIC));

		SceneManager.LoadScene("Credits", LoadSceneMode.Single);
	}

    IEnumerator FlyToBranch() {
		player.animating = true;
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
		player.animating = false;
        bird.gameObject.SetActive(false);
		player.gameObject.SetActive(true);
    }
}
