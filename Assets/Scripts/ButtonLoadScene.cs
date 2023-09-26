using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//button that loads a scene
public class ButtonLoadScene : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { LoadScene(); });
    }

    private void LoadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneToLoad);
    }
}
