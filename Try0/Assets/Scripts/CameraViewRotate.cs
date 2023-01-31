using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewRotate : MonoBehaviour
{
    //public GameObject cube;
    Quaternion camaeraXAngle;//获取鼠标X轴偏移量当摄像机旋转角度
    Quaternion camaeraYAngle;//获取鼠标Y轴偏移量当摄像机旋转角度

    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public float m_minimumX = -360f;
    public float m_maximumX = 360f;
    public float m_minimumY = -90f;
    public float m_maximumY = 90f;

    public float m_sensitivityX = 10f;
    public float m_sensitivityY = 10f;

    public RotationAxes m_axes = RotationAxes.MouseXAndY;

    float m_rotationY = 0f;

    void Start(){
        if(GetComponent<Rigidbody>()){
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }
 
    void LateUpdate()
    {
        /*if (Input.GetAxis("Mouse X") > 0.2)//当鼠标X轴偏移量大于0.3时执行，鼠标偏移量过小不执行(玩家也许只是想上下旋转摄像机但是难免会造成X轴偏移，所以当X轴偏移过小时不执行)
        {
            camaeraXAngle = Quaternion.Slerp(camaeraXAngle, Quaternion.Euler(0, Input.GetAxis("Mouse X"), 0), 0.1f);      //平滑设置旋转角度
            transform.rotation *= camaeraXAngle;//旋转
        }
         else if (Input.GetAxis("Mouse X") < -0.2)
        {
           camaeraXAngle = Quaternion.Slerp(camaeraXAngle, Quaternion.Euler(0, Input.GetAxis("Mouse X"), 0), 0.1f);
            transform.rotation *= camaeraXAngle;
        }
         if (Input.GetAxis("Mouse Y") < -0.2 && this.transform.rotation.x > 0.15)
        {
            camaeraYAngle = Quaternion.Slerp(camaeraYAngle, Quaternion.Euler(Input.GetAxis("Mouse Y"), 0, 0), 0.1f);
            transform.rotation *= camaeraYAngle;
            Debug.Log(camaeraYAngle.GetType());
        }
        else if (Input.GetAxis("Mouse Y") > 0.2 && this.transform.rotation.x < 0.5)
        {
            camaeraYAngle = Quaternion.Slerp(camaeraYAngle, Quaternion.Euler(Input.GetAxis("Mouse Y"), 0, 0), 0.1f);
            transform.rotation *= camaeraYAngle;
        }
        //this.transform.position = (this.transform.rotation * new Vector3(0, 0, -5) + cube.transform.position);//摄像机的位置设置
        //this.transform.LookAt(cube.transform);//摄像机看向角色*/
        if (m_axes == RotationAxes.MouseXAndY) {
            float m_rotationX = transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * m_sensitivityX;
            m_rotationY += Input.GetAxis ("Mouse Y") * m_sensitivityY;
            m_rotationY = Mathf.Clamp (m_rotationY, m_minimumY, m_maximumY);

            transform.localEulerAngles = new Vector3 (-m_rotationY, m_rotationX, 0);
        } else if (m_axes == RotationAxes.MouseX) {
            transform.Rotate (0, Input.GetAxis ("Mouse X") * m_sensitivityX, 0);
        } else {
            m_rotationY += Input.GetAxis ("Mouse Y") * m_sensitivityY;
            m_rotationY = Mathf.Clamp (m_rotationY, m_minimumY, m_maximumY);

            transform.localEulerAngles = new Vector3 (-m_rotationY, transform.localEulerAngles.y, 0);
        }
    }   
}
