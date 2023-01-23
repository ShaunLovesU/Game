using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ViewRotation : MonoBehaviour
{
    //public GameObject cube;//获取角色，不推荐这个做法，建议测试使用
    Quaternion camaeraXAngle;//获取鼠标X轴偏移量当摄像机旋转角度
    Quaternion camaeraYAngle;//获取鼠标Y轴偏移量当摄像机旋转角度
 
 
    void LateUpdate()
    {
        if (Input.GetAxis("Mouse X") > 0.2)//当鼠标X轴偏移量大于0.3时执行，鼠标偏移量过小不执行(玩家也许只是想上下旋转摄像机但是难免会造成X轴偏移，所以当X轴偏移过小时不执行)
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
        }
        else if (Input.GetAxis("Mouse Y") > 0.2 && this.transform.rotation.x < 0.5)
        {
            camaeraYAngle = Quaternion.Slerp(camaeraYAngle, Quaternion.Euler(Input.GetAxis("Mouse Y"), 0, 0), 0.1f);
            transform.rotation *= camaeraYAngle;
        }
        //this.transform.position = (this.transform.rotation * new Vector3(0, 0, -5) + cube.transform.position);//摄像机的位置设置
        //this.transform.LookAt(cube.transform);//摄像机看向角色
    }   
}