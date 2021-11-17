using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SwitchScript : MonoBehaviour
{
    public enum MyColors
    {
        red, green, blue
    };

    public Tilemap associatedTilemapOn;
    public MyColors keyColor = MyColors.green;
    public Sprite spriteOn;
    public Sprite spriteOff;
    public bool isOn = true;

    private AudioSource audioSource;
    public AudioClip audioON;
    public AudioClip audioOFF;
    [Range(0.0f, 1.0f)]
    public float audioVolume = 1.0f;
    private int audioVolumeGlobal;

    private string colorName = "green";
    private SpriteRenderer spriteRenderer;
    private TilemapRenderer tilemapRenderer;
    private TilemapCollider2D tilemapCollider2D;

    private Animator animatorSwitch;

    void Start()
    {
        animatorSwitch = gameObject.GetComponent<Animator>();
        animatorSwitch.SetBool("IsInitialOn", isOn);
        animatorSwitch.SetBool("IsOn", isOn);
        audioVolumeGlobal = PlayerPrefs.GetInt("VolumeEffects", 75);
        SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
        if (keyColor == MyColors.blue)
        {
            rend.color = new Color(0.2787469f, 0.3286503f, 0.6792453f);
            colorName = "blue";
        }
        else if (keyColor == MyColors.green)
        {
            rend.color = new Color(0.3907071f, 0.8396226f, 0.384167f);
            colorName = "green";
        }
        else if (keyColor == MyColors.red)
        {
            rend.color = new Color(0.7075472f, 0.3037113f, 0.3076654f);
            colorName = "red";
        }

        tilemapRenderer = associatedTilemapOn.GetComponent<TilemapRenderer>();
        tilemapCollider2D = associatedTilemapOn.GetComponent<TilemapCollider2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        audioSource = gameObject.GetComponent<AudioSource>();
        SwitchRender(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Pressed " + colorName + " color switch");
        if (collision.gameObject.CompareTag("Player"))
        {
            isOn = !isOn;
            SwitchRender();
        }
    }

    private void SwitchRender()
    {
        if (isOn)
        {
            animatorSwitch.SetBool("IsOn", true);
            audioSource.PlayOneShot(audioON, audioVolume * (audioVolumeGlobal / 100.0f));
            spriteRenderer.sprite = spriteOn;
            tilemapRenderer.enabled = true;
            tilemapCollider2D.enabled = true;
        }
        else
        {
            animatorSwitch.SetBool("IsOn", false);
            audioSource.PlayOneShot(audioOFF, audioVolume * (audioVolumeGlobal / 100.0f));
            spriteRenderer.sprite = spriteOff;
            tilemapRenderer.enabled = false;
            tilemapCollider2D.enabled = false;
        }
    }

    private void SwitchRender(bool flag)
    {
        if (isOn)
        {
            spriteRenderer.sprite = spriteOn;
            tilemapRenderer.enabled = true;
            tilemapCollider2D.enabled = true;
        }
        else
        {
            spriteRenderer.sprite = spriteOff;
            tilemapRenderer.enabled = false;
            tilemapCollider2D.enabled = false;
        }
    }
}
