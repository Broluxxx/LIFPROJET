using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public float timerDuration = 300f;
    public Text timerText;

    private bool isGameOver = false;

    void Update()
    {
        if (!isGameOver)
        {
            timerDuration -= Time.deltaTime;
            timerText.text = Mathf.Round(timerDuration).ToString();

            if (timerDuration <= 0f)
            {
                isGameOver = true;
                SceneManager.LoadScene("GameOver");
                
            }
        }
    }
}