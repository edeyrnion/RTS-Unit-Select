using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private Camera _camera;
    private readonly float _maxDistance = 50f;

    [SerializeField] private GUIStyle _mouseDragSkin;
    private Rect _selectionBox;

    private Vector3 _mouseDownPosition; // Worldspace
    private readonly float _mouseClickThreshold = 1f;

    private readonly float _dragThreshold = 4f;
    private bool _reachedDragThreshold = false;

    List<GameObject> _unitsInDragBox = new List<GameObject>();

    private bool _isDragging = false;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_isDragging)
        {
            _selectionBox = Helpers.GetScreenRect(_camera.WorldToScreenPoint(_mouseDownPosition), Input.mousePosition);

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
                _mouseDownPosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.nearClipPlane));
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
                        Events.UnitSelect.Invoke(target, UseModifierKey());
                    }
                    else if (!UseModifierKey())
                    {
                        Events.UnitDeselect.Invoke();
                    }
                }
                else
                {
                    GameObject[] targets = DragSelect(_selectionBox);

                    Events.UnitDragSelect.Invoke(targets, UseModifierKey());
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
        if (Vector3.Distance(_camera.WorldToScreenPoint(_mouseDownPosition), Input.mousePosition) < _dragThreshold)
        {
            return true;
        }

        return false;
    }

    private GameObject[] DragSelect(Rect box)
    {
        _unitsInDragBox.Clear();

        Vector2 temp = (_camera.WorldToScreenPoint(_mouseDownPosition) + Input.mousePosition) * 0.5f;

        foreach (var unit in Unit.AllUnits)
        {
            Vector2 pos = _camera.WorldToScreenPoint(unit.transform.position);

            if (Mathf.Abs(pos.x - temp.x) < (Mathf.Abs(box.width) * 0.5f) && Mathf.Abs(pos.y - temp.y) < (Mathf.Abs(box.height) * 0.5f))
            {
                _unitsInDragBox.Add(unit);
            }
        }
        GameObject[] targets = _unitsInDragBox.ToArray();
        return targets;
    }

    private bool UseModifierKey()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
}
