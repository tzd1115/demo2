using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AiAgent : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5f;

    public int currentPointIndex = 0;
    private Rigidbody rb;
    public bool loop = false;
    public bool stop = false;
    Vector3 dfposition;
    public Transform point;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 dfposition = Manager.Instance.player.GetComponentInChildren<Camera>().
            transform.localPosition;
    }

    // ⚠️ 只要涉及 Rigidbody 物理移動，程式碼一定要寫在 FixedUpdate 裡！
    void FixedUpdate()
    {
        if (stop) { return; }
        if (waypoints.Length == 0 ||currentPointIndex >= waypoints.Length) return;

        Vector3 targetPosition = waypoints[currentPointIndex].position + new Vector3(0,transform.position.y,0);

        // 計算下一影格物理要前進的新位置
        Vector3 newPosition = Vector3.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);

        // 讓物理引擎把物體推過去（會正常計算碰撞，不會穿牆）
        rb.MovePosition(newPosition);

        // 轉向邏輯
        Vector3 direction = targetPosition - rb.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));
        }

        // 抵達檢查
        if (Vector3.Distance(rb.position, targetPosition) < 0.1f)
        {
            if (loop)
            {
                currentPointIndex = (currentPointIndex + 1) % waypoints.Length;
                return;
            }
            ++currentPointIndex;
        }

        
    }
    public void Stop()
    {
        stop=true;
    }
    public void StopStop()
    {
        stop = false;
    }

    private void OnTriggerEnter(Collider oth)
    {
        if (!oth.transform.root.CompareTag("Player")) { return; }

        OnYourFace();
    }
    void OnYourFace()
    {
        Manager.Instance.player.GetComponent<PlayerController>().enabled = false;

        Manager.Instance.player.GetComponentInChildren<Camera>().
        transform.position = point.position;

        Manager.Instance.player.GetComponentInChildren<Camera>().
        transform.rotation = point.rotation;
        Manager.Instance.player.GetComponentInChildren<PlayerController>().HideMesh();
        Stop();
        StartCoroutine(Manager.Instance.CountDownAndAction(3f, action: () =>
        {

            Manager.Instance.player.GetComponentInChildren<Camera>().
            transform.localRotation = Quaternion.identity;

            Manager.Instance.player.GetComponentInChildren<Camera>().
            transform.localPosition = dfposition;

            Manager.Instance.player.GetComponent<PlayerController>().enabled = true;
            Manager.Instance.player.GetComponentInChildren<PlayerController>().ShowMesh();
            StopStop();

        }));

    }
}