// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CutscenePlayer : MonoBehaviour
// {
//     [SerializeField]
//     BirdController player;

//     [SerializeField]
//     HotBird bird;
//     [SerializeField]
//     Animator birdAnimator;

//     [SerializeField]
//     HotBird hotBird;
//     [SerializeField]
//     Animator hotBirdAnimator;

//     [SerializeField]
//     Transform branchSitPosition;

//     void Start() {
//         StartCoroutine(OpeningCutsceneRoutine());
//     }

//     public void PlayIntermediateCutscene() {
//         StartCoroutine(IntermediateCutsceneRoutine());
//     }

//     public void PlayEndingCutscene() {
//         StartCoroutine(EndingCutsceneRoutine());
//     }

//     IEnumerator OpeningCutsceneRoutine() {
//         yield return StartCoroutine(FlyToBranch());

//         bird.ShowBubble(BubbleContents.LOVE);
//         yield return StartCoroutine(bird.NormalRoutine());
//         bird.HideBubble();

//         hotBird.ShowBubble(BubbleContents.MUSIC);
//         yield return StartCoroutine(bird.NormalRoutine());
//         hotBird.HideBubble();

//         bird.ShowBubble(BubbleContents.SCRIBBLES);
//         yield return StartCoroutine(bird.SingRoutine());
//         bird.HideBubble();

//         hotBird.ShowBubble(BubbleContents.SCRIBBLES);
//         yield return StartCoroutine(bird.AngerRoutine());
//         hotBird.HideBubble();

//         hotBird.ShowBubble(BubbleContents.MUSIC);
//         yield return StartCoroutine(bird.SingRoutine());
//         hotBird.HideBubble();

//         yield return null;

//         ReleasePlayer();
//     }

//     IEnumerator IntermediateCutsceneRoutine() {
// 		yield return StartCoroutine(FlyToBranch());
	
// 		bird.ShowBubble(BubbleContents.LOVE);
// 		yield return StartCoroutine(bird.NormalRoutine());
// 		bird.HideBubble();

// 		hotBird.ShowBubble(BubbleContents.MUSIC);
// 		yield return StartCoroutine(bird.NormalRoutine());
// 		hotBird.HideBubble();

// 		bird.ShowBubble(BubbleContents.SCRIBBLES);
// 		yield return StartCoroutine(bird.SingRoutine());
// 		bird.HideBubble();

// 		hotBird.ShowBubble(BubbleContents.SCRIBBLES);
// 		yield return StartCoroutine(bird.AngerRoutine());
// 		hotBird.HideBubble();

// 		hotBird.ShowBubble(BubbleContents.MUSIC);
// 		yield return StartCoroutine(bird.SingRoutine());
// 		hotBird.HideBubble();

// 		yield return null;

//         ReleasePlayer();
//     }

// 	IEnumerator EndingCutsceneRoutine() {
// 		yield return StartCoroutine(FlyToBranch());

// 		bird.ShowBubble(BubbleContents.LOVE);
// 		yield return StartCoroutine(bird.SingRoutine());
// 		bird.HideBubble();

// 		hotBird.ShowBubble(BubbleContents.LOVE);
// 		yield return StartCoroutine(bird.SingRoutine());
// 		hotBird.HideBubble();

// 		hotBird.ShowBubble(BubbleContents.LOVE);
// 		bird.ShowBubble(BubbleContents.LOVE);
// 		yield return StartCoroutine(bird.SingRoutine());
// 		bird.HideBubble();

// 		hotBird.ShowBubble(BubbleContents.LOVE);
// 		bird.ShowBubble(BubbleContents.LOVE);
// 		yield return StartCoroutine(bird.DanceRoutine());
// 		bird.HideBubble();

// 		yield return null;

//         ReleasePlayer();
// 	}

//     IEnumerator FlyToBranch() {
//         Vector3 startingPosition = player.transform.position;
//         float distance = (branchSitPosition.position - startingPosition).magnitude;
//         float travelTime = 3 * distance;
//         float timer = 0;
//         while (timer < travelTime) {
//             timer += Time.deltaTime;
//             Vector3 newPosition = Vector3.Lerp(startingPosition, branchSitPosition.position, timer / travelTime);
//             player.transform.position = newPosition;
//             yield return null;
//         }
//         player.gameObject.SetActive(false);
//         bird.gameObject.SetActive(true);
//     }

//     void ReleasePlayer() {
//         bird.gameObject.SetActive(false);
// 		player.gameObject.SetActive(true);
//     }
// }
