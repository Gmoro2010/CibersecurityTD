using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class PausaPartida : MonoBehaviour
{
    [SerializeField] private Button pausar;
    [SerializeField] private GameObject pantalla;
    [SerializeField] private Button continuar;
    [SerializeField] private Button volver;

    private CanvasGroup hud;
    private float escalaAnterior;
    private bool pausada;
    private bool cargandoMenu;

    private void Awake()
    {
        hud = GetComponentInParent<CanvasGroup>();
    }

    private void OnEnable()
    {
        pausar.onClick.AddListener(Pausar);
        continuar.onClick.AddListener(Continuar);
        volver.onClick.AddListener(VolverAlMenu);
    }

    private void OnDisable()
    {
        pausar.onClick.RemoveListener(Pausar);
        continuar.onClick.RemoveListener(Continuar);
        volver.onClick.RemoveListener(VolverAlMenu);
        if (pausada) RestaurarPartida();
        cargandoMenu = false;
    }

    private void OnApplicationFocus(bool tieneFoco)
    {
        if (!tieneFoco && AjustesJuego.PausarAlPerderFoco) Pausar();
    }

    private void Pausar()
    {
        if (pausada || !hud.interactable || Time.timeScale == 0f) return;

        pausada = true;
        escalaAnterior = Time.timeScale;
        Time.timeScale = 0f;
        hud.interactable = false;
        pantalla.SetActive(true);
        EventSystem.current?.SetSelectedGameObject(continuar.gameObject);
    }

    private void Continuar()
    {
        if (!pausada || cargandoMenu) return;

        RestaurarPartida();
        EventSystem.current?.SetSelectedGameObject(pausar.gameObject);
    }

    private void RestaurarPartida()
    {
        pausada = false;
        Time.timeScale = escalaAnterior;
        if (hud != null) hud.interactable = true;
        if (pantalla != null) pantalla.SetActive(false);
    }

    private void VolverAlMenu()
    {
        if (!pausada || cargandoMenu) return;

        cargandoMenu = true;
        SceneManager.LoadScene(ResultadoPartida.Menu);
    }
}
