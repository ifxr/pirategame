using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//controls customize screen
public class Customize : MonoBehaviour
{
    private TMP_InputField nameField;
    private TMP_Dropdown difficultySelect;
    private TMP_Dropdown colorSelect;

    private List<Color> possiblePlayerColors = new List<Color>();



    private void Awake()
    {
        nameField = transform.Find("Input_Name").GetComponent<TMP_InputField>();
        difficultySelect = transform.Find("Dropdown_Difficulty").GetComponent<TMP_Dropdown>();
        colorSelect = transform.Find("Dropdown_ColorSelect").GetComponent<TMP_Dropdown>();

        //initialize possible colors list (must manually add dropdown options in inspector to be consistent)
        possiblePlayerColors.Add(new Color(247f / 255f, 247f / 255f, 247f / 255f)); //white
        possiblePlayerColors.Add(new Color(36 / 255f, 36 / 255f, 36 / 255f)); //black
        possiblePlayerColors.Add(new Color(140 / 255f, 18 / 255f, 18 / 255f)); //red
        possiblePlayerColors.Add(new Color(19 / 255f, 105 / 255f, 22 / 255f)); //green
        possiblePlayerColors.Add(new Color(19 / 255f, 105 / 255f, 105 / 255f)); //blue
        possiblePlayerColors.Add(new Color(252 / 255f, 239 / 255f, 91 / 255f)); //yellow
        possiblePlayerColors.Add(new Color(255 / 255f, 166 / 255f, 210 / 255f)); //pink
        possiblePlayerColors.Add(new Color(120 / 255f, 49 / 255f, 176 / 255f)); //purple

        
    }

    private void Start() //sets the initial values of ui elements to match data in GameManager
    {
        nameField.text = GameManager.Instance.playerName;
        difficultySelect.value = difficultySelect.options.FindIndex((i) => { return i.text.ToLower().Equals(GameManager.Instance.selectedDifficultySetting.name.ToLower()); }); 
        colorSelect.value = possiblePlayerColors.FindIndex(i => { return i.Equals(GameManager.Instance.playerColor); });
    }

    public void SetPlayerName(string newName) //called by name input field event
    {
        GameManager.Instance.playerName = newName;
    }

    public void SetDifficulty() //called by difficulty dropdown
    {
        GameManager.Instance.selectedDifficultySetting = GameManager.Instance.possibleDifficulties.Find(i => { return i.name.ToLower().Equals(difficultySelect.options[difficultySelect.value].text.ToLower()); });
    }

    public void SetColor() //called by color dropdown
    {
        GameManager.Instance.playerColor = possiblePlayerColors[colorSelect.value];
    }
}
