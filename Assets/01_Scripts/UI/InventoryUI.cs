using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI eyeAmountText;

    private void Awake()
    {
        eyeAmountText = GetComponentInChildren<TextMeshProUGUI>();

        UpdateEyeAmountText(PlayerInventory.Instance);
    }

    public void UpdateEyeAmountText(PlayerInventory playerInventory)
    {
        eyeAmountText.text = $"x {playerInventory.NumberOfEyes}";
    }
}
