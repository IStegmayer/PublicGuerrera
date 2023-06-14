using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dzajna
{
public class StatusBar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }


    public void SetMaxStat(float maxStat)
    {
        slider.maxValue = maxStat;
        slider.value = maxStat;
    }

    public void SetMaxStat(int maxStat)
    {
        slider.maxValue = maxStat;
        slider.value = maxStat;
    }

    public void SetCurrentStat(float currentStat)
    {
        slider.value = currentStat;
    }

    public void SetCurrentStat(int currentStat)
    {
        slider.value = currentStat;
    }
}
}