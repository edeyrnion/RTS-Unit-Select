using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static List<GameObject> AllUnits = new List<GameObject>();

    public Material Normal;
    public Material Selected;

    private Renderer _renderer;

    [SerializeField] private GUIStyle _unitBox;

    private void OnEnable()
    {
        AllUnits.Add(gameObject);
    }

    private void Start()
    {
        Events.UpdateUnits.AddListener(UpdateUnitsHandler);
        _renderer = GetComponent<Renderer>();
    }

    private void OnGUI()
    {
        Rect rect = Helpers.GetScreenRect(_renderer);
        rect.y = Screen.height - rect.y - rect.height;

        GUI.Box(rect, "", _unitBox);
    }

    private void UpdateUnitsHandler(List<GameObject> units)
    {
        if (units.Contains(gameObject))
        {
            _renderer.material = Selected;
        }
        else
        {
            _renderer.material = Normal;
        }
    }

    private void OnDisable()
    {
        AllUnits.Remove(gameObject);
    }
}
