using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _scrollIncrement;
    [SerializeField]
    private Camera _cam;
    [SerializeField]
    private bool _invertScroll;
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
        if(_invertScroll == false)
        {
            _cam.fieldOfView -= Input.mouseScrollDelta.y;
        }
        if (_invertScroll == true)
        {
            _cam.fieldOfView += Input.mouseScrollDelta.y;
        }



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
