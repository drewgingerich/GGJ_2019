using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	const string NORMAL = "Pleased";
	const string ANGRY = "Angered";
	const string SINGING = "Singing";

	const string LOVE = "Happy";
	const string SCRIBBLES = "Angry";
	const string MUSIC = "Singing";

    void Start() {
        StartCoroutine(OpeningCutsceneRoutine());
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
		bird.anim.SetBool(talkingBool, true);
		bird.anim.SetTrigger(birdStateTrigger);
		bird.thoughtsAnim.SetBool(thoughtsStateBool, true);

		yield return new WaitForSeconds(2f);

		bird.anim.SetBool(talkingBool, false);
		bird.thoughtsAnim.SetBool(thoughtsStateBool, false);

		yield return new WaitForSeconds(1f);
	}

    IEnumerator OpeningCutsceneRoutine() {
        yield return StartCoroutine(FlyToBranch());

		yield return StartCoroutine(InteractionRoutine(bird, NORMAL, LOVE));
		yield return StartCoroutine(InteractionRoutine(hotBird, SINGING, MUSIC));
		yield return StartCoroutine(InteractionRoutine(bird, SINGING, SCRIBBLES));
		yield return StartCoroutine(InteractionRoutine(hotBird, ANGRY, SCRIBBLES));
		yield return StartCoroutine(InteractionRoutine(hotBird, SINGING, MUSIC));

        ReleasePlayer();
    }

	IEnumerator GoodItemCutsceneRoutine() {
		yield return StartCoroutine(FlyToBranch());

		yield return StartCoroutine(InteractionRoutine(bird, NORMAL, LOVE));
		yield return StartCoroutine(InteractionRoutine(hotBird, NORMAL, MUSIC));

		ReleasePlayer();
	}

    IEnumerator BadItemCutsceneRoutine(NestItem item) {
		yield return StartCoroutine(FlyToBranch());

		yield return StartCoroutine(InteractionRoutine(bird, NORMAL, LOVE));
		yield return StartCoroutine(InteractionRoutine(hotBird, ANGRY, SCRIBBLES));
		item.transform.SetParent(null);
		item.Fall();
		yield return StartCoroutine(InteractionRoutine(hotBird, SINGING, SCRIBBLES));

        ReleasePlayer();
    }

	IEnumerator EndingCutsceneRoutine() {
		yield return StartCoroutine(FlyToBranch());

		yield return StartCoroutine(InteractionRoutine(bird, NORMAL, LOVE));
		yield return StartCoroutine(InteractionRoutine(bird, SINGING, MUSIC));
		yield return StartCoroutine(InteractionRoutine(hotBird, NORMAL, LOVE));
		StartCoroutine(InteractionRoutine(bird, SINGING, MUSIC));
		yield return StartCoroutine(InteractionRoutine(hotBird, SINGING, MUSIC));

		yield return null;

        ReleasePlayer();
	}

    IEnumerator FlyToBranch() {
		player.animating = true;
        Vector3 startingPosition = player.transform.position;
        float distance = (branchSitPosition.position - startingPosition).magnitude;
        float travelTime = 0.3f * distance;
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
