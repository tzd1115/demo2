using UnityEngine;

public class WaitToSpawn : MonoBehaviour
{
    public MonoBehaviour ScriptWantToActive;
    public float activeAfterSecond;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        StartCoroutine(Manager.Instance.CountDownAndAction(activeAfterSecond, action: () =>
        {
            Debug.Log("after?[]");
            ScriptWantToActive.enabled = true;
            StopAllCoroutines();
            this.enabled = false;
        }));

    }

    // Update is called once per frame
    
}
