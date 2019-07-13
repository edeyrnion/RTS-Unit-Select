using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private Camera _camera;
    private readonly float _maxDistance = 50f;
    //RaycastHit hitInfo;

    [SerializeField] private GUIStyle _mouseDragSkin;

    private Vector3 _mouseDownPosition;
    private Vector3 _mouseUpPosition;
    //private Vector3 _mouseCurrentPosition;
    private readonly float _clickThreshold = 2f;

    private bool _isDragging;

    public Events.EventUnitSelect _unitSelect;
    public Events.EventUnitDragSelect _dragSelect;
    public Events.EventUnitDeselect _unitDeselect;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        LayerMask layerMask = (int)Layer.Unit | (int)Layer.Terrain;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, _maxDistance, layerMask))
        {
            GameObject target = hitInfo.collider.gameObject;

            if (Input.GetMouseButtonDown(0))
            {
                _mouseDownPosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.nearClipPlane));
                _isDragging = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;

                if (IsLeftMouseClick())
                {
                    if (target.layer == (int)Layer.Unit)
                    {
                        _unitSelect.Invoke(target, UseModifierKey());
                    }
                    else if (!UseModifierKey())
                    {
                        _unitDeselect.Invoke();
                    }

                }
                else
                {
                    Vector3 topLeftCorner = _mouseDownPosition;
                    Vector3 bottomRightCorner = _camera.ScreenToWorldPoint(Input.mousePosition);
                    GameObject[] targets = DragSelect(topLeftCorner, bottomRightCorner, (int)Layer.Unit);

                    _dragSelect.Invoke(targets, UseModifierKey());
                }
            }
        }
    }

    private void OnGUI()
    {
        if (_isDragging && !IsLeftMouseClick())
        {
            Vector3 mouseDownPosition = _camera.WorldToScreenPoint(_mouseDownPosition);
            float boxWidth = mouseDownPosition.x - Input.mousePosition.x;
            float boxHight = mouseDownPosition.y - Input.mousePosition.y;

            float boxLeft = Input.mousePosition.x;
            float boxTop = Screen.height - Input.mousePosition.y - boxHight;

            GUI.Box(new Rect(boxLeft, boxTop, boxWidth, boxHight), "", _mouseDragSkin);
        }
    }

    private bool IsLeftMouseClick()
    {
        if (Vector3.Distance(_camera.WorldToScreenPoint(_mouseDownPosition), Input.mousePosition) < _clickThreshold)
        {
            return true;
        }

        return false;
    }

    private GameObject[] DragSelect(Vector3 topLeftCorner, Vector3 bottomRightCorner, int layerMask)
    {
        GameObject[] targets = new GameObject[0];
        return targets;
    }

    private bool UseModifierKey()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
}
