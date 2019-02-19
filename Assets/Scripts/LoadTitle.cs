using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadTitle : MonoBehaviour
{

    public Image teamTitleCard;
    public VideoPlayer video;

    void Start() {
        teamTitleCard.fillAmount = 0;
    }
    void Awake() {
        video = GetComponent<VideoPlayer>();
        video.loopPointReached += OnMovieFinished; 
    }

    void OnMovieFinished(VideoPlayer player)
    {
        teamTitleCard.fillAmount = 1;
        StartCoroutine(WaitThenSceneChangeRoutine());
        
    }

    IEnumerator WaitThenSceneChangeRoutine() {
        yield return new WaitForSeconds(2f);
        Debug.Log("scene change");
        SceneManager.LoadScene("Title");
        yield return null;
    }
}
