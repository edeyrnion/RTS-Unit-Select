using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static IReadOnlyCollection<Unit> Units => _units;
    private static readonly HashSet<Unit> _units = new HashSet<Unit>();

    public static IReadOnlyCollection<Unit> UnitsOnScreen => _unitsOnScreen;
    private static readonly HashSet<Unit> _unitsOnScreen = new HashSet<Unit>();

    public static IReadOnlyCollection<Unit> UnitsSelected => _unitsSelected;
    private static readonly HashSet<Unit> _unitsSelected = new HashSet<Unit>();

    public Rect Bounds2D { get => UpdateBounds(); }
    public bool IsSelected { get; private set; }
    public bool IsVisible { get; private set; }

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        _units.Add(this);
    }

    private Rect UpdateBounds()
    {
        return Helpers.GetScreenRect(_renderer.bounds);
    }

    public void ClickUnit()
    {
        if (_unitsSelected.Contains(this))
        {
            DeselectUnit();
            return;
        }
        SelectUnit();
    }

    public void SelectUnit()
    {
        IsSelected = true;
        _unitsSelected.Add(this);
        _renderer.material.color = new Color(0f, 0.65f, 1f); // Test code.
    }

    public void DeselectUnit()
    {
        _unitsSelected.Remove(this);
        DeselectUnit_2();
    }

    private void DeselectUnit_2()
    {
        IsSelected = false;
        _renderer.material.color = new Color(0.9f, 0.9f, 0.9f); // Test code.
    }

    public static void DeselectAllUnits()
    {
        foreach (var unit in _unitsSelected)
        {
            unit.DeselectUnit_2();
        }

        _unitsSelected.Clear();
    }

    private void OnBecameVisible()
    {
        IsVisible = true;
        _unitsOnScreen.Add(this);
    }

    private void OnBecameInvisible()
    {
        IsVisible = false;
        _unitsOnScreen.Remove(this);
    }

    private void OnDisable()
    {
        _units.Remove(this);

        if (IsSelected)
            DeselectUnit();
    }

    private void OnDestroy()
    {
        if (_units.Contains(this))
            _units.Remove(this);

        if (_unitsOnScreen.Contains(this))
            _unitsOnScreen.Remove(this);

        if (_unitsSelected.Contains(this))
            _unitsSelected.Remove(this);
    }
}
