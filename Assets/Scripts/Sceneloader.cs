using UnityEngine;

public class Sceneloader : MonoBehaviour
{
    public void Quit(){
        Application.Quit();
    }

    public void LoadScene(int sceneIndex){
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }
}
