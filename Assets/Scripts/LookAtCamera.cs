using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // 前两种会有倾斜
    // 后两种不会有角度倾斜
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }

    [SerializeField] private Mode mode;
    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:// 看向摄像机
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:// 看向摄像机位置相反的点
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                // 点 + 向量 = 点
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:// 与摄像机相向
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:// 与摄像机相对
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
