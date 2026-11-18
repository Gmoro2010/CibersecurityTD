using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class JugarNivel : MonoBehaviour
{
    [SerializeField] private string nombreEscena = "Mapa1";
    [SerializeField] private GameObject indicadorBloqueo;
    [SerializeField] private TMP_Text etiquetaNivel;
    private bool cargando;

    private bool NivelDisponible => nombreEscena != ResultadoPartida.SegundoMapa || ResultadoPartida.Mapa2Desbloqueado;

    private void OnEnable()
    {
        GetComponent<Button>().interactable = NivelDisponible;
        if (indicadorBloqueo != null)
            indicadorBloqueo.SetActive(!NivelDisponible);
        if (etiquetaNivel != null)
            etiquetaNivel.text = NivelDisponible ? "NIVEL 2" : "NIVEL 2 - BLOQUEADO";
    }

    public void CargarNivel()
    {
        if (!NivelDisponible || cargando) return;

        if (Application.CanStreamedLevelBeLoaded(nombreEscena))
        {
            cargando = true;
            Time.timeScale = 1f;
            SceneManager.LoadScene(nombreEscena);
        }
        else
        {
            Debug.LogWarning("La escena '" + nombreEscena + "' no existe o no está en Build Profiles.");
        }
    }
}
