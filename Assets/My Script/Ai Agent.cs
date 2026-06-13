using UnityEngine;

public class AiAgent : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5f;

    public int currentPointIndex = 0;
    private Rigidbody rb;
    public bool loop = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // ⚠️ 只要涉及 Rigidbody 物理移動，程式碼一定要寫在 FixedUpdate 裡！
    void FixedUpdate()
    {
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
}