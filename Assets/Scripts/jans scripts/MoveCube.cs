using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveCube : MonoBehaviour
{

    private Vector3 _lastPos;

    private void Start()
    {
        GameManager.Instance.moveCube = this;
    }
   

    void Update()
    {
        
        

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

            if (!(this.transform.position == _lastPos)) //if theyre not the same
            {
                //update pos
                GameManager.Instance.DidReceiveMoveInput(this.gameObject.transform.position);
                _lastPos = this.transform.position;
            }
            //if its the same pos, dont do anything
            
           
        }
        

       
    }



}
