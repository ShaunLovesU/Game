using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 5, -7);

    // Update is called once per frame
    void LateUpdate()
    {
     transform.position = player.transform.position + offset;   
    }
}
