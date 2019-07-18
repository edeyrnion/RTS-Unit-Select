﻿using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Base color of the mouse drag selection box.")]
    [SerializeField] private Color _selecteionBoxColor = new Color(0.01f, 0.4f, 0.6f, 1f);
    [Tooltip("Alpha of the inner mouse drag selection box."), Range(0f, 1f)]
    [SerializeField] private float _selectionBoxAlpha = 0.2f;
    [Tooltip("Layer of the clickable objects.")]
    [SerializeField] private LayerMask _unitLayer = ~0;

    private readonly float _maxDistance = 50f; // In unity units.
    private readonly float _dragThreshold = 4f; // In pixel.

    private Vector2 _startPosition;
    private Rect _selectionRect;

    private bool _reachedDragThreshold = false;
    private bool _isMouseDown = false;
    private bool _drawSelectionBox = false;

    private Camera _camera;
    private Texture2D _selectionBoxBorder;
    private Texture2D _selectionBoxFill;
    private Unit _unit;

    private void Start()
    {
        _selectionBoxBorder = new Texture2D(1, 1);
        _selectionBoxBorder.SetPixel(0, 0, new Color(1f, 1f, 1f, 1f));
        _selectionBoxBorder.Apply();

        _selectionBoxFill = new Texture2D(1, 1);
        _selectionBoxFill.SetPixel(0, 0, new Color(1f, 1f, 1f, _selectionBoxAlpha));
        _selectionBoxFill.Apply();

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
                    _unit = hitInfo.transform.gameObject.GetComponent<Unit>();
                    if (_unit)
                    {
                        Click(_unit);
                    }
                }
                else
                {
                    if (!UseModifierKey())
                    {
                        Unit.DeselectAllUnits();
                    }
                }
            }

            _isMouseDown = false;
        }
    }

    private void OnGUI()
    {
        if (_drawSelectionBox)
        {
            GUI.DrawTexture(ScreenToGuiSpace(_selectionRect), _selectionBoxFill, ScaleMode.StretchToFill, true, 1f, _selecteionBoxColor, 0f, 0f);
            GUI.DrawTexture(ScreenToGuiSpace(_selectionRect), _selectionBoxBorder, ScaleMode.StretchToFill, true, 1f, _selecteionBoxColor, 1f, 0f);
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
        _drawSelectionBox = true;
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
    }

    private void EndDrag()
    {
        _drawSelectionBox = false;

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

    private Rect ScreenToGuiSpace(Rect rect)
    {
        rect.y = Screen.height - rect.y - rect.height;
        return rect;
    }
}
