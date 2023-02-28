using DG.Tweening;
using Lean.Pool;
using UnityEngine;

public class Brick : MonoBehaviour
{


    private const float JUMP_DURATION = .5f;
    private const float JUMP_POWER = .001f;

    private MeshRenderer meshRenderer;
    private Collider col;
    public bool IsTaken => brickData is null || brickData.IsCollected;

    public Color Color;

    private BrickGridData brickData;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
    }

    public void SetBrickData(BrickGridData data)
    {
        brickData = data;
        Color = data.Color;
        meshRenderer.material.SetColor(GameStatic.BASE_COLOR, data.Color);
        col.enabled = true;
    }
    public void Take(Character character)
    {
        col.enabled = false;
        transform.SetParent(character.StackPoint);
        Vector3 localPos = (character.StackPoint.childCount - 1) * (GameStatic.BRICK_HEIGHT * transform.localScale.y); //Quick fix for Kenny's model scale issue.
        transform.DOLocalJump(localPos, JUMP_POWER, 1, JUMP_DURATION);
        transform.DOLocalRotate(Vector3.zero, JUMP_DURATION);
        brickData.IsCollected = true;
        brickData.brick = null;
        brickData = null;
    }
    public void Despawn()
    {
        LeanPool.Despawn(this);
    }
}
