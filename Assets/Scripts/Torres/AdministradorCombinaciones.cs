using UnityEngine;

public class AdministradorCombinaciones : MonoBehaviour
{
    public static AdministradorCombinaciones Instance;

    [Header("Configuración")]
    [SerializeField] private BaseDeDatosCombinaciones baseDeDatos;

    private BaseTorre baseSeleccionada;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SeleccionarBase(BaseTorre baseTorre)
    {
        // Si la base está vacía, no se puede usar para combinar
        if (baseTorre.torreEnBase == null)
        {
            Debug.Log("Esta base está vacía.");
            return;
        }

        if (baseSeleccionada == null)
        {
            // Seleccionar la primera base
            baseSeleccionada = baseTorre;
            Debug.Log($"Base seleccionada con torre ID: {baseSeleccionada.torreEnBase.idTorre}");
        }
        else
        {
            // Si hace clic en la misma base, se deselecciona
            if (baseSeleccionada == baseTorre)
            {
                baseSeleccionada = null;
                Debug.Log("Base deseleccionada.");
                return;
            }

            // Segunda base seleccionada: Intentar fusionar las torres
            IntentarCombinar(baseSeleccionada, baseTorre);
        }
    }

    private void IntentarCombinar(BaseTorre base1, BaseTorre base2)
    {
        if (baseDeDatos == null)
        {
            Debug.LogError("Falta asignar la Base de Datos de Combinaciones en el Administrador.");
            baseSeleccionada = null;
            return;
        }

        TorreIdentificador torre1 = base1.torreEnBase;
        TorreIdentificador torre2 = base2.torreEnBase;

        if (torre1 == null || torre2 == null)
        {
            baseSeleccionada = null;
            return;
        }

        // Buscar receta en la base de datos
        GameObject prefabResultado = baseDeDatos.BuscarCombinacion(torre1.idTorre, torre2.idTorre);

        if (prefabResultado != null && prefabResultado.GetComponentInChildren<TorreIdentificador>() != null)
        {
            int inversion = base1.Inversion + base2.Inversion;
            base2.ConservarBloqueoDe(base1);
            base1.QuitarTorre();
            base2.Instalar(prefabResultado, inversion);
        }
        else
        {
            Debug.Log($"No existe una combinación válida entre '{torre1.idTorre}' y '{torre2.idTorre}'.");
        }

        // Limpiar selección
        baseSeleccionada = null;
    }
}
