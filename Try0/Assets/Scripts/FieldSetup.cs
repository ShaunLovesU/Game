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

    public int scene = 0;


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

                generateField(platform, filler, decoration, i, scene);
                generateStairs(platform, i, scene);
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
            //Debug.Log("level" + level + " ,position "+i+" occupied, type: "+platform[i]+", xCoor: "+ xCoor+", zCoor: "+zCoor);
            
            Vector3 posPlatform = new Vector3(xCoor, y, zCoor);
            if(platform[i] != 0){
                GameObject tempPlatform = Instantiate(platformFloor, posPlatform, rot);
            }      
        }
    }


    void generateStairs(int [] platform, int level, int scene)
    {
        Debug.Log("in generateStairs()");
        //Debug.Log("level: "+ level);

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
        //int xCoor2 = 0;
        //int zCoor2 = 0;
        y = (level+1)*levelHeight;
        x = baseXCoor + scene*gapSize;
        float[] degree = new float[9] {-45f, 90f, 45f, 0f, 0f, 0f, 45f, 90f, -45f};
        //adjust bridge position
        int gapPozi = 10;

        for(int i = 0; i < 9; i++){
            if(platform[i] != 0){
                step1 = i;
                if(step1 == 0||step1 == 3|| step1 == 6){
                    xCoor1 = 20+x-gapPozi;
                }
                if(step1 == 1||step1 == 7){
                    xCoor1 = 20+x;
                }
                if(step1 == 2||step1 == 5|| step1 == 8){
                    xCoor1 = 20+x+gapPozi;
                }
                if(step1 == 0||step1 == 1|| step1 == 2){
                    zCoor1 = 20+baseZCoor-gapPozi;
                }
                if(step1 == 3||step1 == 5){
                    zCoor1 = 20+baseZCoor;
                }
                if(step1 == 6||step1 == 7|| step1 == 8){
                    zCoor1 = 20+baseZCoor+gapPozi;
                }
                Vector3 posBridge1 = new Vector3(xCoor1, y, zCoor1);
                 Quaternion rot = new Quaternion ();
                GameObject bridge1 = Instantiate(bridge, posBridge1, rot);
                bridge1.transform.Rotate(0, degree[step1], 0, Space.World);
            }
        }
    }      


    //return which platform on level level-1 should the stair starts from level level platform i points to
    int CheckNeighbour(int posi, int[] saturated, int[] platform, int r)
    {
        Debug.Log("in CheckNeighbour");
        //int r = 1;
        int ans = 4;

        int i_x = r%3;
        //int i_z = i/3;
        int posi_x = posi%3;
        //int pozi_z = i/3;
        /*if((i == posi+1 && i_x > posi_x)||(i == posi-1 && i_x < posi_x)||(i == posi-3 && i_x == posi_x)||(i == posi+3 && i_x == posi_x)){
            ans = i;
            break;
        }*/
        if(posi+r < 9){
            if(saturated[posi+r] != 1 && platform[posi+r] != 0 && i_x > posi_x){
                ans = posi+r;

                Debug.Log("head: "+ posi+", ans: "+ans);
                return ans;
            }
        }
        if(posi - r >= 0){
            if(saturated[posi-r] != 1 && platform[posi-r] != 0 && i_x < posi_x){
                ans = posi+r;

                Debug.Log("head: "+ posi+", ans: "+ans);
                return ans;
            }
        }
        if(posi+r*3 < 9){
            if(saturated[posi+r*3] != 1 && platform[posi+r*3] != 0 && i_x == posi_x){
                ans = posi+r;

                Debug.Log("head: "+ posi+", ans: "+ans);
                return ans;
            }
        }
        if(posi-r*3 >= 0){
            if(saturated[posi-r*3] != 1 && platform[posi-r*3] != 0 && i_x == posi_x){
                ans = posi+r;

                Debug.Log("head: "+ posi+", ans: "+ans);
                return ans;
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
}
