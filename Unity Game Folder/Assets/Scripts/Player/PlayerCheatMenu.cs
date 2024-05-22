using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCheatMenu : MonoBehaviour
{
    public void GoToLevelOne() => SceneManager.LoadScene("Level 1");
    public void GoToLevelTwo() => SceneManager.LoadScene("Level 2");
    public void GoToLevelThree() => SceneManager.LoadScene("Level 3");
    public void GoToLevelFour() => SceneManager.LoadScene("Level 4");
    public void GoToLevelFive() => SceneManager.LoadScene("Level 5");
    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
