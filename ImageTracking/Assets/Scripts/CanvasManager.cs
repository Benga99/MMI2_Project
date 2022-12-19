using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject gamePanel;
    public GameObject winPanel;
    public GameObject losePanel;

    public GameObject waitForConnectionText;

    public HealthBar healthBarMe;
    public HealthBar healthBarEnemy;

    private BulletManager bManager;

    // Start is called before the first frame update
    void Start()
    {
        bManager = FindObjectOfType<BulletManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(healthBarEnemy == null)
        {
            var healths = FindObjectsOfType<HealthBar>();
            foreach(var h in healths)
            {
                if(h.identifier == "enemy")
                {
                    healthBarEnemy = h;
                    break;
                }
            }
        }
    }

    public void PlayButtonPressed()
    {
        waitForConnectionText.SetActive(true);

        // do the connection

        gamePanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void QuitButtonPressed()
    {
        Application.Quit();
    }

    public void ActivateGameWin()
    {
        winPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    public void ActivateGameLose()
    {
        losePanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    public void TryAgainButtonPressed()
    {
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        gamePanel.SetActive(true);
        bManager.currentHealth = bManager.maxHealth;
        healthBarMe.SetMaxHealth(bManager.maxHealth);
        healthBarMe.SetHealth(bManager.maxHealth);
        healthBarEnemy.SetMaxHealth(bManager.maxHealth);
        healthBarEnemy.SetHealth(bManager.maxHealth);
    }
}
