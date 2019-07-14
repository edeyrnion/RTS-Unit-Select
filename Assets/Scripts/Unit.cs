using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Material _normal;
    [SerializeField] private Material _selected;
    [SerializeField] private GUIStyle _unitBox;

    public static IReadOnlyList<Unit> Units => _units;
    private static readonly List<Unit> _units = new List<Unit>();

    public static IReadOnlyList<Unit> UnitsOnScreen => _unitsOnScreen;
    private static readonly List<Unit> _unitsOnScreen = new List<Unit>();

    public Renderer Renderer { get; private set; }
    public Collider Collider { get; private set; }
    public Rect Bounds2D { get; private set; }
    public bool IsSelected { get; private set; }

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
        Collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        _units.Add(this);
    }

    private void OnBecameVisible()
    {
        _unitsOnScreen.Add(this);
    }

    private void Update()
    {
        if (Renderer.isVisible)
        {
            Bounds2D = Helpers.GetScreenRect(Renderer);
        }
    }

    private void OnGUI()
    {
        if (Renderer.isVisible && IsSelected)
        {
            GUI.Box(Helpers.ScreenToGuiSpace(Bounds2D), "", _unitBox);
        }
    }

    public void SelectUnit()
    {
        IsSelected = true;
    }

    public void DeselectUnit()
    {
        IsSelected = false;
    }

    private void OnBecameInvisible()
    {
        _unitsOnScreen.Remove(this);
    }

    private void OnDisable()
    {
        _units.Remove(this);
    }

    private void OnDestroy()
    {
        if (_units.Contains(this))
        {
            _units.Remove(this);
        }
        if (_unitsOnScreen.Contains(this))
        {
            _unitsOnScreen.Remove(this);
        }
    }
}
