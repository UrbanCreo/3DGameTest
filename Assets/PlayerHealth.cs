using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    Text healthText;

    void Start()
    {
        healthText = GameObject.FindGameObjectWithTag("HP").GetComponent<Text>();
        health = int.Parse(healthText.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (health != int.Parse(healthText.text))
        {
            healthText.text = health.ToString();
        }
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//Pokud zdraví hráèe klesne na nulu nebo nižší, scénu resetuje GetActiveScene().buildIndex
        }
    }
}