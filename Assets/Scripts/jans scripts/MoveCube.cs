using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class MoveCube : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.moveCube = this;
    }
   

    void Update()
    {
        //GameManager.Instance._player.id = 

        

        if (GameManager.Instance.IsMyId(this.gameObject))
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            if (Input.GetKey(KeyCode.A))
                rb.AddForce(Vector3.left);
            if (Input.GetKey(KeyCode.D))
                rb.AddForce(Vector3.right);
            if (Input.GetKey(KeyCode.W))
                rb.AddForce(Vector3.up);
            if (Input.GetKey(KeyCode.S))
                rb.AddForce(Vector3.down);

            GameManager.Instance.DidReceiveMoveInput(this.gameObject.transform.position);
        }


       
    }



}
