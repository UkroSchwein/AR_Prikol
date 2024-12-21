using System;
using System.Collections;
using System.Collections.Generic;
using PaleLuna.DataHolder.Dictionary;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager ))]
public class ImageTracking : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager _arTrackedImageManager;

    [SerializeField]
    private SerializedDictionary<GameObject> _prefabs;
    
    private readonly Dictionary<string, GameObject> _spawnedObjects = new();

    #region [ MonoBehaviour Methods ]

    private void OnValidate() => _arTrackedImageManager ??= GetComponent<ARTrackedImageManager>();
    private void OnEnable() => _arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    private void OnDisable() => _arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;

    private void Start()
    {
        var prefabs = _prefabs.Convert();

        foreach (string key in prefabs.GetKeys())
        {
            GameObject gObj = Instantiate(prefabs[key]);
            gObj.SetActive(false);
            
            _spawnedObjects.Add(key, gObj);
        }

        _prefabs = null;
    }

    #endregion
    
    
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        // Обработка добавленных изображений
        foreach (ARTrackedImage trackedImage in args.added)
        {
            SpawnPrefab(trackedImage);
        }

        // Обновление существующих изображений
        foreach (ARTrackedImage trackedImage in args.updated)
        {
            UpdatePrefab(trackedImage);
        }

        // Удаление изображений
        foreach (ARTrackedImage trackedImage in args.removed)
        {
            if (_spawnedObjects.TryGetValue(trackedImage.referenceImage.name, out GameObject obj))
            {
                Destroy(obj);
                _spawnedObjects.Remove(trackedImage.referenceImage.name);
            }
        }
    }

    private void SpawnPrefab(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        GameObject gObj = _spawnedObjects[imageName];
        if (gObj == null) return;
        
        gObj.SetActive(true);
        gObj.transform.parent = trackedImage.transform;
    }
    
    private void UpdatePrefab(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (!_spawnedObjects.TryGetValue(imageName, out GameObject obj)) return;
        
        obj.transform.position = trackedImage.transform.position;
        obj.transform.rotation = trackedImage.transform.rotation;
    }
}
