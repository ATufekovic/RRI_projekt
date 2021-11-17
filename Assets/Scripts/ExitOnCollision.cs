using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitOnCollision : MonoBehaviour
{
    public string sceneNameToLoad = "MainMenu";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Entered scene change trigger");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Loading scene: " + sceneNameToLoad);
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}
