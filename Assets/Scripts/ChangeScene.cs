using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    //Utilizaremos loadscene para cambiar de escena cuando se llame a la funcion en los botones con el canvas
    public void StartScene() 
    {
        SceneManager.LoadScene("LoreScene");
    }

    public void HallwayScene()
    {
        SceneManager.LoadScene("HallwayScene");
    }
    public void classScene()
    {
        SceneManager.LoadScene("classScene");
    }

    public void InstructionsScene()
    {
        SceneManager.LoadScene("InstructionsScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}