using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    GameObject modelGameObject;
    Vector3 temp;
    Animator animator;

    public float speed = 25.0f;


    private void Start()
    {
        modelGameObject = this.gameObject;
        CameraMove();
        animator = modelGameObject.GetComponent<Animator>();
    }


    private void MoveCube() 
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 75.0f;
            animator.SetFloat("Multiplier", 2.0f);
        }
        else
        {
            speed = 25.0f;
            animator.SetFloat("Multiplier", 1.0f);
        }
        if (Input.GetKey(KeyCode.A))
        { 
            modelGameObject.transform.position = Vector3.MoveTowards(modelGameObject.transform.position, modelGameObject.transform.position + Vector3.left, Time.deltaTime * speed);
            modelGameObject.transform.LookAt(modelGameObject.transform.position + Vector3.left);
            animator.SetBool("Move", true);
            return;
        }
        if (Input.GetKey(KeyCode.D))
        {
            modelGameObject.transform.position = Vector3.MoveTowards(modelGameObject.transform.position, modelGameObject.transform.position + Vector3.right, Time.deltaTime * speed);
            modelGameObject.transform.LookAt(modelGameObject.transform.position + Vector3.right);
            animator.SetBool("Move", true);
            return;
        }
        if (Input.GetKey(KeyCode.S))
        {
            modelGameObject.transform.position = Vector3.MoveTowards(modelGameObject.transform.position, modelGameObject.transform.position + Vector3.back, Time.deltaTime * speed);
            modelGameObject.transform.LookAt(modelGameObject.transform.position + Vector3.back);
            animator.SetBool("Move", true);
            return;
        }
        if (Input.GetKey(KeyCode.W))
        {
            modelGameObject.transform.position = Vector3.MoveTowards(modelGameObject.transform.position, modelGameObject.transform.position + Vector3.forward, Time.deltaTime * speed);
            modelGameObject.transform.LookAt(modelGameObject.transform.position + Vector3.forward);
            animator.SetBool("Move", true);
            return;
        }
        animator.SetBool("Move", false);
    }
    
    private void CameraMove() 
    {
        temp = modelGameObject.transform.position;
        temp.y = 34.5f;
        temp.z -= 20f;
        Camera.main.transform.position = temp;
    }



    private void FixedUpdate()
    {
        MoveCube();
    }

    private void LateUpdate()
    {
        CameraMove();
    }
}
