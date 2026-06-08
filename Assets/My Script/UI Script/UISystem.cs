using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas canvas;
    public GameObject pausePanel;

    public static UISystem UIinstance;

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
    private void Update()
    {

        if (Input.GetKeyDown("q"))
        {

            if (!pausePanel.activeInHierarchy)
            {
                Manager.Instance.player.GetComponent<PlayerController>().enabled = false;
                pausePanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Manager.Instance.player.GetComponent<PlayerController>().enabled = true;
                Manager.Instance.player.GetComponent<PlayerController>().applySetting();
                pausePanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

            }
        }
    }
}
