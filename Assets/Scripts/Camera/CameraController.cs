using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _scrollIncrement;
    void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        WASD();
        Scroll();

    }
    void Scroll()
    {
        Vector2 scrollDelta = Input.mouseScrollDelta;



        EdgeScroll();
    }
    void WASD()
    {
        if (Input.GetKey(KeyCode.W)) //up/forward
        {
            transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.A))//left
        {
            transform.Translate(Vector3.left * (_speed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.S))//down/back
        {
            transform.Translate(Vector3.back * (_speed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.D))//right
        {
            transform.Translate(Vector3.right * (_speed * Time.deltaTime));
        }
    }
    void EdgeScroll()
    {

    }
}
