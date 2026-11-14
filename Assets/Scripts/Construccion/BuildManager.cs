using UnityEngine;
using TMPro;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    [Header("Economía")]
    [SerializeField] private int dineroInicial = 200;
    private int dineroActual;
    [SerializeField] private TextMeshProUGUI textoDineroUI;

    [Header("Referencias de Combinación")]
    [SerializeField] private BaseDeDatosCombinaciones baseDeDatosCombinaciones;

    private TorreItem torreParaConstruir;
    public TorreItem TorreParaConstruir => torreParaConstruir;

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
        if (!PuedeConstruir || baseDestino == null) return false;

        // CASO 1: Base vacía -> Construir y vincular inmediatamente
        if (baseDestino.torreEnBase == null)
        {
            dineroActual -= torreParaConstruir.costo;
            ActualizarUIDinero();

            GameObject nuevaTorre = Instantiate(torreParaConstruir.prefabTorre, baseDestino.transform.position, Quaternion.identity);
            
            // Buscamos el identificador en la raíz o en sus hijos
            TorreIdentificador identificador = nuevaTorre.GetComponentInChildren<TorreIdentificador>();
            
            if (identificador != null)
            {
                baseDestino.torreEnBase = identificador;
            }
            else
            {
                Debug.LogError($"¡ERROR! El prefab '{torreParaConstruir.prefabTorre.name}' NO tiene el componente 'TorreIdentificador'.");
            }

            torreParaConstruir = null;
            return true;
        }
        // CASO 2: Base ocupada -> Intentar combinar
        else
        {
            if (baseDeDatosCombinaciones == null)
            {
                Debug.LogError("Falta asignar la Base de Datos de Combinaciones en el BuildManager.");
                return false;
            }

            TorreIdentificador idTorreTienda = torreParaConstruir.prefabTorre.GetComponentInChildren<TorreIdentificador>();
            TorreIdentificador idTorreExistente = baseDestino.torreEnBase;

            if (idTorreTienda == null || idTorreExistente == null)
            {
                Debug.LogError("Falta el componente 'TorreIdentificador' en una de las torres.");
                return false;
            }

            GameObject prefabResultado = baseDeDatosCombinaciones.BuscarCombinacion(idTorreTienda.idTorre, idTorreExistente.idTorre);

            if (prefabResultado != null)
            {
                dineroActual -= torreParaConstruir.costo;
                ActualizarUIDinero();

                // Destruimos por completo la torre anterior (incluyendo su raíz)
                Destroy(idTorreExistente.transform.root.gameObject);

                // Instanciamos la nueva torre combinada
                GameObject nuevaTorreGO = Instantiate(prefabResultado, baseDestino.transform.position, Quaternion.identity);
                
                // Vinculamos el nuevo identificador a la base
                baseDestino.torreEnBase = nuevaTorreGO.GetComponentInChildren<TorreIdentificador>();

                torreParaConstruir = null;
                Debug.Log($"¡Combinación exitosa! Se creó {nuevaTorreGO.name} y la base se actualizó correctamente.");
                return true;
            }
            else
            {
                Debug.Log($"No existe combinación entre '{idTorreTienda.idTorre}' y '{idTorreExistente.idTorre}'.");
                return false;
            }
        }
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