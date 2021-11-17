using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour
{
    public Text textJumpKey;

    public Slider sliderVolumeEffects;
    private Text textSliderVolumeEffects;
    public Slider sliderVolumeMusic;
    private Text textSliderVolumeMusic;

    public Text textDeaths;

    private KeyCode jumpKey;
    private bool changeJumpKey = false;
    private int volumeEffects;
    private int volumeMusic;

    // Start is called before the first frame update
    void Start()
    {
        jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", KeyCode.Space.ToString()));
        textJumpKey.text = jumpKey.ToString();

        volumeEffects = PlayerPrefs.GetInt("VolumeEffects", 75);
        textSliderVolumeEffects = sliderVolumeEffects.transform.GetChild(3).gameObject.GetComponent<Text>();
        textSliderVolumeEffects.text = volumeEffects.ToString();
        sliderVolumeEffects.value = volumeEffects/1.0f;

        volumeMusic = PlayerPrefs.GetInt("VolumeMusic", 75);
        textSliderVolumeMusic = sliderVolumeMusic.transform.GetChild(3).gameObject.GetComponent<Text>();
        textSliderVolumeMusic.text = volumeMusic.ToString();
        sliderVolumeMusic.value = volumeMusic / 1.0f;

        textDeaths.text = "Deaths: " + PlayerPrefs.GetInt("Deaths", 0).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (changeJumpKey)
        {
            foreach(KeyCode kc in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kc))
                {
                    jumpKey = kc;
                    textJumpKey.text = jumpKey.ToString();
                    PlayerPrefs.SetString("Jump", jumpKey.ToString());
                    changeJumpKey = false;
                }
            }
        }
    }

    public void SetVolumeEffects()
    {
        int volume = Convert.ToInt32(sliderVolumeEffects.value);
        textSliderVolumeEffects.text = volume.ToString();
        PlayerPrefs.SetInt("VolumeEffects", volume);
    }
    public void SetVolumeMusi()
    {
        int volume = Convert.ToInt32(sliderVolumeMusic.value);
        textSliderVolumeMusic.text = volume.ToString();
        PlayerPrefs.SetInt("VolumeMusic", volume);
    }
    public void WipeStats()
    {
        PlayerPrefs.SetInt("Deaths", 0);
        textDeaths.text = "Deaths: 0";
    }
    public void SetJumpKey()
    {
        changeJumpKey = true;
        textJumpKey.text = "Waiting for input...";
    }
}
