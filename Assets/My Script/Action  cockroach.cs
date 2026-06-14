using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class ActionCockroach: MonoBehaviour
{
    public GameObject go;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<CameraShift>().action = action;
    }

    // Update is called once per frame
    void action(){
        StartCoroutine(Manager.Instance.CountDownAndAction(1f, action:()=>
        {
            go.SetActive(true);
            go.GetComponent<AudioSource>().Play();
        }));
    }

    
}
