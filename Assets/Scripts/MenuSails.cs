using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSails : MonoBehaviour
{
    private Renderer playerSailRenderer;
    private Renderer playerRolledSailRenderer;

    private void Awake()
    {
        playerSailRenderer = GameObject.Find("playerSail").GetComponent<Renderer>();
        playerRolledSailRenderer = GameObject.Find("playerRolledSail2").GetComponent<Renderer>();
    }

    private void Start()
    {
        playerSailRenderer.material.color = GameManager.Instance.playerColor;
        playerRolledSailRenderer.material.color = GameManager.Instance.playerColor;
    }

    private void Update()
    {
        if (!playerRolledSailRenderer.material.color.Equals(GameManager.Instance.playerColor))
        {
            playerSailRenderer.material.color = GameManager.Instance.playerColor;
            playerRolledSailRenderer.material.color = GameManager.Instance.playerColor;
        }
    }
}
