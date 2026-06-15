using UnityEngine;

public class WaitToSpawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        StartCoroutine(Manager.Instance.CountDownAndAction(5f, action: () =>
        {
            Debug.Log("after 3 s");
            GetComponent<AiAgent>().enabled = true;
            StopAllCoroutines();
            this.enabled = false;
        }));

    }

    // Update is called once per frame
    
}
