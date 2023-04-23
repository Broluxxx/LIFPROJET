using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class switch_gameover : MonoBehaviour
{
    public float timerDuration = 1;
    public float timerText;

    private bool isGameOver = false;

    void Update()
    {
        if (!isGameOver)
        {
            timerDuration -= Time.deltaTime;
            timerText = Mathf.Round(timerDuration);

            if (timerDuration <= 0f)
            {
                isGameOver = true;
                SceneManager.LoadScene("GameOver");
                
            }
        }
    }
}