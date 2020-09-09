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
    #region Boundaries
    [SerializeField]
    Vector2 _xBounds, _yBounds, _zBounds;
    #endregion


    void Update()
    {
        MoveCamera();
    }

    void MoveCamera() // Might add A middlemouse to rotate the camera.
    {
        ClampPosition();
        WASD();
        Scroll();
        EdgeScroll();
    }
    void Scroll() // Need to limit max zoom
    {

        if (_invertScroll == false)
        {
            _cam.transform.position = new Vector3(_cam.transform.position.x + Input.GetAxis("Mouse ScrollWheel") * _scrollIncrement,
                _cam.transform.position.y + -Input.GetAxis("Mouse ScrollWheel") * _scrollIncrement,
                _cam.transform.position.z
            );
        }
        else
        {
            _cam.transform.position = new Vector3(_cam.transform.position.x + -Input.GetAxis("Mouse ScrollWheel") * _scrollIncrement,
              _cam.transform.position.y + Input.GetAxis("Mouse ScrollWheel") * _scrollIncrement,
              _cam.transform.position.z
             );
        }

    }
    void WASD()
    {
        MoveDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
    }
    void ClampPosition()
    {
        #region Parent Object
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, _xBounds.x, _xBounds.y),
            Mathf.Clamp(transform.position.y, _yBounds.x, _yBounds.y),
            Mathf.Clamp(transform.position.z, _zBounds.x, _zBounds.y)
            );
        #endregion
    }
    void EdgeScroll() // Still need to smooth the edge scrolling. Quick look at the API (Vector3.SmoothDamp) (Mathf.SmoothDamp) (Vector3.Lerp) 
    {
        //Don't really want to use this in update unless required. Perhaps UI triggers?
        if (Input.mousePosition.x > Screen.width - _edgeScrollSize)
        {
            //Debug.Log("CameraController::MPos.X:RIGHT " + Input.mousePosition.x);
            MoveDirection(Vector3.right);
        }
        if (Input.mousePosition.x < _edgeScrollSize)
        {
            //Debug.Log("CameraController::MPos.X:LEFT " + Input.mousePosition.x);
            MoveDirection(Vector3.left);
        }
        if (Input.mousePosition.y > Screen.height - _edgeScrollSize)
        {
            //Debug.Log("CameraController::MPos.Y:TOP " + Input.mousePosition.y);
            MoveDirection(Vector3.forward);
        }
        if (Input.mousePosition.y < _edgeScrollSize)
        {
            //Debug.Log("CameraController::MPos.Y:BOTTOM " + Input.mousePosition.y);
            MoveDirection(Vector3.back);
        }
    }


    void MoveDirection(Vector3 direction) //may add an option for an independent edge scroll speed.
    {
        transform.Translate(direction * (_speed * Time.deltaTime));
    }
}




