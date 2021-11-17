using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KeyScript : MonoBehaviour
{
    public enum MyColors
    {
        red, green, blue
    };

    public MyColors keyColor = MyColors.green;
    public Tilemap associatedTilemap;
    public AudioClip[] audioClips;
    [Range(0.0f, 1.0f)]
    public float audioVolume = 1.0f;
    private int audioVolumeGlobal;
    private AudioSource audioSource;
    private bool hasTriggered = false;

    private Animator animatorLock;
    
    private void Start()
    {
        animatorLock = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioVolumeGlobal = PlayerPrefs.GetInt("VolumeEffects", 75);
        SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
        if(keyColor == MyColors.blue)
        {
            rend.color = new Color(0.2787469f, 0.3286503f, 0.6792453f);
        } else if(keyColor == MyColors.green)
        {
            rend.color = new Color(0.3907071f, 0.8396226f, 0.384167f);
        } else if(keyColor == MyColors.red)
        {
            rend.color = new Color(0.7075472f, 0.3037113f, 0.3076654f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Pressed " + colorName + " color key, has triggered: " + hasTriggered.ToString());
        animatorLock.SetBool("IsUnlocked", true);
        if (collision.gameObject.CompareTag("Player") && (!hasTriggered) && (associatedTilemap != null))
        {
            int numOfChildren = associatedTilemap.transform.childCount;
            for(int i = numOfChildren - 1; i >= 0; i--)
            {
                Destroy(associatedTilemap.transform.GetChild(i).gameObject);
            }
            Destroy(associatedTilemap.gameObject);
            hasTriggered = true;

            int trackToPlay = Random.Range(0, audioClips.Length);
            audioSource.PlayOneShot(audioClips[trackToPlay], audioVolume*(audioVolumeGlobal / 100.0f));
        }
    }
}
