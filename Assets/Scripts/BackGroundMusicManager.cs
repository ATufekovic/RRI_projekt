using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackGroundMusicManager : MonoBehaviour
{
    //singleton pattern
    private static BackGroundMusicManager instance = null;
    public static BackGroundMusicManager Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        } else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public AudioClip bgmClip;
    public AudioClip bgmEndClip;
    private bool isEndTriggered = false;
    private AudioSource audioSource;

    public string sourcePositionName;
    private Transform sourcePosition;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = bgmClip;
        int audioVolume = PlayerPrefs.GetInt("VolumeMusic", 75);
        audioSource.volume = audioVolume/100.0f;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEndTriggered)
        {
            StartCoroutine(CheckForEndScene());
        }
        StartCoroutine(CenterCamera());
    }

    private IEnumerator CenterCamera()
    {
        //track an object (ie. player or camera) and set self to its position
        if (sourcePosition == null)
        {
            Debug.Log("Didnt find transform, searching...");
            sourcePosition = GameObject.Find(sourcePositionName).transform;
        }
        else
        {
            gameObject.transform.position = sourcePosition.position;
        }
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator CheckForEndScene()
    {

        if(SceneManager.GetActiveScene().name == "EndScene" && (!isEndTriggered))
        {
            isEndTriggered = true;
            audioSource.clip = bgmEndClip;
            audioSource.Play();
        }
        yield return new WaitForSeconds(1.0f);
    }
}
