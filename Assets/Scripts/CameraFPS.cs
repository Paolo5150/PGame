using UnityEngine;

public class CameraFPS : MonoBehaviour
{
    public float MouseSensitivity = 10.0f;
    public GameObject MainCamera;

    private float m_xRotation = 0.0f;
    private float m_YRotation = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        m_xRotation -= mouseY;
        m_xRotation = Mathf.Clamp(m_xRotation, -90.0f, 90.0f);

        m_YRotation += mouseX;

        MainCamera.transform.localRotation = Quaternion.Euler(m_xRotation, 0, 0);
        transform.localRotation = Quaternion.Euler(0, m_YRotation, 0);
    }
}
