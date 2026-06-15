using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpHeight = 1.5f;
    public float gravity = -19.62f;

    [Header("視角旋轉")]
    public Transform cameraTransform;
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    [Header("鏡頭晃動 (Head Bobbing)")]
    public float bobFrequency = 2.0f;       // 晃動頻率 (與速度相關)
    public float bobHorizontalAmplitude; // 水平晃幅度0.05f;
    public float bobVerticalAmplitude;  // 垂直晃幅度 0.05f; 
    [Range(0, 1)] public float headBobSmoothing = 0.1f; // 平滑度

    // 內部變量
    private CharacterController controller;
    private Vector3 velocity;
    public bool isGrounded;
    private Vector3 cameraDefaultPos;
    private float bobTimer;

    void Awake()
    {
        // 🙋‍♂️ 玩家一出生，立刻把自己登记到全局管理器里
       
        if (Manager.Instance != null)
        {
            if (!Manager.Instance.player)
            {
                Manager.Instance.player = this.gameObject;
            }
        }
    }
    void Start()
    {
        
        applySetting();

        controller = GetComponent<CharacterController>();

        // 記錄相機在角色內的初始位置
        if (cameraTransform != null)
            cameraDefaultPos = cameraTransform.localPosition;

        // 鎖定滑鼠
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        // 1. 環境檢查
        isGrounded = controller.isGrounded;
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (Cursor.visible) { return; }
        // 2. 處理旋轉 (視角控制)
        HandleRotation();

        // 3. 處理移動 (WASD + 跳躍)
        HandleMovement();

        // 4. 處理鏡頭晃動
        ApplyHeadBob();
       
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 判斷是否在跑步
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // 跳躍控制
        //if (Input.GetButtonDown("Jump") && isGrounded)
        //{
        //    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        //}

        //// 應用重力
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    void ApplyHeadBob()
    {
        if (cameraTransform == null) return;

        // 獲取鍵盤輸入的強度 (不再依賴 controller.velocity)
        float inputMagnitude = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).magnitude;

        // 如果沒按鍵盤，或不在地面，就回歸原位
        if (!isGrounded || inputMagnitude < 0.1f)
        {
            bobTimer = 0;
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraDefaultPos, headBobSmoothing);
            return;
        }

        // 判斷當前是走路還是跑步速度來決定晃動頻率
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        bobTimer += Time.deltaTime * currentSpeed * bobFrequency;

        float newX = cameraDefaultPos.x + Mathf.Cos(bobTimer * 0.5f) * bobHorizontalAmplitude;
        float newY = cameraDefaultPos.y + Mathf.Sin(bobTimer) * bobVerticalAmplitude;

        cameraTransform.localPosition = new Vector3(newX, newY, cameraDefaultPos.z);
    }
    public void applySetting()
    {
        bobHorizontalAmplitude = PlayerPrefs.GetFloat("shakesetting", 0.05f);
        bobVerticalAmplitude = bobHorizontalAmplitude;
        mouseSensitivity = PlayerPrefs.GetFloat("mousesensitive", 500f);
    }
    public void HideMesh()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
    public void ShowMesh()
    {
        GetComponent<MeshRenderer>().enabled = true ;
    }
    public void die()
    {
        UIManager.Instance.loadingPanel.LoadScene("Start");
    }
}