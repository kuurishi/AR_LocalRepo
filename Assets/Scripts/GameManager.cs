using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Settings _settings;
    private List<GameObject> players = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();


    public void OnTap(LeanFinger _finger)
    {
        print("GameManager.OnTap");

        //create raycast
        Ray ray = Camera.main.ScreenPointToRay(_finger.ScreenPosition);
        RaycastHit hit;

        //if game object is returned
        //then switch the gameObject's material
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) //instead of infinity, can limit to 10f etc.
        {
            GameObject hitGameObject = hit.collider.gameObject;
            Cube hitCubeObject = hitGameObject.GetComponent<Cube>();

            if(hitCubeObject != null)
            {
                hitCubeObject.SwitchMaterial();
            }
            else
            {
                print("warning! returned gameobject did not have a cube component");
            }

        }
        else
        {
            print("ray returned no results");
        }
                
    }



    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    private void Start()
    {
        _settings = FindObjectOfType<Settings>();

        PopulateScene();

    }

    private void PopulateScene()
    {

        PopulateSceneWith(_settings.numberOfPlayers, _settings.playerPrefab);
        PopulateSceneWith(_settings.numberOfEnemies, _settings.enemyPrefab);


    }


    private void PopulateSceneWith(int amount, GameObject prefab)
    {

        for (int i = 0; i < amount; i++)
        {
            Vector3 objectPos = RandomLocationWithinBoundaries();
            GameObject obj = Instantiate(prefab, objectPos, Quaternion.identity);
            
            if(prefab == _settings.playerPrefab)
            {
                players.Add(obj);
            }
            else if(prefab == _settings.enemyPrefab)
            {
                enemies.Add(obj);
            }
        }


    }


    public void Repopulate()
    {

      
        foreach(GameObject player in players) // if we need to count through them, use for int i, <, i++
        {
            Destroy(player);
        }

        foreach (GameObject enemy in enemies) 
        {
            Destroy(enemy);
        }




        PopulateSceneWith(_settings.numberOfPlayers, _settings.playerPrefab);
        PopulateSceneWith(_settings.numberOfEnemies, _settings.enemyPrefab);


    }


    public void AddEnemy()
    {
        PopulateSceneWith(1, _settings.enemyPrefab);
    }




    private Vector3 RandomLocationWithinBoundaries()
    {
        float randomX = Random.Range(_settings.spawnBoundaryXMin, _settings.spawnBoundaryXMax);
        float randomY = Random.Range(_settings.spawnBoundaryYMin, _settings.spawnBoundaryYMax);
        float randomZ = Random.Range(_settings.spawnBoundaryZMin, _settings.spawnBoundaryZMax);

        return new Vector3(randomX, randomY, randomZ);
    }


}
