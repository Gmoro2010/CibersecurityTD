using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class TutorialPartida : MonoBehaviour
{
    public const string Escena = "Tutorial";
    private const string ClaveTutorial = "tutorialVisto";

    [SerializeField] private OleadaManip oleadas;
    [SerializeField] private BuildManager construccion;
    [SerializeField] private BaseTorre baseMarcada;
    [SerializeField] private SpriteRenderer indicador;
    [SerializeField] private TorreItem canon;
    [SerializeField] private TorreItem fuego;
    [SerializeField] private Button botonCanon;
    [SerializeField] private Button botonFuego;
    [SerializeField] private Button[] otrosBotones;
    [SerializeField] private Button omitir;
    [SerializeField] private TMP_Text titulo;
    [SerializeField] private TMP_Text instrucciones;

    private enum Paso
    {
        Pendiente,
        ColocarCanon,
        IniciarPrimeraOleada,
        PrimeraOleada,
        VenderCanon,
        ReponerCanon,
        Combinar,
        IniciarSegundaOleada,
        SegundaOleada
    }

    private bool saliendo;
    private bool ventaPracticada;
    private Paso pasoActual;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AbrirEnPrimerInicio()
    {
        if (PlayerPrefs.GetInt(ClaveTutorial, 0) == 0 &&
            SceneManager.GetActiveScene().name == ResultadoPartida.Menu)
            SceneManager.LoadSceneAsync(Escena);
    }

    public static void RecordarTutorial()
    {
        PlayerPrefs.SetInt(ClaveTutorial, 1);
        PlayerPrefs.Save();
    }

    private void OnEnable()
    {
        construccion.VentaPermitida = false;
        construccion.PermisoConstruir = PuedeConstruir;
        oleadas.PermisoInicio = PuedeIniciar;
        omitir.onClick.AddListener(Omitir);
        foreach (var boton in otrosBotones) boton.interactable = false;
    }

    private void OnDisable()
    {
        construccion.VentaPermitida = true;
        construccion.PermisoConstruir = null;
        oleadas.PermisoInicio = null;
        omitir.onClick.RemoveListener(Omitir);
    }

    private bool PuedeConstruir(BaseTorre destino, TorreItem item)
    {
        if (saliendo || destino != baseMarcada || oleadas.OleadaEnCurso) return false;
        return pasoActual switch
        {
            Paso.ColocarCanon or Paso.ReponerCanon => baseMarcada.torreEnBase == null && item == canon,
            Paso.Combinar => baseMarcada.Inversion == canon.costo && item == fuego,
            _ => false
        };
    }

    private bool PuedeIniciar() => !saliendo && baseMarcada.torreEnBase != null &&
        (oleadas.OleadaActual == 0 ||
            (ventaPracticada && baseMarcada.Inversion == canon.costo + fuego.costo));

    private Paso DeterminarPaso()
    {
        bool baseVacia = baseMarcada.torreEnBase == null;
        if (oleadas.OleadaActual == 0) return baseVacia ? Paso.ColocarCanon : Paso.IniciarPrimeraOleada;
        if (oleadas.OleadaActual == 2) return Paso.SegundaOleada;
        if (oleadas.OleadaEnCurso) return Paso.PrimeraOleada;
        if (!ventaPracticada) return Paso.VenderCanon;
        if (baseVacia) return Paso.ReponerCanon;
        return baseMarcada.Inversion == canon.costo ? Paso.Combinar : Paso.IniciarSegundaOleada;
    }

    private void Update()
    {
        if (pasoActual == Paso.VenderCanon && baseMarcada.torreEnBase == null)
            ventaPracticada = true;

        Paso paso = DeterminarPaso();
        if (paso == pasoActual) return;
        pasoActual = paso;

        construccion.VentaPermitida = paso == Paso.VenderCanon;
        botonCanon.interactable = paso is Paso.ColocarCanon or Paso.ReponerCanon;
        botonFuego.interactable = paso == Paso.Combinar;
        indicador.enabled = botonCanon.interactable || botonFuego.interactable || construccion.VentaPermitida;
        titulo.text = paso switch
        {
            Paso.VenderCanon or Paso.ReponerCanon => "VENTA DE TORRES",
            Paso.ColocarCanon or Paso.IniciarPrimeraOleada or Paso.PrimeraOleada => "ENTRENAMIENTO / 01",
            _ => "ENTRENAMIENTO / 02"
        };
        instrucciones.text = paso switch
        {
            Paso.ColocarCanon => "Elige el cañón y pulsa la base marcada.\nArriba ves monedas, vida y oleadas.",
            Paso.IniciarPrimeraOleada or Paso.IniciarSegundaOleada => "Pulsa EMPEZAR o Espacio para iniciar.",
            Paso.PrimeraOleada => "La torre dispara sola y cada baja da monedas.\nSi llegan al núcleo, pierdes vida.",
            Paso.VenderCanon => "Clic derecho en la base para vender.\nRecuperas el 60 %, redondeado hacia abajo.",
            Paso.ReponerCanon => "Vuelve a colocar el cañón en la base marcada.",
            Paso.Combinar => "Elige el lanzallamas y pulsa la misma base.\nLas dos torres se combinan.",
            _ => "Resiste la última oleada con la torre combinada."
        };
    }

    private void Omitir()
    {
        if (saliendo) return;
        saliendo = true;
        RecordarTutorial();
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(ResultadoPartida.Menu);
    }
}
