using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//button that quits the game, in editor or full build
public class ButtonQuit : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { Quit(); });
    }

    private void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
}
