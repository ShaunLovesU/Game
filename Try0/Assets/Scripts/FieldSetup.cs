using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSetup : MonoBehaviour
{

    public GameObject poison;
    public GameObject water;
    public GameObject stairs;
    public GameObject stepPrefab;
    public GameObject floor;
    public GameObject platformFloor;
    public GameObject wall;
    public GameObject bridge;
    public GameObject connector;

    public int baseXCoor = -20;
    public int baseZCoor = -20;
    public int gapSize = 300;
    public int levelHeight = 20;


    // Start is called before the first frame update
    void Start()
    {
        System.Random rdm = new System.Random();

        int[] platform = new int[9]; 
        int[] filler = new int[9]; 
        int[] decoration = new int[9]; 
        int[,] occupied = new int[3,9];

        //5 scenes
        //for(int k = 0; k < 5; k++){
        //each scene, three levels; each level, eight platform slots
            for(int i = 0; i < 3; i++){
                //platform: generate base platform type
                //type: 0: no platform; 1 = solid square; 2 = one hole in the centre; 3 = two cornor holes; 4 = middle mosac hole
                //filler: generate filler type
                //type: 0: no filler; 1 = water; 2 = poison
                //decoration: generate decorative object combination type
                for(int j = 0; j < 9; j++){
                    platform[j] = rdm.Next(0,5);
                    filler[j] = rdm.Next(0,3);
                    decoration[j] = rdm.Next(0,4);
                    if(platform[j] != 0){
                        occupied[i,j] = 1;
                    }else{
                        occupied[i,j] = 0;
                    }
                }
                //control empty percentage
                for(int j = 0; j < 9; j++){
                    if(platform[j] != 0){
                        if(CheatRdm() != 0 || j == 4){
                            platform[j] = 0;
                            occupied[i, j] = 0;
                            filler[j] = 0;
                            decoration[j] = 0;
                        }
                    }
                }
                //make sure each level there is at least 2 platform
                int exi = 0;
                for(int j = 0; j < 9; j++){
                    if(platform[j] != 0){
                        exi++;
                    }
                    if(exi == 2){
                        break;
                    }
                }
                int temp;
                if(exi < 2){
                    temp = rdm.Next(0, 9);
                    while(exi < 2 || occupied[i, exi] == 1){
                        temp = rdm.Next(0, 9);
                    }
                    exi++;
                    //Debug.Log("in start(), the second filler platform is number "+temp);
                    platform[temp] = rdm.Next(1,4);
                    occupied[i, temp] = 1;
                }

                /*for(int j = 0; j < 9; j++){
                    Debug.Log("j: "+j+", occupied["+i+", "+j+"]: "+occupied[i, j]+"; platform["+i+", "+j+"]: "+platform[j]);
                }*/

                //place the prefabs
                //generateField(platform, filler, decoration, i, k);
                //generateStairs(platform, i, k);
                generateField(platform, filler, decoration, i, 0);
                generateStairs(platform, i, 0);
            //}
        }    
    }

    void generateField(int [] platform, int [] filler, int [] decoration, int level, int scene)
    {
        Debug.Log("in generateField");

        //generate the 5 scenes
        int x, y;
        int xCoor, zCoor;
        y = (level+1)*levelHeight;
        x = baseXCoor + scene*gapSize;

        //generate base floor and walls
        Vector3 posFloor = new Vector3(scene*gapSize, 0, 0);
        Quaternion rot = new Quaternion ();
        GameObject tempFloor = Instantiate(floor, posFloor, rot);
        
        Vector3 posWall1 =  new Vector3(-35.5f+scene*gapSize, 35.5f, 0);
        GameObject tempWall1 = Instantiate(wall, posWall1, rot);
        tempWall1.transform.Rotate(0, 0, 0, Space.World);

        Vector3 posWall2 =  new Vector3(0f+scene*gapSize, 35.5f, 35.5f);
        GameObject tempWall2 = Instantiate(wall, posWall2, rot);
        tempWall2.transform.Rotate(0, 90, 0, Space.World);

        Vector3 posWall3 =  new Vector3(35.5f+scene*gapSize, 35.5f, 0);
        GameObject tempWall3 = Instantiate(wall, posWall3, rot);
        tempWall3.transform.Rotate(0, 0, 0, Space.World);

        Vector3 posWall4 =  new Vector3(0f+scene*gapSize, 35.5f, -35.5f);
        GameObject tempWall4 = Instantiate(wall, posWall4,rot);
        tempWall4.transform.Rotate(0, 90, 0, Space.World);

        //calculate platform coordinates
        for(int i = 0; i < 9; i++){
            if(i == 4){
                continue;
            }

            xCoor = ((i % 3) * 20 + x);
            zCoor = ((i/3)*20 + baseZCoor);
            Debug.Log("level" + level + " ,position "+i+" occupied, type: "+platform[i]+", xCoor: "+ xCoor+", zCoor: "+zCoor);
            
            Vector3 posPlatform = new Vector3(xCoor, y, zCoor);
            if(platform[i] != 0){
                GameObject tempPlatform = Instantiate(platformFloor, posPlatform, rot);
            }      
        }
    }

    void generateStairs(int [] platform, int level, int scene)
    {
        Debug.Log("in generateStairs()");

        /*for(int i = 0; i < 9; i++){
            if(platform[i]!=0){
                Debug.Log("level" + level + " ,position "+i+" occupied");
            }
        }*/

        //pick two platforms to connect to the 'elevator'
        System.Random rdm = new System.Random();
        int step1 = 4;
        int step2 = 4;
        //Debug.Log(platform.Length);
        step1 = rdm.Next(0,9);
        //Debug.Log(step1);
        while(platform[step1] == 0){
            step1 = rdm.Next(0,9);
        }
        step2 = rdm.Next(0,9);
        while(platform[step2] == 0 || step2 == step1){
            step2 = rdm.Next(0,9);
        }

        

        //add bridge from platforms to 'elevator'
        int x, y;
        int xCoor1 = 0;
        int zCoor1 = 0;
        int xCoor2 = 0;
        int zCoor2 = 0;
        y = (level+1)*levelHeight;
        x = baseXCoor + scene*gapSize;
        float[] degree = new float[9] {45f, 0f, -45f, 90f, 0f, -90f, 135f, 180f, -135f};
        if(step1 == 0||step1 == 3|| step1 == 6){
            xCoor1 = 20+x-12;
        }
        if(step1 == 1||step1 == 7){
            xCoor1 = 20+x;
        }
        if(step1 == 2||step1 == 5|| step1 == 8){
            zCoor1 = 20+x+12;
        }
        if(step1 == 0||step1 == 1|| step1 == 2){
            zCoor1 = 20+baseZCoor+12;
        }
        if(step1 == 3||step1 == 5){
            zCoor1 = 20+baseZCoor;
        }
        if(step1 == 6||step1 == 7|| step1 == 8){
            zCoor1 = 20+baseZCoor-12;
        }
        if(step2 == 0||step2 == 3|| step2 == 6){
            xCoor2 = 20+x-12;
        }
        if(step2 == 1||step2 == 7){
            xCoor2 = 20+x;
        }
        if(step2 == 2||step2 == 5|| step2 == 8){
            zCoor2 = 20+x+12;
        }
        if(step2 == 0||step2 == 1|| step2 == 2){
            zCoor2 = 20+baseZCoor+12;
        }
        if(step2 == 3||step2 == 5){
            zCoor2 = 20+baseZCoor;
        }
        if(step2 == 6||step2 == 7|| step2 == 8){
            zCoor2 = 20+baseZCoor-12;
        }
        //xCoor = (20 + x - 12);
        //zCoor = (20+baseZCoor + 12);

        //Debug.Log("for position: " + step1 + ", xCoor: "+xCoor+", zCoor: "+zCoor);

        Debug.Log("two steps are: "+step1+", "+step2);
        Debug.Log("xCoor1: "+ xCoor1+", zCoor1: "+zCoor1);
        Debug.Log("xCoor2: "+ xCoor2+", zCoor2: "+zCoor2);

        Vector3 posBridge1 = new Vector3(xCoor1, y, zCoor1);
        Vector3 posBridge2 = new Vector3(xCoor2, y, zCoor2);
        Quaternion rot = new Quaternion ();
        GameObject bridge1 = Instantiate(bridge, posBridge1, rot);
        bridge1.transform.Rotate(0, degree[step1], 0, Space.World);
        GameObject bridge2 = Instantiate(bridge, posBridge2, rot);
        bridge2.transform.Rotate(0, degree[step2], 0, Space.World);

        /*for(int i = 0; i < 9; i++){
            //for(int j = 0; j < 2; j++){
                Debug.Log(sign[i,0]);
                Debug.Log(sign[i,1]);
            //}
        }*/
    }      

    //return which platform on level level-1 should the stair starts from level level platform i points to
    int CheckPriority(int a, int[,] occupied, int level)
    {
        int ans = 4;
        int[] priority1 = new int[8] {7,6,8,3,5,1,2,0};
        int[] priority3 = new int[8] {5,8,2,1,7,3,6,0};
        int[] priority5 = new int[8] {3,0,6,7,1,5,2,8};
        int[] priority7 = new int[8] {1,2,0,5,3,7,8,6};

        if(a == 0 || a == 1){
            for(int i = 0; i < priority1.Length; i++){
                if(occupied[level, priority1[i]] != 0){
                    Debug.Log("starting node is: "+ a + " returned: "+ priority1[i]);
                    return priority1[i];
                }
            }
        }
        if(a == 2 || a == 5){
            for(int i = 0; i < priority5.Length; i++){
                if(occupied[level, priority5[i]] != 0){
                    Debug.Log("starting node is: "+ a + " returned: "+ priority5[i]);
                    return priority5[i];
                }
            }
        }
        if(a == 8 || a == 7){
            for(int i = 0; i < priority7.Length; i++){
                if(occupied[level, priority7[i]] != 0){
                    Debug.Log("starting node is: "+ a + " returned: "+ priority7[i]);
                    return priority7[i];
                }
            }
        }
        if(a == 6 || a == 3){
            for(int i = 0; i < priority3.Length; i++){
                if(occupied[level, priority3[i]] != 0){
                    Debug.Log("starting node is: "+ a + " returned: "+ priority3[i]);
                    return priority3[i];
                }
            }
        }

        return ans;
    }


    int CheatRdm()
    {
        System.Random random = new System.Random();
        if (random.NextDouble() < 0.6)
            return 0;

        return 1;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
