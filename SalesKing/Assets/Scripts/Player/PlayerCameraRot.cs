using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraRot : MonoBehaviour
{
    [SerializeField] private float mouseSpeed = 8f; //ȸ���ӵ�
    private float mouseY = 0f; //���Ʒ� ȸ������ ���� ����

    public bool isCameraLocked;

    void Update()
    {
        if (!isCameraLocked)
        {
            mouseY += Input.GetAxis("Mouse Y") * mouseSpeed;
            mouseY = Mathf.Clamp(mouseY, -50f, 30f);
            this.transform.localEulerAngles = new Vector3(-mouseY, 0, 0);
        }
        
    }
}