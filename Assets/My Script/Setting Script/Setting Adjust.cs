using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingAdjust : MonoBehaviour
{
    public Slider slider;
    public TMP_InputField IF;
    // 監聽：當滑桿數值改變時，自動去執行 OnVolumeChanged 這個 Function
    // 它會自動把當前的數值（float）傳過去
    
    private void Start()
    {
        slider.onValueChanged.AddListener(OnValueChange);
    }
    void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnValueChange);
    }

    void OnValueChange(float v)
    {

    }
}
