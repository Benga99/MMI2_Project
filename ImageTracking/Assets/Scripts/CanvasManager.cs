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

    public HealthBar healthBar;

    private BulletManager bManager;

    // Start is called before the first frame update
    void Start()
    {
        bManager = FindObjectOfType<BulletManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        healthBar.SetMaxHealth(bManager.maxHealth);
        healthBar.SetHealth(bManager.maxHealth);
    }
}
