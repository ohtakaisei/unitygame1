using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public int colums = 20, rows = 20;
    public GameObject[] floorTiles;
    public GameObject[] OutWallTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject exit;
    public GameObject[] enemyTiles;//enemyの素材取得

    private List<Vector3> gridPositons = new List<Vector3>();

    public int wallMinimun = 5, wallMaximum = 9, foodMinimun = 1, foodMaximum = 5;




    void BoardSetup()
    {
        for(int x = -1; x < colums + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate;

                if(x == -1 || x == colums || y == -1 || y == rows)
                {
                    toInstantiate = OutWallTiles[Random.Range(0, OutWallTiles.Length)];
                }
                else
                {
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                }

                Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }



    void InitialiseList()
    {
        gridPositons.Clear();

        for(int x = 1; x < colums -1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositons.Add(new Vector3(x, y, 0));
            }
        }
    }

    Vector3 RandomPositon()
    {
        int randomIndex = Random.Range(0, gridPositons.Count);

        Vector3 randomPositon = gridPositons[randomIndex];

        gridPositons.RemoveAt(randomIndex);

        return randomPositon;


    }


    void LayoutobjectAtRandom(GameObject[] taileArray,int min ,int max)
    {
        int objectCount = Random.Range(min,max + 1);

        for(int i = 0; i < objectCount;i++)
        {
            Vector3 randomPositon = RandomPositon();

            GameObject tileChoice = taileArray[Random.Range(0, taileArray.Length)];

            Instantiate(tileChoice, randomPositon, Quaternion.identity);
        }
    }


    public void SetupSecene(int level)//引数を受け取るように
    {
        BoardSetup();
        InitialiseList();
        LayoutobjectAtRandom(wallTiles, wallMinimun,wallMaximum);
        LayoutobjectAtRandom(foodTiles, foodMinimun, foodMaximum);


        int enemyCount = (int)Mathf.Log(level, 2f);//logの計算でEnemyの生成数を決める
        LayoutobjectAtRandom(enemyTiles, enemyCount, enemyCount);//Enemyを生成する（生成するオブジェクト、最小、最大）

        Instantiate(exit, new Vector3(colums - 1, rows - 1, 0), Quaternion.identity);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}