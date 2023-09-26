using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UpgradeableStatShop : MonoBehaviour
{
    private UpgradeableStat stat;

    private Button btn_levelDown, btn_levelUp, btn_confirm, btn_cancel;
    private TextMeshProUGUI txt_Level, txt_cost;

    private void Awake()
    {
        btn_levelDown = transform.Find("Btn_LevelDown").GetComponent<Button>();
        btn_levelUp = transform.Find("Btn_LevelUp").GetComponent<Button>();
        txt_Level = transform.Find("Txt_Level").GetComponent<TextMeshProUGUI>();
        txt_cost = transform.Find("Txt_Cost").GetComponent<TextMeshProUGUI>();

        btn_confirm = transform.parent.Find("Btn_Confirm").GetComponent<Button>();
        btn_cancel = transform.parent.Find("Btn_Cancel").GetComponent<Button>();

        btn_levelDown.onClick.AddListener(delegate { TempLevelDown(); });
        btn_levelUp.onClick.AddListener(delegate { TempLevelUp(); });
        btn_confirm.onClick.AddListener(delegate { ConfirmLevelUp(); });
        btn_cancel.onClick.AddListener(delegate { CancelLevelUp(); });
    }

    private void Update()
    {
        txt_Level.text = "lvl " + stat.tmplvl;
        txt_cost.text = stat.CostFunction(stat.tmplvl + 1) + "g";

        btn_levelDown.interactable = stat.tmplvl > stat.lvl;
        btn_levelUp.interactable = GameManager.Instance.playerHideout.currentGold - stat.CostFunction(stat.tmplvl + 1) >= 0;

        if(stat.tmplvl > stat.lvl)
        {
            txt_Level.color = Color.green;
        }
        else
        {
            txt_Level.color = Color.white;
        }
    }

    private void TempLevelUp()
    {
        GameManager.Instance.playerHideout.currentGold -= stat.CostFunction(stat.tmplvl + 1);
        stat.TempLevelUp();
    }

    private void TempLevelDown()
    {
        GameManager.Instance.playerHideout.currentGold += stat.CostFunction(stat.tmplvl);
        stat.TempLevelDown();
    }

    private void CancelLevelUp()
    {
        stat.CancelLevelUp(out int refund);
        GameManager.Instance.playerHideout.currentGold += refund;
        GameManager.Instance.ui.CloseShop();
    }

    private void ConfirmLevelUp()
    {
        stat.ConfirmLevelUp();
        GameManager.Instance.ui.CloseShop();
    }

    public void SetStat(UpgradeableStat _stat)
    {
        stat = _stat;
    }
}
