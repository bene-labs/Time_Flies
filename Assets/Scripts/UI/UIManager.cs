using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI energyBarText;
    [SerializeField] private RectTransform energyBarValueTransform;
    [SerializeField] private GameObject deathScreen;

    private static UIManager instance;

    private void Awake()
    {
        // singleton pattern for easy ui component access
        if (instance == null) instance = this;
    }

    public static void SetTimerText(string text)
    {
        instance.timerText.text = text;
    }

    public static void SetEnergyBar(float currentValue, float maxValue)
    {
        instance.energyBarText.text = currentValue + " / " + maxValue;
        instance.energyBarValueTransform.localScale = new Vector3(Math.Clamp(currentValue / maxValue, 0, 1), 1, 1);
    }

    public static void SetDeathScreenVisibility(bool isVisible)
    {
        instance.deathScreen.SetActive(isVisible);
    }
}