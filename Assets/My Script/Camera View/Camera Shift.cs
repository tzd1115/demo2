using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraShift : MonoBehaviour
{
    GameObject panel;
    Coroutine coroutine;
    Vector3 dfposition = new Vector3(0,1,0);

    private void Start()
    {

        panel = UISystem.UIinstance.canvas.transform.Find("tips").gameObject;
    }
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.tag != "Player") { return; }

        PressToView();
        //if () { return; }
        
        
    }
    private void OnTriggerExit(Collider obj)
    {
        if (obj.tag !="Player") { return; }
        StopCoroutine(coroutine);
        panel.gameObject.SetActive(false);
    }
    
    void PressToView()
    {
        panel.GetComponent<PanelEdit>().EditWord("點擊F查看");
        panel.SetActive(true);

        coroutine = StartCoroutine(Manager.Instance.WaitForPress("f", 0f, Pressed: () =>
        {
            Manager.Instance.player.GetComponent<PlayerController>().enabled = false;

            Manager.Instance.player.GetComponentInChildren<Camera>().
            transform.position = transform.position;

            Manager.Instance.player.GetComponentInChildren<Camera>().
            transform.rotation = transform.rotation;

            panel.GetComponent<PanelEdit>().EditWord("點擊R退出");

            StartCoroutine(Manager.Instance.WaitForPress("r", 0f, Pressed: () =>
            {
                Manager.Instance.player.GetComponent<PlayerController>().enabled = true;
                Manager.Instance.player.GetComponentInChildren<Camera>().
                transform.rotation = Quaternion.identity;

                Manager.Instance.player.GetComponentInChildren<Camera>().
                transform.position = dfposition;
              
                
                PressToView();
            }   
                )
            );
        }
        )
        );
        
    }  
}
