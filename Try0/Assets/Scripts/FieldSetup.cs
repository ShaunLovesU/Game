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
        for(int k = 0; k < 5; k++){
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
                        if(CheatRdm() != 0){
                            platform[j] = 0;
                            occupied[i, j] = 0;
                            filler[j] = 0;
                            decoration[j] = 0;
                        }
                    }
                }
                //make sure each level there is at least one platform
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
                    platform[temp] = 0;
                    occupied[i, temp] = 0;
                }

                for(int j = 0; j < 9; j++){
                    Debug.Log("j: "+j+", occupied["+i+", "+j+"]: "+occupied[i, j]+"; platform["+i+", "+j+"]: "+platform[j]);
                }

                //place the prefabs
                generateField(platform, filler, decoration, i, k);
                //generateStairs(occupied, i, k);
            }
            /*for(int i = 0; i < 3; i++){
                for(int j = 0; j < 9; j++){
                    if(occupied[i, j] == 1){
                        Debug.Log("level "+i+", position "+j+", occupied");
                    }
                }
            }*/

            generateStairs(occupied, k);
        }    
    }

    void generateField(int [] platform, int [] filler, int [] decoration, int level, int scene)
    {
        Debug.Log("in generateField");

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

        Vector3 posWall2 =  new Vector3(0f+scene*gapSize, 35.5f, -35.5f);
        GameObject tempWall2 = Instantiate(wall, posWall2, rot);
        tempWall2.transform.Rotate(0, 90, 0, Space.World);

        Vector3 posWall3 =  new Vector3(35.5f+scene*gapSize, 35.5f, 0);
        GameObject tempWall3 = Instantiate(wall, posWall3, rot);
        tempWall3.transform.Rotate(0, 0, 0, Space.World);

        Vector3 posWall4 =  new Vector3(0f+scene*gapSize, 35.5f, 35.5f);
        GameObject tempWall4 = Instantiate(wall, posWall4,rot);
        tempWall4.transform.Rotate(0, 90, 0, Space.World);

        //calculate platform coordinates
        for(int i = 0; i < 9; i++){
            if(i == 4){
                continue;
            }

            xCoor = ((i % 3) * 20 + x);
            zCoor = ((i/3)*20 + baseZCoor);
            //Debug.Log("level: "+level+", scene: "+scene+", i: "+ i +", xCoor: "+ xCoor+", zCoor: "+zCoor);
            
            Vector3 posPlatform = new Vector3(xCoor, y, zCoor);
            if(platform[i] != 0){
                GameObject tempPlatform = Instantiate(platformFloor, posPlatform, rot);
            }      
        }
    }

    void generateStairs(int [,] occupied, int scene)
    {
        Debug.Log("in generateStairs");
        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 9; j++){
                if(occupied[i, j] == 1){
                    Debug.Log("level "+i+", position "+j+", occupied");
                }
            }
        }


        //first number on level i, second number on level i-1
        int[,] edge10 = new int[9,9];
        int[,] edge21 = new int[9,9];
        int[,] edge32 = new int[9,9];
        int[,] addStairs = new int[3,2];
        int level = 0;
        int finished = 0;
        int temp;
        System.Random rdm = new System.Random();

        //stairs[level, 0]: on level level, platform stairs[level, 0] will have a stair to level level-1
        //checkpriority will return which platform the stairs should point to on level level-1
        while(finished == 0){
            temp = rdm.Next(0, 9);
            while(occupied[level, temp] == 0){
                temp = rdm.Next(0, 9);
            }
            addStairs[level, 0] = temp;
            temp = rdm.Next(0, 9);
            while(occupied[level, temp] == 0 || temp == addStairs[level, 0]){
                temp = rdm.Next(0, 9);
            }
            addStairs[level, 1] = temp;

            //Debug.Log("level: "+level + ", first starting node: "+ addStairs[level, 0] + ", second starting node is: " + addStairs[level, 1]);
            //Debug.Log("emptiness: occupied["level+","+addStairs[level, 0]+"]: "+ occupied[level, addStairs[level, 0]]+", occupied["+level+","+ addStairs[level, 1]+"]: "+ occupied[level, addStairs[level, 1]]);

            if(level == 2){
                edge32[addStairs[level, 0], CheckPriority(addStairs[level, 0], occupied, level)] = 1;
                edge32[addStairs[level, 1], CheckPriority(addStairs[level, 1], occupied, level)] = 1;
                finished = 1;
            }   
            if(level == 1){
                edge21[addStairs[level, 0], CheckPriority(addStairs[level, 0], occupied, level)] = 1;
                edge21[addStairs[level, 1], CheckPriority(addStairs[level, 1], occupied, level)] = 1;
                level = 2;
            }
            if(level == 0){
                edge10[addStairs[level, 0], CheckPriority(addStairs[level, 0], occupied, level)] = 1;
                edge10[addStairs[level, 1], CheckPriority(addStairs[level, 1], occupied, level)] = 1;
                level = 1;
            }
        }
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
