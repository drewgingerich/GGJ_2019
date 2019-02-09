using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            Debug.Log("fired");
            SceneManager.LoadScene("Title");
        }

        
    }
}