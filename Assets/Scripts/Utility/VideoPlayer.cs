using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoPlayer : MonoBehaviour
{
    private float elapsedTime = 0f;
    private float targetTime = 55f; 

    void Update()
    {

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= targetTime)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
