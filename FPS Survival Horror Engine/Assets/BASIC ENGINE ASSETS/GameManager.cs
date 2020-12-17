using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    float restartTime = 14f;
    public Animator textAnim;
    public Animator deathScreen;

    void Start()
    {
        textAnim = GameObject.Find("Equipped Name").GetComponent<Animator>();
        deathScreen = GameObject.Find("YOU ARE DEAD").GetComponent<Animator>();
    }

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Invoke("YouAreDead", 2f);
            Debug.Log("Game Over");
            Invoke ("Restart", restartTime);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    void YouAreDead()
    {
        deathScreen.SetTrigger("YouAreDead");
    }

    public void SlotTextFade()
    {
        textAnim.SetTrigger("SlotSelect");
    }

}
