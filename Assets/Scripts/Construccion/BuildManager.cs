using UnityEngine;
using TMPro;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    [Header("Economía")]
    [SerializeField] private int dineroInicial = 50;
    private int dineroActual;
    [SerializeField] private TextMeshProUGUI textoDineroUI;
    public int DineroActual => dineroActual;

    [Header("Referencias de Combinación")]
    [SerializeField] private BaseDeDatosCombinaciones baseDeDatosCombinaciones;

    private TorreItem torreParaConstruir;
    public TorreItem TorreParaConstruir => torreParaConstruir;
    public System.Func<BaseTorre, TorreItem, bool> PermisoConstruir { get; set; }
    public bool VentaPermitida { get; set; } = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        dineroActual = dineroInicial;
        ActualizarUIDinero();
    }

    public void SeleccionarTorreParaConstruir(TorreItem torre)
    {
        torreParaConstruir = torre;
    }

    public bool PuedeConstruir => torreParaConstruir != null && dineroActual >= torreParaConstruir.costo;

    public bool ConstruirTorreEnBase(BaseTorre baseDestino)
    {
        if (Time.timeScale == 0f || !PuedeConstruir || baseDestino == null ||
            PermisoConstruir?.Invoke(baseDestino, torreParaConstruir) == false) return false;

        GameObject prefab = torreParaConstruir.prefabTorre;
        TorreIdentificador identificador = prefab != null ? prefab.GetComponentInChildren<TorreIdentificador>() : null;
        if (identificador == null) return false;

        if (baseDestino.torreEnBase != null)
        {
            if (baseDeDatosCombinaciones == null) return false;
            prefab = baseDeDatosCombinaciones.BuscarCombinacion(identificador.idTorre, baseDestino.torreEnBase.idTorre);
            if (prefab == null || prefab.GetComponentInChildren<TorreIdentificador>() == null) return false;
        }

        int costo = torreParaConstruir.costo;
        baseDestino.Instalar(prefab, baseDestino.Inversion + costo);
        dineroActual -= costo;
        torreParaConstruir = null;
        ActualizarUIDinero();
        return true;
    }

    public GameObject ConstruirTorreEn(Vector3 posicion)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(posicion);
        foreach (var hit in hits)
        {
            BaseTorre baseEncontrada = hit.GetComponentInParent<BaseTorre>();
            if (baseEncontrada != null)
            {
                ConstruirTorreEnBase(baseEncontrada);
                return baseEncontrada.torreEnBase != null ? baseEncontrada.torreEnBase.gameObject : null;
            }
        }
        return null;
    }

    public void GanarDinero(int cantidad)
    {
        dineroActual += cantidad;
        ActualizarUIDinero();
    }

    private void ActualizarUIDinero()
    {
        if (textoDineroUI != null)
        {
            textoDineroUI.text = $"{dineroActual}B";
        }
    }
}
