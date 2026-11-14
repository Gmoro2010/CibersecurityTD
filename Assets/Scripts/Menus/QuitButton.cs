using UnityEngine;

public class QuitButton : MonoBehaviour
{
    // Llama a esta función desde el evento OnClick del botón
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");

#if UNITY_EDITOR
        // En el Editor, Application.Quit() no cierra nada, así que detenemos el Play Mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}