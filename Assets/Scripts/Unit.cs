using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // HashSet of all units.
    public static IReadOnlyCollection<Unit> Units => _units;
    private static readonly HashSet<Unit> _units = new HashSet<Unit>();

    // HashSet of all units currently visible on screen.
    public static IReadOnlyCollection<Unit> UnitsOnScreen => _unitsOnScreen;
    private static readonly HashSet<Unit> _unitsOnScreen = new HashSet<Unit>();

    // HashSet of all units currently selected.
    public static IReadOnlyCollection<Unit> UnitsSelected => _unitsSelected;
    private static readonly HashSet<Unit> _unitsSelected = new HashSet<Unit>();

    public Rect Bounds2D { get => UpdateBounds(); } // Units 2D bounds in screenspace.

    public bool IsSelected { get; private set; }
    public bool IsVisible { get; private set; }

    private Renderer _renderer; // Used for bounds.

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        _units.Add(this);
    }

    private void OnGUI()
    {
        if (IsSelected)
        {
            // Stuff that should happen, if unit is selected.
        }
    }

    // Calculates the 2D bounds in screenspace.
    private Rect UpdateBounds()
    {
        return Helpers.GetScreenRect(_renderer.bounds);
    }

    public void Click()
    {
        if (_unitsSelected.Contains(this))
        {
            Deselect();
            return;
        }
        Select();
    }

    public void Select()
    {
        IsSelected = true;
        _unitsSelected.Add(this);

        _renderer.material.color = new Color(0f, 0.65f, 1f); // Test code.
    }

    public void Deselect()
    {
        _unitsSelected.Remove(this);
        Deselect_2();
    }

    private void Deselect_2()
    {
        IsSelected = false;

        _renderer.material.color = new Color(0.9f, 0.9f, 0.9f); // Test code.
    }

    public static void DeselectAll()
    {
        foreach (var unit in _unitsSelected)
        {
            unit.Deselect_2();
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
            Deselect();
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
