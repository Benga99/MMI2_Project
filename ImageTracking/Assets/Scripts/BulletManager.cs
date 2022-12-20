using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;


public class BulletManager : MonoBehaviour, IPunObservable
{
    public GameObject bulletPrefab;
    public GameObject DirectionObject;
    public TextMeshProUGUI debugText;
    public HealthBar healthBarMe;
    public HealthBar healthBarEnemy;


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
        healthBarMe.SetMaxHealth(maxHealth);
        canvasMan = FindObjectOfType<CanvasManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //debugText.text = "healthBarEnemy is null: " + (healthBarEnemy == null).ToString();
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
        debugText.text = "in TakeDamage";
        if (healthBarEnemy.health >= damage)
        {
            //currentHealth -= damage;
            debugText.text += "Hit Enemy";
            healthBarEnemy.SetHealth(healthBarEnemy.health - damage);
        }

        if (healthBarEnemy.health <= 0)
        {
            canvasMan.ActivateGameWin();
        }
        else if(healthBarMe.health <= 0)
        {
            canvasMan.ActivateGameLose();
        }
    }

    public void SetHealthBarEnemy(HealthBar h)
    {
        healthBarEnemy = h;
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // If we are the owner of this character, we are allowed to write something to the stream that gets send to all other clients.
        if (stream.IsWriting)
        {
            debugText.text += "Sent Enemy Health: " + healthBarEnemy.health;
            // Here we send the current health to the stream.
            Debug.Log("sent " + healthBarEnemy.health);
            stream.SendNext(healthBarEnemy.health);
        }
        // If we are not the owner, we just want to receive the new value and show it accordingly.
        else
        {
            int newHealth = (int)stream.ReceiveNext();
            debugText.text += "Received own Health: " + newHealth;
            Debug.Log("received " + newHealth);
            // Here we set the local health variable to the one we got back form the stream. What we get from the stream is simple bits/bytes, so we first have to cast it to float with the '(float)' function.
            //healthBarMe.health = (int)stream.ReceiveNext();
            healthBarMe.SetHealth(newHealth);
            if (healthBarMe.health <= 0)
            {
                canvasMan.ActivateGameLose();
            }
        }

    }
}
