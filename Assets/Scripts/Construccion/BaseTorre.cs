using UnityEngine;
using UnityEngine.InputSystem; // Necesario para el nuevo Input System

[RequireComponent(typeof(Collider2D))]
public class BaseTorre : MonoBehaviour
{
    [Header("Estado de la Base")]
    [Tooltip("Referencia a la torre que está actualmente encima de esta base (nulo si está vacía)")]
    public TorreIdentificador torreEnBase;

    [Header("Configuración de Venta")]
    [Tooltip("Cantidad de dinero que se devuelve al vender la torre")]
    [SerializeField] private int valorDeVenta = 100;

    private void OnMouseOver()
    {
        // Verificamos que el ratón esté disponible en el nuevo Input System
        if (Mouse.current == null) return;

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
        if (torreEnBase == null) return;

        // Devolver dinero al jugador a través del BuildManager
        if (BuildManager.Instance != null)
        {
            BuildManager.Instance.GanarDinero(valorDeVenta);
        }

        // Destruir el GameObject raíz de la torre (sea básica o combinada)
        Destroy(torreEnBase.transform.root.gameObject);

        // Liberar la base
        torreEnBase = null;

        Debug.Log("¡Torre vendida con éxito con clic derecho! Base liberada.");
    }
}