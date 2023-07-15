using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Timer : MonoBehaviour
{
    public float maxTime = 60.0f;

    private float _timeLeft;
    private bool _updateTimer;

    private void Awake()
    {
        StopTimer();
    }

    private void Update()
    {
        if (_updateTimer)
        {
            _timeLeft -= Time.deltaTime;

            UIManager.SetTimerText(
                TimeSpan.FromSeconds(_timeLeft).Minutes + ":" +
                TimeSpan.FromSeconds(_timeLeft).Seconds + ":" +
                (TimeSpan.FromMinutes(_timeLeft).Milliseconds / 10).ToString("00")
            );

            if (_timeLeft <= 0)
            {
                StopTimer();

                if (TryGetComponent(out Player.Player player))
                    player.OnRoundEnded();
            }
        }
    }

    public void RestartTimer()
    {
        _timeLeft = maxTime;
        _updateTimer = true;
    }

    public void StopTimer()
    {
        _updateTimer = false;
    }
}