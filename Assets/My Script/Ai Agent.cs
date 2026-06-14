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
    private Transform playerTransform;
    public bool linChase = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 dfposition = Manager.Instance.player.GetComponentInChildren<Camera>().
            transform.localPosition;

        if (Manager.Instance != null && Manager.Instance.player != null)
        {
            playerTransform = Manager.Instance.player.transform;
        }
        else
        {
            // 备用方案
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }

        if (playerTransform == null)
        {
            Debug.LogError("【LinearChaser】未能在场景中找到玩家，请检查 Manager 里的赋值或 Player 的 Tag！");
        }
    }
    //private void Update()
    //{
    //    if (stop) { return; }
    //    if (waypoints.Length == 0 || currentPointIndex >= waypoints.Length) return;

    //    // ⭕ 正確寫法：目標點直接就是路點的位置，但我們把高度「鎖定」在跟 Rigidbody 目前一樣的高度
    //    Vector3 targetPosition = waypoints[currentPointIndex].position;
    //    targetPosition.y = transform.position.y; // 👈 直接這樣鎖定，最安全、絕對不拉扯！

    //    // 計算下一影格物理要前進的新位置
    //    Vector3 newPosition = Vector3.MoveTowards(rb.position, targetPosition, speed * Time.deltaTime);

    //    // 讓物理引擎把物體推過去
    //    rb.MovePosition(newPosition);

    //    // 轉向邏輯
    //    Vector3 direction = targetPosition - rb.position;
    //    if (direction != Vector3.zero)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(direction);
    //        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.deltaTime));
    //    }

    //    // 抵達檢查
    //    if (Vector3.Distance(rb.position, targetPosition) < 0.1f)
    //    {
    //        if (loop)
    //        {
    //            currentPointIndex = (currentPointIndex + 1) % waypoints.Length;
    //            return;
    //        }
    //        ++currentPointIndex;
    //    }
    //}
    // ⚠️ 只要涉及 Rigidbody 物理移動，程式碼一定要寫在 FixedUpdate 裡！
    void FixedUpdate()
    {
        
        if (stop) { return; }


        if (waypoints.Length == 0 || currentPointIndex >= waypoints.Length)
        {
            if (linChase) {

                return;

            }
            return;
        }
        // ⭕ 正確寫法：目標點直接就是路點的位置，但我們把高度「鎖定」在跟 Rigidbody 目前一樣的高度
        Vector3 targetPosition = waypoints[currentPointIndex].position;
        targetPosition.y = rb.position.y; // 👈 直接這樣鎖定，最安全、絕對不拉扯！

        // 計算下一影格物理要前進的新位置
        Vector3 newPosition = Vector3.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);

        // 讓物理引擎把物體推過去
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
    void Update()
    {
        if (waypoints.Length == 0 || currentPointIndex >= waypoints.Length &&linChase)
        {
            LinearChaser();
        }
    }
    void LinearChaser()
    {
        if (playerTransform == null) return;

        // 1. 计算目标位置，但【锁定 Y 轴】（使用鬼自己的 Y 轴高度）
        // 这样可以极其完美地防止鬼因为玩家跳跃、下蹲而跟着起飞或陷地
        Vector3 targetPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);

        // 2. 让鬼的身体永远死死盯着玩家的方向
        transform.LookAt(targetPosition);

        // 3. 核心：使用 MoveTowards 进行纯坐标线性平滑移动
        // 这种移动不受任何物理碰撞体挤压的影响，绝对稳定
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // 4. 距离判定，走廊很窄，只要距离够近就判定抓到
        //if (Vector3.Distance(transform.position, targetPosition) <= catchDistance)
        //{
        //    CatchPlayer();
        //}
    }

    private void CatchPlayer()
    {
        Debug.Log("【大叔回头/鬼贴脸】抓到玩家了！");

        // 触发你的结束/重置逻辑
        // Manager.Instance.level = 0; 

        // 停止脚本，防止一瞬间重复触发
        this.enabled = false;
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
        if (oth.gameObject.layer!=LayerMask.NameToLayer("Player")) { return; }
        Debug.Log(oth.gameObject.name);
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
        CatchPlayer();
        StartCoroutine(Manager.Instance.CountDownAndAction(3f, action: () =>
        {

            Manager.Instance.player.GetComponentInChildren<Camera>().
            transform.localRotation = Quaternion.identity;

            Manager.Instance.player.GetComponentInChildren<Camera>().
            transform.localPosition = dfposition;

            Manager.Instance.player.GetComponent<PlayerController>().enabled = true;
            Manager.Instance.player.GetComponentInChildren<PlayerController>().ShowMesh();
            

        }));

    }
}