using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class JugarNivel : MonoBehaviour
{
    [SerializeField] private string nombreEscena = "Mapa1";

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(CargarNivel);
    }

    public void CargarNivel()
    {
        if (Application.CanStreamedLevelBeLoaded(nombreEscena))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(nombreEscena);
        }
        else
        {
            Debug.LogWarning("La escena '" + nombreEscena + "' no existe o no está en Build Profiles.");
        }
    }
}