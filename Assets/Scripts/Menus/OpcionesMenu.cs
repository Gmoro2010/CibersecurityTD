using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class OpcionesMenu : MonoBehaviour
{
    [SerializeField] private OptionsButton navegacion;
    [SerializeField] private Toggle barrasEnemigas;
    [SerializeField] private Toggle reducirDestellos;
    [SerializeField] private Toggle pausaAlCambiarVentana;
    [SerializeField] private Button repetirTutorial;
    [SerializeField] private Button volver;

    private bool cargandoTutorial;

    private void OnEnable()
    {
        barrasEnemigas.SetIsOnWithoutNotify(AjustesJuego.MostrarBarrasEnemigas);
        reducirDestellos.SetIsOnWithoutNotify(AjustesJuego.ReducirDestellos);
        pausaAlCambiarVentana.SetIsOnWithoutNotify(AjustesJuego.PausarAlPerderFoco);

        barrasEnemigas.onValueChanged.AddListener(CambiarBarrasEnemigas);
        reducirDestellos.onValueChanged.AddListener(CambiarDestellos);
        pausaAlCambiarVentana.onValueChanged.AddListener(CambiarPausaAutomatica);
        repetirTutorial.onClick.AddListener(RepetirTutorial);
        volver.onClick.AddListener(Volver);
        EventSystem.current?.SetSelectedGameObject(barrasEnemigas.gameObject);
    }

    private void OnDisable()
    {
        barrasEnemigas.onValueChanged.RemoveListener(CambiarBarrasEnemigas);
        reducirDestellos.onValueChanged.RemoveListener(CambiarDestellos);
        pausaAlCambiarVentana.onValueChanged.RemoveListener(CambiarPausaAutomatica);
        repetirTutorial.onClick.RemoveListener(RepetirTutorial);
        volver.onClick.RemoveListener(Volver);
    }

    private void Update()
    {
        if (Keyboard.current?.escapeKey.wasPressedThisFrame == true) Volver();
    }

    private static void CambiarBarrasEnemigas(bool activo) => AjustesJuego.MostrarBarrasEnemigas = activo;
    private static void CambiarDestellos(bool activo) => AjustesJuego.ReducirDestellos = activo;
    private static void CambiarPausaAutomatica(bool activo) => AjustesJuego.PausarAlPerderFoco = activo;

    private void RepetirTutorial()
    {
        if (cargandoTutorial) return;
        cargandoTutorial = true;
        SceneManager.LoadSceneAsync(TutorialPartida.Escena);
    }

    private void Volver()
    {
        if (!cargandoTutorial) navegacion.CloseOptions();
    }
}
