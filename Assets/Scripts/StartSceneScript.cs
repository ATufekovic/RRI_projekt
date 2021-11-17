using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneScript : MonoBehaviour
{
    private string jumpKeyCode;
    // Start is called before the first frame update
    void Start()
    {
        jumpKeyCode = PlayerPrefs.GetString("Jump", "Space");
        gameObject.GetComponent<Text>().text = "A/D or left/right arrows or horizontal joystick axis to move.\n" + jumpKeyCode + " to jump.";
    }
}
