using UnityEngine;
using UnityEngine.UI;

public class NucleoVida : MonoBehaviour
{
    [SerializeField] private float vidaMaximaNucleo = 200f; 
    [SerializeField] private Slider barraVidaNucleo;       
    private float vidaActualNucleo;

    public float VidaActual => vidaActualNucleo;
    public float VidaMaxima => vidaMaximaNucleo;

    private void Awake()
    {
        vidaActualNucleo = vidaMaximaNucleo;
        if (barraVidaNucleo != null)
        {
            barraVidaNucleo.maxValue = vidaMaximaNucleo;
            barraVidaNucleo.value = vidaMaximaNucleo;
        }
    }

    public void DanarNucleo(float cantidad)
    {
        vidaActualNucleo = Mathf.Clamp(vidaActualNucleo - cantidad, 0f, vidaMaximaNucleo);
        Debug.Log("¡El núcleo Pfinal recibió daño! Vidas restantes: " + vidaActualNucleo);

        if (barraVidaNucleo != null)
        {
            barraVidaNucleo.value = vidaActualNucleo;
        }

        if (vidaActualNucleo <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("¡GAME OVER!");
        Time.timeScale = 0f; // Congela el juego
    }
}
