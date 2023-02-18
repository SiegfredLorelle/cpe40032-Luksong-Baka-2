using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        foreach (Image heart in hearts.Reverse())
        {
            if (heart.sprite == fullHeart)
            {
                heart.sprite = emptyHeart;
                break;
            }
        }
    }

    public void Heal()
    {
        if (currentLives < 3)
        { 
            currentLives++;
            AddAHeart();
        }
        
    }

    private void AddAHeart()
    {
        foreach (Image heart in hearts)
        {
            if (heart.sprite == emptyHeart)
            {
                heart.sprite = fullHeart;
                break;
            }
        }
    }
}
