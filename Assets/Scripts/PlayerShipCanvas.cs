using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerShipCanvas : ShipCanvas
{
    private TextMeshProUGUI goldCounter, openShopText, pickupMultiplierText;
    private PlayerShip player;

    protected RectTransform bb_back;
    protected RectTransform bb_bar;

    protected override void Awake()
    {
        base.Awake();
        goldCounter = transform.Find("Txt_PlayerShipGold").GetComponent<TextMeshProUGUI>();
        openShopText = transform.Find("Txt_OpenShop").GetComponent<TextMeshProUGUI>();
        pickupMultiplierText = transform.Find("Txt_PickupMultiplier").GetComponent<TextMeshProUGUI>();
        player = transform.parent.GetComponent<PlayerShip>();

        bb_back = transform.Find("BoostBar").GetComponent<RectTransform>();
        bb_bar = bb_back.transform.GetChild(0).GetComponent<RectTransform>();
    }
    protected override void Update()
    {
        base.Update();
        if(player.gold == 0)
        {
            goldCounter.text = "";
        }
        else
        {
            goldCounter.text = player.gold + "g";
        }

        if(player.GetPickupGoldMultiplier() < 1.01)
        {
            pickupMultiplierText.gameObject.SetActive(false);
        }
        else
        {
            pickupMultiplierText.gameObject.SetActive(true);
            pickupMultiplierText.text = player.GetPickupGoldMultiplier().ToString("F1") + " X Gold Pickups";
        }

        //update boost bar
        float boostPercent = player.GetBoostChargePercent();
        if (bb_back.gameObject.activeSelf && Mathf.Abs(boostPercent - 1) < 0.001)
        {
            bb_back.gameObject.SetActive(false);
        }
        else if (!bb_back.gameObject.activeSelf && Mathf.Abs(boostPercent - 1) > 0.001)
        {
            bb_back.gameObject.SetActive(true);
        }

        if (bb_back.gameObject.activeSelf)
        {
            bb_bar.sizeDelta = new Vector2(boostPercent * bb_back.sizeDelta.x, bb_back.sizeDelta.y);
        }

        openShopText.gameObject.SetActive(player.isInHideout);
    }
}
