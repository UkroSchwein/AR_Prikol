using System;
using TMPro;
using UnityEngine;

public class ObjectPlacementView : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown _dropdown;

    public Action<int> onDropDownChange;

    private void Awake()
    {
        _dropdown.onValueChanged.AddListener(value => onDropDownChange?.Invoke(value));
    }
}
