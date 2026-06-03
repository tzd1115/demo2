using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public static Manager Instance { get; private set; }
    public int level = 0;
    public int is_exit = 0;
    public GameObject []fordest;
    public Dictionary<int, GameObject> MapDic = new Dictionary<int, GameObject>();
    public GameObject firstdest;
    public GameObject spawnpoint;
    private void Awake()
    {

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
            fordest[1] = Instantiate(

                Resources.Load("prefeb/map").GameObject(),
                spawnpoint.transform.position,
                spawnpoint.transform.rotation

                );
        }
    }
    void Start()
    {
        
    }
    
}
