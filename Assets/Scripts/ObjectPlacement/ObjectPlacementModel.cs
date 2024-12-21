using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPlacementModel : MonoBehaviour
{
    [SerializeField]
    private Transform[] _prefabs;
    
    private readonly List<Transform> _spawnedTransforms = new();
    
    public Transform currentTransform {get; private set;}
    public GameObject currentGObj => currentTransform.gameObject;

    private void Start()
    {
        foreach (Transform prefab in _prefabs)
        {
            var transformInstantiate = Instantiate(prefab);
            transformInstantiate.gameObject.SetActive(false);
            
            _spawnedTransforms.Add(transformInstantiate);
        }
        currentTransform = _spawnedTransforms.First();

        _prefabs = null;
    }

    public void SetCurrentTransform(int index)
    {
        currentTransform = _spawnedTransforms[index]; 
    }
}
