using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeMixer : SettingAdjust
{

    public AudioMixer audioMixer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Awake()
    {
        initial();
    }
    
       
        
    
    public override void OnSliderValueChange(float value)
    {
        base.OnSliderValueChange(value);
        SetMasterVolume(slider.value);
    }

    public override void OnInputFieldEndEdit(string text)
    {
        base.OnInputFieldEndEdit(text);

        SetMasterVolume(slider.value);
    }
    
    public void SetMasterVolume(float sliderValue)
    {
        // 💥 注意：如果 sliderValue 是 0，Log10(0) 會變成負無限大，會出 Bug
        if (sliderValue <= 0)
        {
            audioMixer.SetFloat(ValueName, -80f); // -80 分貝在 Unity 代表完全靜音
            return;
        }

        // 📐 聲音數學公式：將 0~1 的線性數值轉為分貝（dB）
        // 當 slider = 1，dB = 0 (原始音量)
        // 當 slider = 0.0001，dB = -80 (靜音)
        float dB = Mathf.Log10(sliderValue) * 20f;

        audioMixer.SetFloat(ValueName, dB);
    }
    public override void initial()
    {
        base.initial();
        audioMixer.SetFloat(ValueName, PlayerPrefs.GetFloat(ValueName));
    }

    // Update is called once per frame

}
