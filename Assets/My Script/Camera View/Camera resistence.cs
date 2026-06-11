using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraresistence : MonoBehaviour
{
    [Header("視角旋轉")]
    public Camera camera;
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    private float yRotation = 0f;
    public float currentRotationY;
    // Start is called before the first frame update
    
    void OnEnable()
    {

        currentRotationY = camera.transform.eulerAngles.y;
        yRotation = currentRotationY;

    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.visible) { return; }
        HandleRotation();
        
    }
    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -5f, 5f);

        yRotation = Mathf.Clamp(yRotation, currentRotationY + -10f, currentRotationY + 10f);

        Debug.Log(currentRotationY);

        camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        
                
    }
}
