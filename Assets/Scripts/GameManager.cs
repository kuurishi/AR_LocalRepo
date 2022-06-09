using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private Settings _settings;
    private List<GameObject> players = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();


    private void Start()
    {
        _settings = FindObjectOfType<Settings>();

        PopulateScene();

    }


    private void PopulateScene()
    {

        for(int i = 0; i < _settings.numberOfPlayers; i++)
        {
            Vector3 playerPos = RandomLocationWithinBoundaries();
            GameObject player = Instantiate(_settings.playerPrefab, playerPos, Quaternion.identity);
            players.Add(player);
        }


        for (int i = 0; i < _settings.numberOfEnemies; i++)
        {
            Vector3 enemyPos = RandomLocationWithinBoundaries();
            GameObject enemy = Instantiate(_settings.enemyPrefab, enemyPos, Quaternion.identity);
            players.Add(enemy);
        }


    }



    private Vector3 RandomLocationWithinBoundaries()
    {
        float randomX = Random.Range(_settings.spawnBoundaryXMin, _settings.spawnBoundaryXMax);
        float randomY = Random.Range(_settings.spawnBoundaryYMin, _settings.spawnBoundaryYMax);
        float randomZ = Random.Range(_settings.spawnBoundaryZMin, _settings.spawnBoundaryZMax);

        return new Vector3(randomX, randomY, randomZ);
    }


}
