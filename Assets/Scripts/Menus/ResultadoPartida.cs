using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class ResultadoPartida : MonoBehaviour
{
    public const string Menu = "Menu principal";
    public const string PrimerMapa = "Mapa1";
    public const string SegundoMapa = "Mapa2";
    private const string ClaveMapa2 = "mapa2Desbloqueado";

    [SerializeField] private OleadaManip oleadas;
    [SerializeField] private NucleoVida nucleo;
    [SerializeField] private CanvasGroup hud;
    [SerializeField] private GameObject pantalla;
    [SerializeField] private Image borde;
    [SerializeField] private TMP_Text titulo;
    [SerializeField] private TMP_Text mensaje;
    [SerializeField] private Button volver;

    private bool resultadoMostrado;
    private bool cargandoMenu;

    public static bool Mapa2Desbloqueado => PlayerPrefs.GetInt(ClaveMapa2, 0) == 1;

    private void OnEnable()
    {
        oleadas.OnAllWavesCompleted += MostrarVictoria;
        nucleo.Destruido += MostrarDerrota;
        volver.onClick.AddListener(VolverAlMenu);
    }

    private void OnDisable()
    {
        oleadas.OnAllWavesCompleted -= MostrarVictoria;
        nucleo.Destruido -= MostrarDerrota;
        volver.onClick.RemoveListener(VolverAlMenu);
    }

    private void MostrarVictoria()
    {
        if (resultadoMostrado) return;

        bool desbloqueo = SceneManager.GetActiveScene().name == PrimerMapa && !Mapa2Desbloqueado;
        if (desbloqueo)
        {
            PlayerPrefs.SetInt(ClaveMapa2, 1);
            PlayerPrefs.Save();
        }

        Mostrar("VICTORIA", desbloqueo ? "Núcleo defendido. Mapa 2 desbloqueado."
            : "Defendiste el núcleo durante todas las oleadas.", new Color32(100, 248, 196, 255));
    }

    private void MostrarDerrota()
    {
        string detalle = oleadas.OleadaActual > 0
            ? $"El núcleo cayó en la oleada {oleadas.OleadaActual} de {oleadas.TotalOleadas}."
            : "El núcleo fue destruido.";
        Mostrar("DERROTA", detalle, new Color32(255, 100, 123, 255));
    }

    private void Mostrar(string encabezado, string detalle, Color color)
    {
        if (resultadoMostrado) return;
        resultadoMostrado = true;
        oleadas.DetenerPartida();
        Time.timeScale = 0f;

        titulo.text = encabezado;
        titulo.color = color;
        borde.color = color;
        mensaje.text = detalle;
        hud.interactable = false;
        pantalla.SetActive(true);
        EventSystem.current?.SetSelectedGameObject(volver.gameObject);
    }

    private void VolverAlMenu()
    {
        if (cargandoMenu) return;
        cargandoMenu = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(Menu);
    }
}
