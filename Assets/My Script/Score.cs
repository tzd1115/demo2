using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public TextMeshPro text;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
       text.text = Manager.Instance.level.ToString();
    }
}
