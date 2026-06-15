using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public TextMeshPro text;
    // Start is called before the first frame update

    
    // Update is called once per frame
    void Start()
    {
        UpdateText(Manager.Instance.level);
        Manager.Instance.OnlvChange +=UpdateText;
    }
    void UpdateText(int textx)
    {
        text.text = textx.ToString();
    }
    void OnDestroy()
    {
        // 3. 安全防护：物体销毁时，取消订阅，防止内存泄漏
        if (Manager.Instance != null)
        {
            Manager.Instance.OnlvChange -= UpdateText;
        }
    }
}
