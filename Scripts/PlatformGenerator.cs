using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{

    public Transform generationPoint;
    private float distanceBetween;

    private float[] platformWidths;

    public float distanceBetweenMin;
    public float distanceBetweenMax;

    //Height Difference in platforms
    private float minHeight;
    private float maxHeight;
    public Transform maxHeightPoint;
    public float maxHeightChange;
    private float heightChange;
    
    private int platformSelector;
    //public GameObject[] thePlatforms;

    //reference to object pooler
    public ObjectPooler[] theObjectPools;
    private GameObject newPlatform;

    [Header("Obstacle Generation System")]
    //obstacle Generator
    [SerializeField] GameObject[] Obstacles, Coins, Enemies;

    void Start()
    {
        //creating the array platformWidths with the same size thePlatforms array has
        platformWidths = new float[theObjectPools.Length];

        //Paralleling the platformWidths to the type of the platform that is selected
        for (int i = 0; i < theObjectPools.Length; i++)
        {
            platformWidths[i] = theObjectPools[i].pooledObject.GetComponent<BoxCollider2D>().size.x;
        }

       

        minHeight = transform.position.y;
        maxHeight = maxHeightPoint.position.y;


    }

    
    void Update()
    {
        SpawnPlatform();
    }

    public void SpawnPlatform()
    {
        //If the X position of the Platform generator (the object in which this script is attached to) is less than the position of-- 
        //-- the GenerationPoint (an empty object that is attached in front of the camera and moves with it)
        if (transform.position.x < generationPoint.position.x)
        {
            //Getting A random distance to add to the new position of the generator by getting a random value from 2 public variables: distanceBetweenMin and distanceBetweenMax
            distanceBetween = Random.Range(distanceBetweenMin, distanceBetweenMax);

            //Getting a random array position from 0 to the maximum amount of "thePlatforms" array
            platformSelector = Random.Range(0, theObjectPools.Length);

            //Heightchange
            heightChange = transform.position.y + Random.Range(maxHeightChange, -maxHeightChange);

            if (heightChange > maxHeight)
            {
                heightChange = maxHeight;
            }
            else if (heightChange < minHeight)
            {
                heightChange = minHeight;
            }


            //set the X position of the generator forward adding the distanceBetween value and the width of the random platform in order to avoid overlapping
            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2) + distanceBetween, heightChange, transform.position.z);


            //instantiates a new platform at the position of the platform generator
            //Instantiate(thePlatforms[platformSelector], transform.position, transform.rotation);


            //Platform Spawning using Pooling

            newPlatform = theObjectPools[platformSelector].GetPooledObject();
            newPlatform.transform.position = transform.position;
            newPlatform.transform.rotation = transform.rotation;
            newPlatform.SetActive(true);


            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2), transform.position.y, transform.position.z);

            SpawnObstacle();
        }
    }


    public void SpawnObstacle()
    {
        // RANDOM OBSTACLE SELLECTOR \\
        GameObject obstacleSelector;
        int randomNumber = Random.Range(0, 100);
        
        // 30% chance Warband Members
        if (randomNumber >= 0 && randomNumber < 30)
        {
            obstacleSelector = Enemies[Random.Range(0, Enemies.Length)];
        }
        //25% chance Coins
        else if (randomNumber >= 30 && randomNumber < 55)
        {
            obstacleSelector = Coins[Random.Range(0, Coins.Length)];
        }
        //40% chance Obstacles
        else if (randomNumber >= 55 && randomNumber < 95)
        {
            obstacleSelector = Obstacles[Random.Range(0, Obstacles.Length)];
        }
        //10% chance
        else
        {
            obstacleSelector = null;
        }


        // OBSTACLE SPAWNER \\ 
        if (obstacleSelector != null)
        {
            int childrenCount;

            childrenCount = newPlatform.transform.GetChild(0).transform.childCount;

            // Choose point to spawn the obstacle
            if (childrenCount != 0)
            {
                Transform obstacleSpawn;
                Vector3 obstacleVector;

                int obstacleSpawnIndex = Random.Range(0, childrenCount);

                obstacleSpawn = newPlatform.transform.GetChild(0).transform.GetChild(obstacleSpawnIndex);
                obstacleVector.x = obstacleSpawn.transform.position.x;
                obstacleVector.y = obstacleSpawn.transform.position.y + obstacleSelector.transform.position.y;
                obstacleVector.z = obstacleSpawn.transform.position.z;


                //Spawn obstacle at the position 
                Instantiate (obstacleSelector, obstacleVector, Quaternion.identity);
                Debug.Log(childrenCount);
            }
            else
            {
                Debug.Log("no obstacle");
            }

        }
    }

}
