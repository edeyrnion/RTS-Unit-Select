using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [SerializeField] private GUIStyle _mouseDragSkin;

    private Camera _camera;
    private readonly float _maxDistance = 50f;

    private Vector3 _mouseDownPosition; // Screenspace
    private Rect _selectionBox; // Screenspace
    private readonly float _mouseClickThreshold = 1f; // In pixel.
    private readonly float _dragThreshold = 4f; // In pixel;
    private bool _reachedDragThreshold = false;
    private bool _isDragging = false;

    private List<Unit> _unitsInDragBox = new List<Unit>();

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_isDragging)
        {
            _selectionBox = Helpers.GetScreenRect(_mouseDownPosition, Input.mousePosition);

            if ((Mathf.Abs(_selectionBox.width) >= _dragThreshold || Mathf.Abs(_selectionBox.height) >= _dragThreshold) && !_reachedDragThreshold)
            {
                _reachedDragThreshold = true;
            }
        }

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        LayerMask layerMask = (int)Layer.Unit | (int)Layer.Terrain;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, _maxDistance, layerMask))
        {
            GameObject target = hitInfo.collider.gameObject;

            if (Input.GetMouseButtonDown(0))
            {
                _mouseDownPosition = Input.mousePosition;
                _isDragging = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
                _reachedDragThreshold = false;

                if (IsLeftMouseClick())
                {
                    if (target.tag == "Unit")
                    {
                        Unit unit = target.GetComponent<Unit>();
                        Events.UnitSelect.Invoke(unit, UseModifierKey());
                    }
                    else if (!UseModifierKey())
                    {
                        Events.UnitDeselect.Invoke();
                    }
                }
                else
                {
                    Unit[] units = DragSelect(_selectionBox);

                    Events.UnitDragSelect.Invoke(units, UseModifierKey());
                }
            }
        }
    }

    private void OnGUI()
    {
        if (_isDragging && _reachedDragThreshold)
        {
            GUI.Box(Helpers.ScreenToGuiSpace(_selectionBox), "", _mouseDragSkin);
        }
    }

    private bool IsLeftMouseClick()
    {
        return (Vector3.Distance(_mouseDownPosition, Input.mousePosition) < _mouseClickThreshold);
    }

    private Unit[] DragSelect(Rect box)
    {
        _unitsInDragBox.Clear();

        foreach (var unit in Unit.UnitsOnScreen)
        {
            if (box.Overlaps(unit.Bounds2D))
            {
                _unitsInDragBox.Add(unit);
            }
        }
        Unit[] targets = _unitsInDragBox.ToArray();
        return targets;
    }

    private bool UseModifierKey()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
}
