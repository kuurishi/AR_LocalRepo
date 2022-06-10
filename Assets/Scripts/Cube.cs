using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{

    //public Viariables
    public GameObjectType type;
    public string title;


    //private Variables
    private Settings _settings;
    private Renderer _renderer;
    private int _lifePoints;


    private bool _materialChanged = false;



    // Start is called before the first frame update
    //MonoBehaviour Events
    void Start()
    {
        _settings = FindObjectOfType<Settings>();
        _renderer = GetComponent<Renderer>();
        _lifePoints = LifePointsFor(type);

        if (_renderer != null && _settings != null)
        {
            _renderer.material = MaterialFor(type);
        }
        else
        {
            if (_renderer == null)
            {
                Debug.LogWarning($"{title}: _renderer is null", this);
            }
            else if (_settings == null)
            {
                Debug.LogWarning($"{title}: _settings is null", this);
            }
        }
    }



    //Public Methods
    public void AddLifePoints(int points)
    {
        _lifePoints += points;
    }



    //private Methods
    private int LifePointsFor(GameObjectType _type)
    {
        switch (_type)
        {
            case GameObjectType.Player:
                return 100;
            case GameObjectType.Enemy:
                return 200;
            default:
                Debug.LogWarning("Cube.LifePoints: _type is unknown");
                return 0;
        }
    }





    private Material MaterialFor(GameObjectType _type)
    {
        if (_type == GameObjectType.Player)
        {
            return _settings.playerMaterial;
        }
        else if (_type == GameObjectType.Enemy)
        {
            return _settings.enemyMaterial;
        }


        Debug.LogWarning("Cube.LifePoints: _type is unknown");
        return _settings.errorMaterial;
    
    }

    



    public void SwitchMaterial()
    {
        
        _renderer.material = _settings.tappedMaterial;


        if (!_materialChanged)
        {
            _renderer.material = _settings.tappedMaterial;
            _materialChanged = true;

        }
        else
        {
            _renderer.material = MaterialFor(type);
            _materialChanged = false;
        }


    }




}
