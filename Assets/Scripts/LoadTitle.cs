using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class LoadTitle : MonoBehaviour
{

    public VideoPlayer video;

    void Awake() {
        video = GetComponent<VideoPlayer>();
        video.loopPointReached += OnMovieFinished; 
    }

    void OnMovieFinished(VideoPlayer player)
    {
        Debug.Log("Event for movie end called");
        SceneManager.LoadScene("Title");
    }
}
