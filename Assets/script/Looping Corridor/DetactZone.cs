using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class DetactZone : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spawnpoint;
    public GameObject spawnpointlv0;
    public TextMeshPro level;
    public bool istrue = false;
    public bool walked = false;
    
    void Start()
    {
        
    }

    // Update is called once per frame
   
    private void OnTriggerEnter(Collider other)
    {
        //Destroy(level.gameObject);
        if (other.tag != "Player")
        {
            return;
        }
         
        if (Manager.Instance.is_exit == 0)
        {                       
            foreach (GameObject scene in Manager.Instance.fordest )
            {                             
                    if (scene != transform.parent.gameObject)
                {
                    
                    Destroy(scene);               
                }
                //else
                //{
                //    Manager.Instance.is_exit = 1;
                //    return;
                //}
            }
            
            //if (Manager.Instance.fordest != null)
            //{
            //    Destroy(Manager.Instance.fordest[0]);
            //    //if (Manager.Instance.level == 0) {
            //    //    Destroy(Manager.Instance.fordest1);
            //    //}
            //}
            Manager.Instance.is_exit = 1;
            Debug.Log("go in");
            level.gameObject.SetActive(false);
            return;
        }
        else
        {
            Manager.Instance.is_exit = 0;
            Manager.Instance.fordest[0] = transform.parent.gameObject;
            if (walked) { 
                
                Manager.Instance.level = 0;
                Debug.Log("walk again reset");

            }
            else
            {
                walked = true;
                if (istrue == true)
                {
                    Manager.Instance.level += 1;
                }
                else
                {

                    if (Manager.Instance.level == 0)
                    {
                        Manager.Instance.fordest[1] = Instantiate(Resources.Load("prefeb/map").
                                                      GameObject(), spawnpointlv0.transform.position,
                                                      spawnpointlv0.transform.rotation);
                        Debug.Log("loop");

                        return;
                    }
                    Manager.Instance.level = 0;


                }


                Debug.Log(Manager.Instance.level);
            }
            
        }


        int rt = UnityEngine.Random.Range(0, 3);

        string name = null;
        
        switch (rt)
        {
            case 0:
                name = "1";
                // 執行生成直路的代碼
                break;

            case 1:
                name = "2";
                break;

            //case 2:
            //    name = "map error1";
            //    break;
        }
        foreach (int a in Manager.Instance.MapDic.Keys)
        {
            if (a == rt)
            {
                Manager.Instance.fordest[1] = Instantiate(Manager.Instance.MapDic[rt]
               , spawnpoint.transform.position, spawnpoint.transform.rotation);
                Debug.Log("go out");
            }
        }
        //if (Manager.Instance.level == 0) { name = "map"; }


        //Manager.Instance.fordest[1] = Instantiate(Resources.Load("prefeb/" + name).
        //    GameObject(), spawnpoint.transform.position, spawnpoint.transform.rotation);
        //Debug.Log("go out");
    }
}
