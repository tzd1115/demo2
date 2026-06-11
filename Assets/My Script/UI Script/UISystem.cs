using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas canvas;
    public GameObject pausePanel;
    bool isOff = false;
    public static UISystem UIinstance;
    Coroutine coroutine;
    private void Awake()
    {
        if (UIinstance == null)
        {
            UIinstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    private void Start()
    {
       
        
    }
    private void Update()
    {
        WaitForPause();

    }
    void WaitForPause()
    {
         if (!Input.GetKeyDown("q")) return;

         if (!pausePanel.activeInHierarchy)
         {

             Cursor.visible = true;
             pausePanel.SetActive(true);
             Cursor.lockState = CursorLockMode.None;
                
         }
         else
         {      
             Manager.Instance.player.GetComponent<PlayerController>().applySetting();
             pausePanel.SetActive(false);
             Cursor.lockState = CursorLockMode.Locked;
             Cursor.visible = false;           

         }
    }
        
        
}
