using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //³æ¨Ò¼Ò¦¡ (Singleton Pattern)
    private static UIManager mInstance = null;
   
    public static UIManager Instance
    {
        get
        {
            //¦pªGmInstance¬Onull¡A¥ý¹Á¸Õ´M§ä³õ´º¤¤ªºUIManagerª«¥ó
            if (mInstance == null)
            {
                mInstance = GameObject.FindObjectOfType<UIManager>();
            }
            //¦pªG³õ´º¤¤§ä¤£¨ì¡A¦A±qResources¸ê®Æ§¨¸ü¤JUIManager Prefab
            if (mInstance == null)
            {
                GameObject prefab = Resources.Load<GameObject>("UIManager");
                if (prefab != null)
                {
                    GameObject obj = GameObject.Instantiate(prefab);
                    mInstance = obj.GetComponent<UIManager>();
                }
                else
                {
                    Debug.LogError("UIManager Prefab is NULL!");
                }
            }
            return mInstance;
        }
    }

    //³õ´º¦WºÙ±`¼Æ
    public const string START_SCENE = "開始遊戲";
    public const string BATTLE_SCENE = "Start";
    public const string FINISH_SCENE = "結束游戲";

    //©Ò¦³UIªºª«¥ó¡A¨ä¹ê¸û¦nªº°µªk¬O¥ÎDictionary¨ÓºÞ²z
    //¦ý³oºØ¼gªk¸ûª½Æ[¡A¾A¦X·s¤â¾\Åª
    public MenuPanel menuPanel;
    public LoadingPanel loadingPanel;
    public FinishPanel finishPanel;
    public GameObject tips;
    public GameObject settingPanel;
    public void Init()
    {
        //½T«OUIManagerª«¥ó¦b³õ´º¤Á´«®É¤£³Q¾P·´
        DontDestroyOnLoad(this.gameObject);

        //´M§äEventSystemª«¥ó
        GameObject eventSystem = GameObject.Find("EventSystem");
        if(eventSystem == null)
        {
            //ª½±µ«Ø¥ßªÅª«¥ó¡A¨Ã±¾¸üEventSystem©MStandaloneInputModule¤¸¥ó
            eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        //ÁôÂÃ©Ò¦³­±ªO
        menuPanel.Hide();
        loadingPanel.Hide();
        finishPanel.Hide();
    }
    void Update()
    {
       WaitForPause();
    }
    void WaitForPause()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (settingPanel.GetComponent<UIPanelBase>().IsActive())
        {
            Debug.Log(2);
            //Manager.Instance.player.GetComponent<PlayerController>().applySetting();
            menuPanel.OnClick_back();
            return;
        }
        if (!menuPanel.IsActive())
        {

            Cursor.visible = true;
            menuPanel.Show();
            Cursor.lockState = CursorLockMode.None;
            Debug.Log(1);

        }
        else
        {
           
           
               Debug.Log(3);
               menuPanel.Hide();
               Cursor.lockState = CursorLockMode.Locked;
               Cursor.visible = false;
        }
    }
}
