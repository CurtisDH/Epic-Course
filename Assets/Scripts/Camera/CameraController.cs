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
            var camPos = _cam.transform.position;
            camPos.x += Input.GetAxis("Mouse ScrollWheel") * _scrollIncrement;
            camPos.y += -Input.GetAxis("Mouse ScrollWheel") * _scrollIncrement;

            _cam.transform.position = camPos;
        }
        else
        {
            var InvertedCamPos = _cam.transform.position;
            InvertedCamPos.x += -Input.GetAxis("Mouse ScrollWheel") * _scrollIncrement;
            InvertedCamPos.y += Input.GetAxis("Mouse ScrollWheel") * _scrollIncrement;
            _cam.transform.position = InvertedCamPos;
        }

    }
    void WASD()
    {
        var hozMovment = Input.GetAxis("Horizontal");
        var vertMovement = Input.GetAxis("Vertical");
        var direction = new Vector3(hozMovment, 0, vertMovement);
        MoveDirection(direction);

    }
    void ClampPosition()
    {
        #region Parent Object
        var clampPos = transform.position;
        clampPos.x = Mathf.Clamp(clampPos.x, _xBounds.x, _xBounds.y);
        clampPos.y = Mathf.Clamp(clampPos.y, _yBounds.x, _yBounds.y);
        clampPos.z = Mathf.Clamp(clampPos.z, _zBounds.x, _zBounds.y);
        transform.position = clampPos;

        #endregion
    }
    void EdgeScroll() // Still need to smooth the edge scrolling. Quick look at the API (Vector3.SmoothDamp) (Mathf.SmoothDamp) (Vector3.Lerp) 
    {
        var direction = Vector3.zero;
        //Don't really want to use this in update unless required. Perhaps UI triggers?
        if (Input.mousePosition.x > Screen.width - _edgeScrollSize)
        {
            //Debug.Log("CameraController::MPos.X:RIGHT " + Input.mousePosition.x);
            direction = Vector3.right;
        }
        if (Input.mousePosition.x < _edgeScrollSize)
        {
            //Debug.Log("CameraController::MPos.X:LEFT " + Input.mousePosition.x);
            direction = Vector3.left;
        }
        if (Input.mousePosition.y > Screen.height - _edgeScrollSize)
        {
            //Debug.Log("CameraController::MPos.Y:TOP " + Input.mousePosition.y);
            direction = Vector3.forward;
        }
        if (Input.mousePosition.y < _edgeScrollSize)
        {
            //Debug.Log("CameraController::MPos.Y:BOTTOM " + Input.mousePosition.y);
            direction = Vector3.back;
        }

        MoveDirection(direction);
    }


    void MoveDirection(Vector3 direction) //may add an option for an independent edge scroll speed.
    {
        transform.Translate(direction * (_speed * Time.deltaTime));
    }
}




