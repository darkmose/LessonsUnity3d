using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    GameObject cube;
    Vector3 temp;
    public float speed = 5.0f;
    Animator animator;

    private void Start()
    {
        cube = this.gameObject;
        temp = cube.transform.position;
        temp.y = 34.5f;
        temp.z -= 20f;
        Camera.main.transform.position = temp;
        animator = cube.GetComponent<Animator>();
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
            cube.transform.position = Vector3.MoveTowards(cube.transform.position, cube.transform.position + Vector3.left, Time.deltaTime * speed);
            cube.transform.LookAt(cube.transform.position + Vector3.left);
            temp = cube.transform.position;
            temp.y = 34.5f;
            temp.z -= 20f;
            Camera.main.transform.position = temp;
            animator.SetBool("Move", true);
            return;
        }
        if (Input.GetKey(KeyCode.D))
        {
            cube.transform.position = Vector3.MoveTowards(cube.transform.position, cube.transform.position + Vector3.right, Time.deltaTime * speed);
            cube.transform.LookAt(cube.transform.position + Vector3.right);
            Camera.main.transform.position += Vector3.right;
            temp = cube.transform.position;
            temp.y = 34.5f;
            temp.z -= 20f;
            Camera.main.transform.position = temp;
            animator.SetBool("Move", true);
            return;
        }
        if (Input.GetKey(KeyCode.S))
        {
            cube.transform.position = Vector3.MoveTowards(cube.transform.position, cube.transform.position + Vector3.back, Time.deltaTime * speed);
            cube.transform.LookAt(cube.transform.position + Vector3.back);
            temp = cube.transform.position;
            temp.y = 34.5f;
            temp.z -= 20f;
            Camera.main.transform.position = temp;
            animator.SetBool("Move", true);
            return;
        }
        if (Input.GetKey(KeyCode.W))
        {
            cube.transform.position = Vector3.MoveTowards(cube.transform.position, cube.transform.position + Vector3.forward, Time.deltaTime * speed);
            cube.transform.LookAt(cube.transform.position + Vector3.forward);
            temp = cube.transform.position;
            temp.y = 34.5f;
            temp.z -= 20f;
            Camera.main.transform.position = temp;
            animator.SetBool("Move", true);
            return;
        }
        animator.SetBool("Move", false);
    }
    
    
    void Update()
    {
        MoveCube();
    }
}
