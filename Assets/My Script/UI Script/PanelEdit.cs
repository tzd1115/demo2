using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelEdit : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI word;
    public void EditWord(string newWord)
    {
        word.text = newWord;
    }
   
}
