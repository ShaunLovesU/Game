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
                        }
                    }
                }
                
                //make sure each level there is at least one platform
                int exi = 0;
                for(int j = 0; j < 9; j++){
                    if(platform[j] != 0){
                        exi = 1;
                        break;
                    }
                }
                if(exi == 0){
                    exi = rdm.Next(0, 9);
                    platform[exi] = 0;
                    occupied[i, exi] = 0;
                }

                //place the prefabs
                generateField(platform, filler, decoration, i, k);
            }
            generateStairs(occupied);
        }    
    }

    void generateField(int [] platform, int [] filler, int [] decoration, int level, int scene)
    {
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
            Debug.Log("level: "+level+", scene: "+scene+", i: "+ i +", xCoor: "+ xCoor+", zCoor: "+zCoor);
            
            Vector3 posPlatform = new Vector3(xCoor, y, zCoor);
            Quaternion rotPlatform = new Quaternion();

            if(platform[i] != 0){
                GameObject tempPlatform = Instantiate(platformFloor, posPlatform, rot);
            }      
        }
    }

    void generateStairs(int [,] occupied)
    {
        int[] occupied1 = new int [9];
        int[] occupied2 = new int [9];
        int[] occupied3 = new int [9];

        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 9; j++){
                if(i == 0){
                    occupied1[j] = occupied[i,j];
                }
                if(i == 1){
                    occupied2[j] = occupied[i,j];
                }
                if(i == 2){
                    occupied3[j] = occupied[i,j];
                }
            }
        }

        //init
        int[,] saturated = new int[3,9];
        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 9; j++){
                saturated[i,j] = 0;
            }
        }

        System.Random rdm = new System.Random();
        int temp;
        //add edges in graph
        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 9; j++){
                if(occupied[i,j] == 0){
                    continue;
                }
                Debug.Log(CheckAdjacent(j));
            }
        }
    }

    int[] CheckAdjacent(int i)
    {
        int[] ans = new int[2];
        if(i == 0){
            ans = new int[2] {1,3};
        }
        if(i == 1){
            ans = new int[2] {0,2};
        }
        if(i == 2){
            ans = new int[2] {1,5};
        }
        if(i == 3){
            ans = new int[2] {0,6};
        }
        if(i == 4){
            ans = new int[2] {4,4};
        }
        if(i == 5){
            ans = new int[2] {2,8};
        }
        if(i == 6){
            ans = new int[2] {3,7};
        }
        if(i == 7){
            ans = new int[2] {6,8};
        }
        if(i == 8){
            ans = new int[2] {5,7};
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
