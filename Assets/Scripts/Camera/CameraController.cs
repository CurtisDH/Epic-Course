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
    private float _maxZoom = 135, _minZoom = 20;

    #endregion

    #region EdgeScroll Variables
    [SerializeField]
    private float _edgeScrollSize;
    #endregion



    void Update()
    {
        MoveCamera();
    }

    void MoveCamera() // Might add A middlemouse to rotate the camera.
    {
        WASD();
        Scroll();
        EdgeScroll();
    }
    void Scroll() // Will change from FOV later just a temp setup
    {

        if (_invertScroll == false)
        {
            if (-Input.mouseScrollDelta.y > 0 && _cam.fieldOfView >= _maxZoom) return;
            if (-Input.mouseScrollDelta.y < 0 && _cam.fieldOfView <= _minZoom) return;
            _cam.fieldOfView -= Input.mouseScrollDelta.y * _scrollIncrement;
        }
        else
        {
            if (Input.mouseScrollDelta.y > 0 && _cam.fieldOfView >= _maxZoom) return;
            if (Input.mouseScrollDelta.y < 0 && _cam.fieldOfView <= _minZoom) return;
            _cam.fieldOfView += Input.mouseScrollDelta.y * _scrollIncrement;
        }

    }
    void WASD() 
    {
        MoveDirection(new Vector3(Input.GetAxis("Horizontal"),0 , Input.GetAxis("Vertical")));
    }
    void EdgeScroll() // Still need to smooth the edge scrolling. Quick look at the API (Vector3.SmoothDamp) (Mathf.SmoothDamp) (Vector3.Lerp) 
    {
        //Don't really want to use this in update unless required. Perhaps UI triggers?
        if (Input.mousePosition.x > Screen.width - _edgeScrollSize)
        {
            Debug.Log("CameraController::MPos.X:RIGHT " + Input.mousePosition.x);
            MoveDirection(Vector3.right);
        }
        if (Input.mousePosition.x < _edgeScrollSize)
        {
            Debug.Log("CameraController::MPos.X:LEFT " + Input.mousePosition.x);
            MoveDirection(Vector3.left);
        }
        if (Input.mousePosition.y > Screen.height - _edgeScrollSize)
        {
            Debug.Log("CameraController::MPos.Y:TOP " + Input.mousePosition.y);
            MoveDirection(Vector3.forward);
        }
        if (Input.mousePosition.y < _edgeScrollSize)
        {
            Debug.Log("CameraController::MPos.Y:BOTTOM " + Input.mousePosition.y);
            MoveDirection(Vector3.back);
        }
    }


    void MoveDirection(Vector3 direction) //may add an option for an independent edge scroll speed.
    {
        transform.Translate(direction * (_speed * Time.deltaTime));
    }
}




