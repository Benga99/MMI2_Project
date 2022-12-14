using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

//Script is based on this tutorial to spawn and use multiple objects: https://www.youtube.com/watch?v=I9j3MD7gS5Y&list=PL9z3tc0RL6Z4WenhJiJieCcrPVNxYszod&index=9

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeablePrefabs;
    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackedImageMananger;
    public float crosshairup;

    private void Awake()
    {
        trackedImageMananger = FindObjectOfType<ARTrackedImageManager>();

        foreach(GameObject prefab in placeablePrefabs) 
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, prefab.transform.rotation);
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(prefab.name, newPrefab);

        }
    }

    private void OnEnable()
    {
        trackedImageMananger.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        trackedImageMananger.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            spawnedPrefabs[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        Vector3 position = trackedImage.transform.position;
        GameObject hitbox = spawnedPrefabs["hitbox"];
        hitbox.transform.position = position;
        hitbox.SetActive(true);

        // TODO: Crosshair rotation has to be changed according to the trackedImage
        GameObject crosshair = spawnedPrefabs["crosshair"];
        Vector3 up = new Vector3(0, crosshairup, 0);
        crosshair.transform.position = position + up;
        crosshair.SetActive(true);

    }
}
