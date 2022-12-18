using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CubeManager : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    // Start is called before the first frame update
    void Start()
    {
        debugText = GameObject.Find("DebugText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            debugText.text += ("\ntrigger with " + other.gameObject.name);
            Destroy(other.gameObject);
        }
    }
}
