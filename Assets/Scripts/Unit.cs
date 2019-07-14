using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Material _normal;
    [SerializeField] private Material _selected;
    [SerializeField] private GUIStyle _unitBox;

    public readonly static List<GameObject> AllUnits = new List<GameObject>(); // Old.
    public static IReadOnlyList<Unit> Units => _units;
    private readonly static List<Unit> _units = new List<Unit>();

    public static IReadOnlyList<Unit> UnitsOnScreen => _unitsOnScreen;
    private readonly static List<Unit> _unitsOnScreen = new List<Unit>();

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
        AllUnits.Add(gameObject); // Old.
        _units.Add(this);
    }

    private void OnBecameVisible()
    {
        _unitsOnScreen.Add(this);
    }

    private void Start()
    {
        Events.UpdateUnits.AddListener(UpdateUnitsHandler);
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

    private void UpdateUnitsHandler(List<GameObject> units)
    {
        if (units.Contains(gameObject))
        {
            Renderer.material = _selected;
        }
        else
        {
            Renderer.material = _normal;
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
        AllUnits.Remove(gameObject); // Old.
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
