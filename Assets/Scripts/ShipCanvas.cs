using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCanvas : MonoBehaviour
{
    protected Transform camLocation;

    protected RectTransform hb_back;
    protected RectTransform hb_bar;

    private Ship ship;

    protected virtual void Awake()
    {
        camLocation = Camera.main.transform;
        GetComponent<Canvas>().worldCamera = Camera.main;
        hb_back = transform.Find("HealthBar").GetComponent<RectTransform>();
        hb_bar = hb_back.transform.GetChild(0).GetComponent<RectTransform>();
        ship = transform.parent.GetComponent<Ship>();
    }
    protected virtual void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - camLocation.position);

        float healthPercent = ship.GetCurrentHealthPercent();
        if(Mathf.Abs(healthPercent) < 0.001)
        {
            Destroy(gameObject);
        }
        if(hb_back.gameObject.activeSelf && Mathf.Abs(healthPercent - 1) < 0.001)
        {
            hb_back.gameObject.SetActive(false);
        }
        else if(!hb_back.gameObject.activeSelf && Mathf.Abs(healthPercent - 1) > 0.001)
        {
            hb_back.gameObject.SetActive(true);
        }

        if (hb_back.gameObject.activeSelf)
        {
            hb_bar.sizeDelta = new Vector2(healthPercent * hb_back.sizeDelta.x, hb_back.sizeDelta.y);
        }
    }
}
