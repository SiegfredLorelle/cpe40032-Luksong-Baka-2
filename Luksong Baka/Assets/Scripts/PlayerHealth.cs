using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Attached to player in Luksong Baka Scene
    // Manages player's health

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public int currentLives;
    private int maximumLives = 3;

    void Start()
    {
        // Set current lives to max and setup hearts UI above the player
        currentLives = maximumLives;
        SetupHearts();
    }

    // Called at start method
    // Set the sprite of the hearts to be a full heart
    void SetupHearts()
    {
        foreach (Image heart in hearts)
        {
            heart.sprite = fullHeart;
        }
    }

    // Called in player controller script when colliding with obstacles
    // Decrement current lives and remove a heart
    public void TakeDamage()
    {
        currentLives--;
        RemoveAHeart();
    }

    // Called when taking damage
    // Not technically removing the heart
    // Just emptying a full heart
    private void RemoveAHeart()
    {
        // Hearts array are in reverse so that the heart to be emptied will the rightmost full heart
        foreach (Image heart in hearts.Reverse())
        {
            if (heart.sprite == fullHeart)
            {
                heart.sprite = emptyHeart;
                break;
            }
        }
    }

    // Called in player controller script when player picked up a heart
    // Increase lives and refill heart if not full
    public void Heal()
    {
        if (currentLives < 3)
        {
            currentLives++;
            AddAHeart();
        }
    }

    // Called when healing
    // Refills the left most empty heart
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
