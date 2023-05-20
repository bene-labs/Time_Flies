using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Timer : MonoBehaviour
{
   public float timeLeft = 60.0f;
   public TextMeshProUGUI timeText;

   private void Update()
   {
       timeLeft -= Time.deltaTime; 
       timeText.text = TimeSpan.FromSeconds(timeLeft).Minutes + ":" + 
                                                   TimeSpan.FromSeconds(timeLeft).Seconds + ":" + 
                                                   (TimeSpan.FromMinutes(timeLeft).Milliseconds / 10).ToString("00");
   }
}
