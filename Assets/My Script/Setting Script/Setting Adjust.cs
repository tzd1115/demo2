using Newtonsoft.Json.Linq;
using TMPro;
using Unity.AI.Assistant;

//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingAdjust : MonoBehaviour
{
    public Slider slider;
    public TMP_InputField IF;
    public string ValueName;
    public float defValue;
    public float minValue;
    public float maxValue;
    public string Xiaoshudian;

    // 監聽：當滑桿數值改變時，自動去執行 OnVolumeChanged 這個 Function
    // 它會自動把當前的數值（float）傳過去

    public virtual void Awake()
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

    public virtual void OnSliderValueChange(float value)
    {
       // IF.GetComponent<TextMeshProUGUI>().text = value.ToString(Xiaoshudian);
        IF.placeholder.GetComponent<TextMeshProUGUI>().text = value.ToString(Xiaoshudian);
        PlayerPrefs.SetFloat(ValueName, value);
        SaveValue();
    }
    
    public virtual void OnInputFieldEndEdit(string text)
    {
        // 嘗試把字串轉換成 float[Range (0f,0.1f)]
        if (float.TryParse(text, out float result))
        {
            if (result > maxValue)
            {

                result = maxValue;
                IF.text = result.ToString();
            }
            if (result < minValue)
            {
                result = minValue;
                IF.text = result.ToString();
            }
            //IF.text = Mathf.Clamp(result,0f,0.1f).ToString();
            slider.value = result; ;
            PlayerPrefs.SetFloat(ValueName, result);
            SaveValue();
          
            IF.text = "";
            //// 轉換成功！
            //return Mathf.Clamp(result, 0f, 0.1f);

            //Debug.Log($"成功拿到 float 數值: {result}");
        }
        else
        {

            // 轉換失敗（例如玩家在框框裡打了英文字母 "abc"）
            // Debug.LogWarning("玩家輸入的不是合法的數字！");
        }


    }
    public virtual void initial()
    {

        float initValue = PlayerPrefs.GetFloat(ValueName, defValue);
        slider.maxValue = maxValue;
        slider.minValue = minValue;
        OnSliderValueChange(initValue);
        OnInputFieldEndEdit(initValue.ToString(Xiaoshudian));

    }


    public void SaveValue()
    {
        PlayerPrefs.Save();
        if (Manager.Instance)
        { Manager.Instance.player.GetComponent<PlayerController>().applySetting(); }
    }
}
