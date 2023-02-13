using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public int currentLives;
    private int maximumLives;

    // Start is called before the first frame update
    void Start()
    {
        maximumLives = 3;

        // FOR TESTING ONLY
        //maximumLives = 100;

        currentLives = maximumLives;

        SetupHearts();
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    void SetupHearts()
    {
        foreach (Image heart in hearts)
        {
            heart.sprite = fullHeart;
        }
    }

    public void TakeDamage()
    {
        currentLives--;
        RemoveAHeart();
    }

    private void RemoveAHeart()
    {
        foreach (Image heart in hearts)
        {
            if (heart.sprite == fullHeart)
            {
                heart.sprite = emptyHeart;
                break;
            }
        }
    }
}
