using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletManager : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject DirectionObject;
    public TextMeshProUGUI debugText;
    public HealthBar healthBar;
    

    public int maxHealth = 100;
    public int currentHealth;
    public int damage = 5;

    private RaycastHit hit;
    private Ray ray;
    private CanvasManager canvasMan;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        canvasMan = FindObjectOfType<CanvasManager>();
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
            if (hit.transform.gameObject.CompareTag("HitBox"))
            {
                TakeDamage(damage);
            }
        }

    }

    void TakeDamage(int damage)
    {
        if(currentHealth >= damage)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            canvasMan.ActivateGameLose();
            //activate game win for the other player
        }
    }

    private IEnumerator moveBullet(GameObject bullet)
    {
        Vector3 initialCameraForward = Camera.main.transform.forward;
        Vector3 initialCameraPosition = Camera.main.transform.position;

        while (bullet != null && Vector3.Distance(initialCameraPosition, bullet.transform.position) < 2000)
        {
            bullet.transform.position += initialCameraForward * Time.deltaTime * 1000;
            yield return new WaitForEndOfFrame();
        }

        Destroy(bullet);
    }
}
