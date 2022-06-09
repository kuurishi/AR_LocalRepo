using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public Material playerMaterial;
    public Material enemyMaterial;
    public Material errorMaterial;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public float numberOfPlayers = 10;
    public float numberOfEnemies = 10;

    //[HideInInspector] //doesn't work for some reason ¯\_(ツ)_/¯
    public float spawnBoundaryXMin = 0;
    public float spawnBoundaryYMin = 0;
    public float spawnBoundaryZMin = 0;
    public float spawnBoundaryXMax = 10;
    public float spawnBoundaryYMax = 10;
    public float spawnBoundaryZMax = 10;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
