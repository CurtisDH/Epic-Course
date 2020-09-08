using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera _cam;

    [SerializeField]
    private float _speed;

    #region Scroll Variables
    [SerializeField]
    private float _scrollIncrement;
    [SerializeField]
    private bool _invertScroll;
    [SerializeField]
    private float _maxZoom = 135,_minZoom = 20;

    #endregion
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
        if (_maxZoom >= _cam.fieldOfView || _minZoom <= _cam.fieldOfView) return;
        else
        {
            if (_invertScroll == false)
            {

                _cam.fieldOfView -= Input.mouseScrollDelta.y * _scrollIncrement;
            }
            else
            {
                _cam.fieldOfView += Input.mouseScrollDelta.y * _scrollIncrement;
            }
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
