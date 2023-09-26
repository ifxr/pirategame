using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHideout : MonoBehaviour
{
    [HideInInspector]
    public int totalGold, currentGold;
    [SerializeField]
    private float goldTransferRate;

    private PlayerShip player;

    private bool interact;
    private void Awake()
    {
        totalGold = 0;
        currentGold = totalGold;
    }

    private void Start()
    {
        player = GameManager.Instance.player;
    }

    private void Update()
    {
        interact = !interact && Input.GetButton("Interact");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            if (other.attachedRigidbody.gameObject == player.gameObject)
            {
                player.isInHideout = true;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            if (other.attachedRigidbody.gameObject == player.gameObject)
            {
                int goldIncrement = Mathf.RoundToInt(Mathf.Clamp((goldTransferRate + (player.gold/5)) * Time.deltaTime, 0, player.gold));
                player.gold -= goldIncrement;
                totalGold += goldIncrement;
                currentGold += goldIncrement;

                if (!GameManager.Instance.isEndlessMode && totalGold >= GameManager.Instance.selectedDifficultySetting.goldTarget)
                {
                    GameManager.Instance.Win();
                }

                if (interact)
                {
                    GameManager.Instance.ui.OpenShop();
                    interact = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody.gameObject == player.gameObject)
        {
            GameManager.Instance.ui.CloseShop();
            player.isInHideout = false;
        }
    }
}
