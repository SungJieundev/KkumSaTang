using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class OutlineHover : MonoBehaviour
{
    public Material outlineMaterial;      // 아웃라인 머티리얼
    private Material originalMaterial;    // 기존 머티리얼
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;

        // 시작 시 아웃라인 제거
        rend.material = originalMaterial;
    }

    void OnMouseEnter()
    {
        if (outlineMaterial != null)
        {
            rend.material = outlineMaterial;
        }
    }

    void OnMouseExit()
    {
        rend.material = originalMaterial;
    }
}