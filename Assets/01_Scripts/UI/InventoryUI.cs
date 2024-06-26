using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI eyeAmountText;

    private void Start()
    {
        eyeAmountText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateEyeAmountText(PlayerInventory playerInventory)
    {
        eyeAmountText.text = playerInventory.NumberOfEyes.ToString();
    }
}
