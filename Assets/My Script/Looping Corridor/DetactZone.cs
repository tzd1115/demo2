using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Schema;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
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
       // Debug.Log($"【碰撞触发】触发物体: {other.name} | 挂载节点: {other.transform.parent?.name} | 当前游戏帧: {Time.frameCount}");
        //Debug.Log(other.name);
        //Destroy(level.gameObject);
        if (other.tag != "Player") return;
       

        if (Manager.Instance.is_exit == 0)
        {
            GoIn();
            return;     
        }
        else
        {
            if (!GoOut()) return; 
        }

        _=generateAsync();

    }
    private void OnTriggerExit(Collider other)
    {
       
        if (other.tag != "Player") return;
        Vector3 playerForward = other.transform.forward;

        // 获取空气墙面向的方向
        Vector3 wallForward = transform.forward;

        // 计算两者的夹角
        float angle = Vector3.Angle(playerForward, wallForward);

        if (angle < 90f)
        {
            Debug.Log("🚶 离开时，玩家面向前方，判定为【通过】");
        }
        else
        {
            Debug.Log("🔄 离开时，玩家面向后方，判定为【回头】");
        }
    }
    void GoIn()
    {
        foreach (GameObject scene in Manager.Instance.fordest)
        {
            if (scene != transform.parent.gameObject)
            {
                Destroy(scene);
            }
        }
        Manager.Instance.is_exit = 1;
        Debug.Log("go in");
        level.gameObject.SetActive(false);
    }

    bool GoOut()
    {

        Manager.Instance.is_exit = 0;
        Manager.Instance.fordest[0] = transform.parent.gameObject;
        if (walked)
        {
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
                if (Manager.Instance.level == 0 && this.name!= "front gate")
                {
                    Lv0generate();
                    return false;
                }
            Manager.Instance.level = 0;
            }
            Debug.Log(Manager.Instance.level);
        }
        return true;
    }
    void generate()
    {
        int rt = UnityEngine.Random.Range(0, 5);

        string name = null;

        switch (rt)
        {
            case 0:
                name = "map";
                // 執行生成直路的代碼
                break;
            case 1:
                name = "map error";
                break;

            case 2:
                name = "bad guy normal map";
                break;

            case 3:
                name = "bad guy cheasing map"; 
                break;
            case 4:
                name = "c map";
                break;
        }
        //if (Manager.Instance.level == 0) { name = "map"; }
        
        Manager.Instance.fordest[1] = Instantiate(Resources.Load("prefeb/" + name).
        GameObject(), spawnpoint.transform.position, spawnpoint.transform.rotation);

        Debug.Log("go out");
    }
    async Task generateAsync()
    {
        int rt = UnityEngine.Random.Range(0, 5);

        string name = null;

        switch (rt)
        {
            case 0:
                name = "map";
                // 執行生成直路的代碼
                break;
            case 1:
                name = "map error";
                break;

            case 2:
                name = "bad guy normal map";
                break;

            case 3:
                name = "bad guy cheasing map";
                break;
            case 4:
                name = "c map";
                break;
        }
        //if (Manager.Instance.level == 0) { name = "map"; }
        ResourceRequest loadrequestmap = Resources.LoadAsync<GameObject>("prefeb/" + name);

        while (!loadrequestmap.isDone)
        {
            await Task.Yield();
        }

        GameObject go = loadrequestmap.asset as GameObject;

        var instantiateOperation = InstantiateAsync(go
        , spawnpoint.transform.position, spawnpoint.transform.rotation);
        
        while (!instantiateOperation.isDone)
        {
            await Task.Yield();
        }
        Manager.Instance.fordest[1] = instantiateOperation.Result[0];

        Debug.Log("go out");
    }

    void Lv0generate()
    {
        Manager.Instance.fordest[1] = Instantiate(Resources.Load("prefeb/noting map").
                                      GameObject(), spawnpointlv0.transform.position,spawnpointlv0.transform.rotation);
        Debug.Log("loop");
    }
}
