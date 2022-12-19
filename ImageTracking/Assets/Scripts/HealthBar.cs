using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealthBar : MonoBehaviourPunCallbacks, IPunObservable
{
    public string identifier;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public int health;

    private void Start()
    {
        if(identifier == "enemy")
        {
            var bM = FindObjectOfType<BulletManager>();
            bM.SetHealthBarEnemy(this);
        }
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        this.health = health;

        fill.color = gradient.Evaluate(1f);
    }
    
    public void SetHealth(int health)
    {
        slider.value = health;
        this.health = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    void Update()
    {
        // This sets the text of the UI text element to be the health value. The 'ToString()' converts the number (float) into text (string).
        

        // This is only true, when we are the local player controlling this character. If this is called on any other client, this will be false and they can not control our character.
        //if (photonView.IsMine) {
           //SetHealth(health);
            // This uses the Input Axis predefined by Unity to create a 3D-Vector of where the character should move. Hotizontal is the Axis controlled by Left/Right-Arrow or by the A/D-keys. Vertical is controlled by Up/Down-Arrow or W/S-keys.
            //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            //charControl.Move(move * Time.deltaTime * speed);

        //}
    }

    
    // This is called every time Photon updates.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonSerializeView");
        // If we are the owner of this character, we are allowed to write something to the stream that gets send to all other clients.
        if (stream.IsWriting)
        {
            Debug.Log("Writing");
            if (identifier == "enemy")
            {
                // Here we send the current health to the stream.
                stream.SendNext(health);
                Debug.Log("sent " + health);
            }
        }
        // If we are not the owner, we just want to receive the new value and show it accordingly.
        else {
            // Here we set the local health variable to the one we got back form the stream. What we get from the stream is simple bits/bytes, so we first have to cast it to float with the '(float)' function.
            Debug.Log("Reading");
            if (identifier == "me")
            {
                health = (int)stream.ReceiveNext();
                Debug.Log("got " + health);
                slider.value = health;
                this.health = (int)slider.value;
                fill.color = gradient.Evaluate(slider.normalizedValue);
            }
        }
        
    }
    
}
