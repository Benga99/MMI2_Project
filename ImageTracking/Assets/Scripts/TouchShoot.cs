using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchShoot : MonoBehaviour
{

    private InputManager inputManager;
    private Camera cameraMain;
    private List<GameObject> spawnedProjectiles = new List<GameObject>();
    public GameObject projectilePrefab;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        cameraMain = Camera.main;
    }


    private void OnEnable()
    {
        inputManager.OnStartTouch += SpawnProjectile;
    }

    private void OnDisable()
    {
        inputManager.OnEndTouch -= SpawnProjectile;
    }

    public void SpawnProjectile(Vector2 screenPosition, float time)
    {
        Vector3 screenCoordinates = new Vector3(screenPosition.x, screenPosition.y, cameraMain.nearClipPlane);
        GameObject projectile = Instantiate(projectilePrefab, screenCoordinates, projectilePrefab.transform.rotation);
        spawnedProjectiles.Add(projectile);
    }

    /*public void Move(Vector2 screenPosition, float time)
    {
        Vector3 screenCoordinates = new Vector3(screenPosition.x, screenPosition.y, cameraMain.nearClipPlane);
        Vector3 worldCoordinates = cameraMain.ScreenToWorldPoint(screenCoordinates);
        //worldCoordinates.z = 0;
        transform.position = worldCoordinates;
    }*/

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject projectile in spawnedProjectiles)
        {
            projectile.transform.position = new Vector3(projectile.transform.position.x, projectile.transform.position.y, projectile.transform.position.z + 0.01f);
        }
    }
}
