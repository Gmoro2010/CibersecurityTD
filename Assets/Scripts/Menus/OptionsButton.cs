using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsButton : MonoBehaviour
{
    [Header("Paneles")]
    [Tooltip("Panel del menú principal (por ejemplo, 'Panel')")]
    public GameObject mainMenuPanel;

    [Tooltip("Panel de opciones que se mostrará")]
    public GameObject optionsPanel;

    // Muestra el panel de opciones y oculta el menú principal
    public void OpenOptions()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    // Vuelve al menú principal (para un botón "Volver" dentro de opciones)
    public void CloseOptions()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        EventSystem.current?.SetSelectedGameObject(gameObject);
    }
}
