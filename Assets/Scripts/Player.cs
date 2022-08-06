using UnityEngine;

public class Player : MonoBehaviour
{
    public LayerMask collisionMask;
    public Camera mainCamera;
    public GameObject targetPrefab;

    [HideInInspector]
    public GameObject currentTarget;

    private float _originalZoom;
    private float _maxZoom;
    private float _minZoom;

    void Start()
    {
        _originalZoom = mainCamera.fieldOfView;
        _maxZoom = _originalZoom - 25f;
        _minZoom = _originalZoom + 40f;
    }

    void Update()
    {
        SetNewTarget();
        Zoom();
    }

    void SetNewTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, collisionMask))
            {
                if (hit.collider.gameObject.layer == K.LAYER_FLOOR)
                {
                    var objectsTouched = Physics.OverlapSphere(hit.point, 1.2f);
                    var obstacleTouched = false;

                    foreach (var touchedObject in objectsTouched)
                    {
                        if (touchedObject.gameObject.layer == K.LAYER_OBSTACLE)
                            obstacleTouched = true;
                    }

                    if (!obstacleTouched)
                    {
                        if (currentTarget != null)
                            Destroy(currentTarget.gameObject);
                        currentTarget = (GameObject)Instantiate(targetPrefab, new Vector3(hit.point.x, targetPrefab.transform.position.y, hit.point.z), targetPrefab.transform.rotation);
                    }
                }
            }
        }
    }

    void Zoom()
    {
        if (Input.GetMouseButtonDown(2))
            mainCamera.fieldOfView = _originalZoom;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (mainCamera.fieldOfView <= _minZoom)
                mainCamera.fieldOfView += 2.5f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (mainCamera.fieldOfView >= _maxZoom)
                mainCamera.fieldOfView -= 2.5f;
        }
    }
}