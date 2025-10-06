using UnityEngine;

public class ColorScript : MonoBehaviour
{
    private Renderer[] enemyRenderers;
    private Color originalColor = Color.cyan;
    private Color damagedColor = Color.white;
    
    void Start()
    {
        enemyRenderers = GetComponentsInChildren<Renderer>();
        
        SetAllRenderersColor(originalColor);
    }
    
    public void ChangeToDamagedColor()
    {
        SetAllRenderersColor(damagedColor);
    }
    
    public void ResetColor()
    {
        SetAllRenderersColor(originalColor);
    }
    
    private void SetAllRenderersColor(Color color)
    {
        if (enemyRenderers != null)
        {
            foreach (Renderer renderer in enemyRenderers)
            {
                if (renderer != null)
                {
                    renderer.material.color = color;
                }
            }
        }
    }
}
