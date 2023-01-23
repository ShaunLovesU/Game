using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 movement;

    public GameObject lava;
    public GameObject water;
    public GameObject bridge;
    public GameObject stairs;
    public GameObject stepPrefab;
    public int numObj = 50;
    public int[] xSign = new int[100];
    public int[] zSign = new int[100];
    public int baseXCoor = -45;
    public int baseZCoor = -45;
//jump control
    public Vector3 jump;
    public float jumpForce = 2.0f;
    public bool isGrounded;
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        //jump init
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);

        System.Random rdm = new System.Random();
        //which spot is occupied
        int[] occupied = new int[100]; 
        for(int i = 0; i < 100; i++){
            occupied[i] = 0;
            xSign[i] = 1;
            zSign[i] = 1;
            if(i%10 == 0 || i % 1 == 0 || i % 2 == 0 || i % 3 == 0 || i % 4 == 0 ){
                xSign[i] = -1;
            }
            if(i / 10 == 5 || i / 10 == 6 || i / 10 == 7 || i / 10 == 8 || i / 10 == 9 ){
                zSign[i] = -1;
            }
        }
        int temp;
        int count = 0;
        //Debug.Log("before for loop, numObj: " + numObj);
        for(int i = 0; i < numObj; i++){
            temp = rdm.Next(0,100);
            while(occupied[temp] == 1){
                temp = rdm.Next(0,100);
            }
            occupied[temp] = 1;
            count++;
        }
        /*Debug.Log("occupied: "+ numObj +" "+ count);
        for(int i = 0; i < 100; i++){
            if(occupied[i] == 1){
                Debug.Log(i+" ");
            }
        }*/

        //Type: 1 = water, 2 = lava, 3 = stairs, 4 = bridge, 5 = vertical bridge, 6 = horizontal bridge
        int[] obstacleType = new int[100];
        for(int i = 0; i < 100; i++){
            if(occupied[i] == 0){
                continue;
            }
            obstacleType[i] = rdm.Next(1,5);

            //to prevent generating too many stairs
            if(obstacleType[i] == 3){
                temp = CheatRdm();
                if(temp != 5){
                    obstacleType[i] = 0;
                    occupied[i] = 0;
                }
            }

            if(obstacleType[i] == 4){
                if(i % 10 == 0 || i % 9 == 0 || i < 10 || i > 89){
                    occupied[i] = 0;
                    obstacleType[i] = 0;
                }else{
                    temp = rdm.Next(0,2);
                    if(temp == 0){
                        occupied[i-1] = 0;
                        occupied[i+1] = 0;
                        obstacleType[i-1] = 0;
                        obstacleType[i+1] = 0;
                        obstacleType[i] = 5;
                    }else{
                        occupied[i-10] = 0;
                        occupied[i+10] = 0;
                        obstacleType[i-10] = 0;
                        obstacleType[i+10] = 0;
                        obstacleType[i] = 6;
                    }
                }
            }
            //if(obstacleType[i] == 3){

            //}
        }
        for(int i = 0; i < 100; i++){
            if(occupied[i] == 1){
                int xCoor = ((i % 10) * 10 + baseXCoor);
                int zCoor = ((i - i%10) + baseZCoor);

                Debug.Log("i: "+ i +", xSign: "+xSign[i]+", zSign: " + zSign[i]+" ,xCoor: "+ xCoor+" ,zCoor: "+zCoor);

                Vector3 pos = new Vector3(xCoor, 1, zCoor);
                Quaternion rot = new Quaternion ();

                if (obstacleType[i] == 1){
                    GameObject fab = Instantiate(water, pos, rot);
                    //water.transform.position = new Vector3(xCoor, 0, zCoor);
                }
                if (obstacleType[i] == 2){
                    GameObject fab = Instantiate(lava, pos, rot);
                    //lava.transform.position = new Vector3(xCoor, 0, zCoor);
                }
                if (obstacleType[i] == 3){
                    //GameObject fab = Instantiate(stepPrefab, pos, rot);
                    stairs.transform.position = new Vector3(xCoor, 0, zCoor);
                    BuildStairs();
                }
                if (obstacleType[i] == 5){
                    GameObject fab = Instantiate(bridge, pos, rot);
                    fab.transform.Rotate(90, 0, 0, Space.World);
                    //bridge.transform.position = new Vector3(xCoor, 0, zCoor);
                }
                if (obstacleType[i] == 6){
                    GameObject fab = Instantiate(bridge, pos, rot);
                    fab.transform.Rotate(90, 90, 0, Space.World);
                    //bridge.transform.position = new Vector3(xCoor, 0, zCoor);
                }
            }
        }
    }

    int CheatRdm()
    {
        System.Random random = new System.Random();
        if (random.NextDouble() < 0.9)
            return 5;

        return random.Next(1, 5);
    }

    void BuildStairs(){
        Vector3 centerBottom = stairs.transform.position;
		float radius = 5f;
		float angleDegrees = 10f;
		float angleRadians = angleDegrees * Mathf.PI / 180f;
		for (var i = 0; i < 100; i++) {
			float x = radius * Mathf.Cos (i * angleRadians);
			float z = radius * Mathf.Sin (i * angleRadians);
			Vector3 pos = new Vector3(centerBottom.x + x, centerBottom.y + i * .2f, centerBottom.z + z);
			Quaternion rot = new Quaternion ();
			GameObject step = Instantiate(stepPrefab, pos, rot);
			step.transform.Rotate (0, -i * angleDegrees, 0);
		}
    }
    void OnCollisionStay(){
        isGrounded = true;
    }
    // Update is called once per frame
    void Update()
    {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime;
        transform.Translate(movement);
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){

        rb.AddForce(jump * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        }
    }
}
