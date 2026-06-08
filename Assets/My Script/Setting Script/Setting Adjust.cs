using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingAdjust : MonoBehaviour
{
    public Slider slider;
    public TMP_InputField IF;
    public string setValue; 
    
    // 監聽：當滑桿數值改變時，自動去執行 OnVolumeChanged 這個 Function
    // 它會自動把當前的數值（float）傳過去

    private void Awake()
    {
        initial();
    }
    private void Start()
    {

       // slider.onValueChanged.AddListener(OnValueChange);
    }
    //void OnDestroy()
    //{
    //    slider.onValueChanged.RemoveListener(OnValueChange);
    //}

    public void OnSliderValueChange()
    {

        IF.placeholder.GetComponent<TextMeshProUGUI>().text = slider.value.ToString("0.00");
        PlayerPrefs.SetFloat(setValue,slider.value);
        PlayerPrefs.Save();
    }
    public void OnIFValueChange()
    {
        
        slider.value = OnInputFieldEndEdit();
        PlayerPrefs.Save();
    }
    public float OnInputFieldEndEdit()
    { 
        // 嘗試把字串轉換成 float[Range (0f,0.1f)]
        if (float.TryParse(IF.text, out float result))
        {
            if (result > 0.1f)
            {

                result = 0.1f;
                IF.text = result.ToString();
            }
            if (result < 0.0f)
            {
                result = 0.0f;
                IF.text = result.ToString();
            }
            //IF.text = Mathf.Clamp(result,0f,0.1f).ToString();

            //// 轉換成功！
            //return Mathf.Clamp(result, 0f, 0.1f);
            return result;
            Debug.Log($"成功拿到 float 數值: {result}");
        }
        else
        {
            return PlayerPrefs.GetFloat(setValue, 0.05f); ;
            // 轉換失敗（例如玩家在框框裡打了英文字母 "abc"）
            Debug.LogWarning("玩家輸入的不是合法的數字！");
        }
        
    }
    public void initial()
    {
       
        float initValue = PlayerPrefs.GetFloat(setValue, 0.05f);
        slider.value = initValue;
        IF.placeholder.GetComponent<TextMeshProUGUI>().text = initValue.ToString("0.00");

    }


    public void SaveValue()
    {
        PlayerPrefs.Save();
    }
}
