using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletManager : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject DirectionObject;
    public TextMeshProUGUI debugText;

    private RaycastHit hit;
    private Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootButtonPressed()
    {

        GameObject bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
        bullet.transform.eulerAngles = new Vector3(90, 0, -Camera.main.transform.eulerAngles.y);
        StartCoroutine(moveBullet(bullet));


        ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.tag == "HitBox")
            {
                debugText.text += "Nice shot!\n";
            }
        }

    }

    private IEnumerator moveBullet(GameObject bullet)
    {
        Vector3 initialCameraForward = Camera.main.transform.forward;
        Vector3 initialCameraPosition = Camera.main.transform.position;
        while (bullet != null && Vector3.Distance(initialCameraForward, initialCameraPosition) < 500)
        {
            bullet.transform.position += initialCameraForward * Time.deltaTime * 1000;
            yield return new WaitForEndOfFrame();
        }
    }
}
