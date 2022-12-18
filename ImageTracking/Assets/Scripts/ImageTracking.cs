using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

//Script is based on this tutorial to spawn and use multiple objects: https://www.youtube.com/watch?v=I9j3MD7gS5Y&list=PL9z3tc0RL6Z4WenhJiJieCcrPVNxYszod&index=9

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    [SerializeField]
    private GameObject[] placeablePrefabs;
    private List<GameObject> spawnedPrefabsList = new List<GameObject>();
    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackedImageMananger;
    public float crosshairup;

    private void Awake()
    {
        trackedImageMananger = FindObjectOfType<ARTrackedImageManager>();
        int i = 0;
        foreach (GameObject prefab in placeablePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, new Vector3(0, 1000, 0), prefab.transform.rotation);
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(i.ToString(), newPrefab);
            spawnedPrefabsList.Add(newPrefab);
            i++;
        }
    }

    private void Update()
    {
        debugText.text = spawnedPrefabsList[0].transform.position.ToString();
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
            //debugText.text = "added + " + trackedImage.trackingState.ToString();
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            //debugText.text = "update + " + trackedImage.trackingState.ToString();
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            //debugText.text = "removed + " + trackedImage.trackingState.ToString();
            spawnedPrefabs[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        Vector3 position = trackedImage.transform.position;
        GameObject hitbox = spawnedPrefabsList[0];
        hitbox.transform.position = position;
        hitbox.SetActive(true);

        /*
        // TODO: Crosshair rotation has to be changed according to the trackedImage
        GameObject crosshair = spawnedPrefabs["crosshair"];
        Vector3 up = new Vector3(0, crosshairup, 0);
        crosshair.transform.position = position + up;
        crosshair.SetActive(true);
        */
    }
}
