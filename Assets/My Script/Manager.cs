using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
//using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UIElements;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public static Manager Instance { get; private set; }
    private int _level = 0;
    public int level
    {
        get => _level;
        set
        {
            _level = value;
            // 只要有人改了 level 的值，大喇叭立刻广播出去！
            OnlvChange?.Invoke(_level);
        }
    }
    public Action <int>OnlvChange;
    

    public int is_exit = 0;
   
    
    public GameObject []fordest;
    
    public GameObject firstdest;
    public GameObject spawnpoint;

    public GameObject ToIns;


    public GameObject player;
    public PlayerController plyC;
    private async void Awake()
    {
        level = 0;
        //fordest = new GameObject[3];
        // 確保場景中只有一個 GameManager
        if (Instance == null)
        {
            Instance = this;
            // 如果切換場景時不想讓 GameManager 消失，可以取消下一行的註釋
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); // 如果有重複的就刪掉
        }
        if (level == 0)
        {
            //fordest[0] = Instantiate(

            //    Resources.Load("prefeb/noting map").GameObject(),
            //    new Vector3(),
            //    Quaternion.identity

            //);
            //action?.Invoke();
            ResourceRequest rr = Resources.LoadAsync<GameObject>("prefeb/noting map");
            await rr;

            GameObject prefab = rr.asset as GameObject;
             
            var instances = await InstantiateAsync(
            prefab,
            spawnpoint.transform.position,
            spawnpoint.transform.rotation
            );
            fordest[1] = instances[0];
        }
       
        
    }
    void Start()
    {
        
    }
   public IEnumerator WaitForPress(string key, float dur, System.Action Pressed)
    {
        
        float timer = 0f;

        while (dur==0f?true: timer <dur)
        {
            if (dur != 0f) { timer += Time.deltaTime;}

            if (Input.GetKeyDown(key)) 
            {
                yield return null;
                Pressed?.Invoke();

                
                yield break;

            }
                    
             yield return null;
          

        }


    }
   public IEnumerator CountDownAndAction(float second ,System.Action action){

        yield return new WaitForSecondsRealtime(second);

        action?.Invoke();

        yield break;
    } 


}
