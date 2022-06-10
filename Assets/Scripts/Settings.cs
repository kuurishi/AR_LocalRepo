using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{


    public Material playerMaterial;
    public Material enemyMaterial;
    public Material errorMaterial;
    public Material tappedMaterial;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public int numberOfPlayers = 10;
    public int numberOfEnemies = 10;

    public float spawnBoundaryXMin = 0;
    public float spawnBoundaryYMin = 0;
    public float spawnBoundaryZMin = 0;
    public float spawnBoundaryXMax = 10;
    public float spawnBoundaryYMax = 10;
    public float spawnBoundaryZMax = 10;


   
}
