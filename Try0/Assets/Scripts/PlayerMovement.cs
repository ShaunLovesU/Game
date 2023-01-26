using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 movement;

    /*public GameObject lava;
    public GameObject water;
    public GameObject bridge;
    public GameObject stairs;
    public GameObject stepPrefab;
    public GameObject 
    public int numObj = 50;
    public int[] xSign = new int[100];
    public int[] zSign = new int[100];
    public int baseXCoor = -45;
    public int baseZCoor = -45;*/
    //public GameObject player;

    //Rigidbody playerRigidBody = player.AddComponent<Rigidbody>();
    //playerRigidBody.mass = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
