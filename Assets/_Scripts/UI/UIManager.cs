using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image timerFillBar;
    [SerializeField] private Image inventoryFillBar;
    [SerializeField] private TextMeshProUGUI cashAmountText;

    // Start is called before the first frame update
    void Start()
    {
        InventoryBarReset();
        TimerReset();
        cashAmountUpdate(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(timerFillBar.fillAmount > 0f)
        {
            timerFillBar.fillAmount -= (Time.deltaTime/10);
        }
        if(inventoryFillBar.fillAmount <= 0f)
        {
            // let the GameManager know that the day is over
        }

        InventoryBarChange(0.1f);
    }

    public void TimerReset()
    {
        timerFillBar.fillAmount = 1f;
    }

    // this can be changed but the max the fill can be is 1 and takes in a float
    public void InventoryBarChange(float weightAdded)
    {
        if(inventoryFillBar.fillAmount >= 1f)
        {
            inventoryFillBar.fillAmount += (weightAdded/100f);
        }
    }
    public void InventoryBarReset()
    {
        inventoryFillBar.fillAmount = 0f;
    }

    // puts the inputted integer as the value, it should be calculated in the GameManager
    public void cashAmountUpdate(int cashAmountTotal) 
    {
        cashAmountText.text = "$" + cashAmountTotal;
    }
}
