using UnityEngine;
using UnityEngine.UI;

public class MouseManager : MonoBehaviour
{
    [SerializeField] private Image _selectionBoxImage;
    [SerializeField] private LayerMask _unitLayer;

    private readonly float _maxDistance = 50f; // In unity units.
    private readonly float _dragThreshold = 4f; // In pixel.

    private Vector2 _startPosition;
    private Rect _selectionRect;

    private bool _reachedDragThreshold = false;
    private bool _isMouseDown = false;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        Vector2 currentMousePosition = Input.mousePosition;

        if (_isMouseDown && !_reachedDragThreshold)
        {
            _reachedDragThreshold = Mathf.Abs(_startPosition.x - currentMousePosition.x) >= _dragThreshold || Mathf.Abs(_startPosition.y - currentMousePosition.y) >= _dragThreshold;

            if (_reachedDragThreshold)
            {
                BeginDrag();
            }
        }

        if (_isMouseDown && _reachedDragThreshold)
        {
            Drag(currentMousePosition);
        }

        if (Input.GetMouseButtonDown(0))
        {
            _startPosition = currentMousePosition;
            _isMouseDown = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_reachedDragThreshold)
            {
                EndDrag();
                _reachedDragThreshold = false;
            }
            else
            {
                Ray ray = _camera.ScreenPointToRay(currentMousePosition);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, _maxDistance, _unitLayer))
                {
                    Unit unit = hitInfo.transform.gameObject.GetComponent<Unit>();
                    Click(unit);
                }
                else
                {
                    Unit.DeselectAllUnits();
                }
            }
            _isMouseDown = false;
        }
    }

    private void Click(Unit unit)
    {
        if (!UseModifierKey())
        {
            Unit.DeselectAllUnits();
        }
        unit.ClickUnit();
    }

    private void BeginDrag()
    {
        _selectionBoxImage.gameObject.SetActive(true);
        _selectionRect = new Rect();
    }

    private void Drag(Vector2 currentMousePosition)
    {
        if (currentMousePosition.x < _startPosition.x)
        {
            _selectionRect.xMin = currentMousePosition.x;
            _selectionRect.xMax = _startPosition.x;
        }
        else
        {
            _selectionRect.xMin = _startPosition.x;
            _selectionRect.xMax = currentMousePosition.x;
        }

        if (currentMousePosition.y < _startPosition.y)
        {
            _selectionRect.yMin = currentMousePosition.y;
            _selectionRect.yMax = _startPosition.y;
        }
        else
        {
            _selectionRect.yMin = _startPosition.y;
            _selectionRect.yMax = currentMousePosition.y;
        }

        _selectionBoxImage.rectTransform.offsetMin = _selectionRect.min;
        _selectionBoxImage.rectTransform.offsetMax = _selectionRect.max;
    }

    private void EndDrag()
    {
        _selectionBoxImage.gameObject.SetActive(false);

        if (!UseModifierKey())
        {
            Unit.DeselectAllUnits();
        }

        foreach (var unit in Unit.UnitsOnScreen)
        {
            if (_selectionRect.Overlaps(unit.Bounds2D))
            {
                unit.SelectUnit();
            }

            //if (_selectionBox.Contains(Camera.main.WorldToScreenPoint(unit.transform.position)))
            //{
            //    unit.SelectUnit();
            //}
        }
    }

    private bool UseModifierKey()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
}
