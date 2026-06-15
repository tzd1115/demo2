using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

//using Unity.VisualScripting;
using UnityEngine;

public class CameraShift : MonoBehaviour
{
    GameObject panel;
    Coroutine coroutine;
    Vector3 dfposition = new Vector3(0,1f,0.1f);
    bool viewed = false;
    public Action action;
    private void Start()
    {
        Vector3 dfposition = Manager.Instance.player.GetComponentInChildren<Camera>().
            transform.localPosition;
        panel = UIManager.Instance.tips;
    }
    
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.tag != "Player") { return; }
       
            PressToView();       
    }
    private void OnTriggerExit(Collider obj)
    {
        if (obj.tag !="Player") { return; }
        StopCoroutine(coroutine);
        StopAllCoroutines();
        panel.gameObject.SetActive(false);
    }

    void PressToView()
    {
        if (!viewed)
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
                action?.Invoke();
               
                panel.GetComponent<PanelEdit>().EditWord("點擊F退出");
                Manager.Instance.player.GetComponent<Cameraresistence>().enabled = true;
                StopCoroutine(coroutine);

                viewed = !viewed;
                PressToView();
            }
            )
            );          
        }
        else
        {
            coroutine = StartCoroutine(Manager.Instance.WaitForPress("f", 0f, Pressed: () =>
            {
                Manager.Instance.player.GetComponent<Cameraresistence>().enabled = false;
                Manager.Instance.player.GetComponentInChildren<Camera>().
                transform.localRotation = Quaternion.identity;

                Manager.Instance.player.GetComponentInChildren<Camera>().
                transform.localPosition = dfposition;

                Manager.Instance.player.GetComponent<PlayerController>().enabled = true;
                StopCoroutine(coroutine);
                Debug.Log("1");

                viewed = !viewed;
                PressToView();
            }
            )
            );         
        }
    }
}
