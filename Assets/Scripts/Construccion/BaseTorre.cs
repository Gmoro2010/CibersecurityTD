using UnityEngine;
using UnityEngine.InputSystem; // Necesario para el nuevo Input System

[RequireComponent(typeof(Collider2D))]
public class BaseTorre : MonoBehaviour
{
    [Header("Estado de la Base")]
    [Tooltip("Referencia a la torre que está actualmente encima de esta base (nulo si está vacía)")]
    public TorreIdentificador torreEnBase;

    private int inversion;
    private float inhabilitadaHasta;
    private SpriteRenderer indicador;
    private Color colorNormal;
    private bool indicadorInhabilitado;

    public int Inversion => inversion;
    public bool Inhabilitada => Time.time < inhabilitadaHasta;

    private void Awake()
    {
        indicador = GetComponent<SpriteRenderer>();
        if (indicador != null) colorNormal = indicador.color;
    }

    private void LateUpdate()
    {
        if (indicador == null || indicadorInhabilitado == Inhabilitada) return;
        indicadorInhabilitado = Inhabilitada;
        indicador.color = indicadorInhabilitado ? new Color32(255, 85, 153, 255) : colorNormal;
    }

    public void Inhabilitar(float segundos)
    {
        inhabilitadaHasta = Mathf.Max(inhabilitadaHasta, Time.time + segundos);
    }

    public void ConservarBloqueoDe(BaseTorre origen)
    {
        inhabilitadaHasta = Mathf.Max(inhabilitadaHasta, origen.inhabilitadaHasta);
    }

    public void Instalar(GameObject prefab, int costoTotal)
    {
        QuitarTorre();
        GameObject instancia = Instantiate(prefab, transform.position, Quaternion.identity);
        torreEnBase = instancia.GetComponentInChildren<TorreIdentificador>();
        inversion = costoTotal;
        foreach (var torre in instancia.GetComponentsInChildren<Turret>()) torre.Base = this;
        foreach (var torre in instancia.GetComponentsInChildren<TurretEstatica>()) torre.Base = this;
    }

    public void QuitarTorre()
    {
        if (torreEnBase != null)
        {
            GameObject raiz = torreEnBase.transform.root.gameObject;
            raiz.SetActive(false);
            Destroy(raiz);
        }
        torreEnBase = null;
        inversion = 0;
    }

    private void OnMouseOver()
    {
        // Verificamos que el ratón esté disponible en el nuevo Input System
        if (Time.timeScale == 0f || Mouse.current == null) return;

        // Clic Izquierdo (Nuevo Input System): Construir o Combinar
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Si el jugador tiene una torre seleccionada en la tienda:
            if (BuildManager.Instance != null && BuildManager.Instance.TorreParaConstruir != null)
            {
                BuildManager.Instance.ConstruirTorreEnBase(this);
                return;
            }

            // Si no hay nada seleccionado, delegamos al Administrador de Combinaciones
            if (AdministradorCombinaciones.Instance != null)
            {
                AdministradorCombinaciones.Instance.SeleccionarBase(this);
            }
        }

        // Clic Derecho (Nuevo Input System): Vender la torre
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (torreEnBase != null)
            {
                VenderTorre();
            }
            else
            {
                Debug.Log("Esta base está vacía, no hay nada que vender.");
            }
        }
    }

    public void VenderTorre()
    {
        if (torreEnBase == null || BuildManager.Instance == null || !BuildManager.Instance.VentaPermitida)
            return;

        BuildManager.Instance.GanarDinero(inversion * 3 / 5);
        QuitarTorre();
    }
}
