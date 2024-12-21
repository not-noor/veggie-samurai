using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TextMeshProUGUI scoreText; 
    public int best { get; private set; } = 0;
    public TextMeshProUGUI bestText;
    public TextMeshProUGUI gameOverText; 
    public int score { get; private set; } = 0;
    private int lives = 3; 
    public Animator gameOverAnimator;
    public GameObject[] hearts;
    public AudioClip bombExplosionSound;
    public AudioClip gameOverSound;
    private AudioSource audioSource;

    // Cooldown to prevent rapid heart removal
    private bool canLoseLife = true;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        best = PlayerPrefs.GetInt("BestScore", 0);
        if (bestText != null)
        {
            bestText.text = best.ToString(); // Update the UI with the current best score
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void IncreaseScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
        if(score>best) {
            best = score;
            PlayerPrefs.SetInt("BestScore",best);
            PlayerPrefs.Save();
            bestText.text = best.ToString();
        }
    }

    public void ResetScore()
    {
        score = 0;
        scoreText.text = score.ToString();
    }

    public void HandleBombExplosion()
    {
        if (!canLoseLife) return; // Prevent rapid life removal

        StartCoroutine(LoseLifeCooldown());

        audioSource.PlayOneShot(bombExplosionSound);

        // Decrease lives and update hearts
        lives--;
        ResetScore();
        if (lives >= 0 && lives < hearts.Length)
        {
            hearts[lives].SetActive(false); // Hide the corresponding heart
        }

        if (lives <= 0)
        {
            StartCoroutine(GameOver());
        }
    }
    private IEnumerator LoseLifeCooldown()
    {
        canLoseLife = false;
        yield return new WaitForSeconds(3f);
        canLoseLife = true;
    }

    private IEnumerator GameOver()
    {
        Spawner spawner = FindObjectOfType<Spawner>();
        spawner.enabled = false;
        yield return new WaitForSeconds(2);

        audioSource.PlayOneShot(gameOverSound);

        gameOverText.gameObject.SetActive(true); // Activate the Game Over TMP text

        yield return new WaitForSeconds(2f);
        gameOverAnimator.SetTrigger("GameOver");

        yield return new WaitForSeconds(1.20f);

        SceneManager.LoadScene(0);
    }
}