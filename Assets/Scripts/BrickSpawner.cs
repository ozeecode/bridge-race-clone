using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{

    [SerializeField] private int gridX = 10;
    [SerializeField] private int gridZ = 10;
    [SerializeField] private float gridSpacingOffset = 2f;

    public List<BrickGridData> brickList = new List<BrickGridData>();

    private WaitForSeconds spawnerDelay = new WaitForSeconds(4f);
    Coroutine spawnerCoroutine;

    public void Init()
    {

        foreach (BrickGridData brickData in brickList)
        {
            if (!brickData.IsCollected)
                brickData.brick.Despawn();
        }
        brickList.Clear(); //TODO: possible too many gc alloc!

        Vector3 startPos = transform.position - new Vector3(gridX * .5f * gridSpacingOffset, 0, gridZ * .5f * gridSpacingOffset);

        for (int x = 0; x <= gridX; x++)
        {
            for (int z = 0; z <= gridZ; z++)
            {
                Vector3 spawnPos = startPos + new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset);
                Brick brick = LeanPool.Spawn(GameAssets.Instance.Brick, spawnPos, Quaternion.identity);
                BrickGridData brickData = new BrickGridData(spawnPos, brick, GameManager.Instance.RandomColor);
                brick.SetBrickData(brickData);
                brickList.Add(brickData);
            }
        }
        if (spawnerCoroutine is not null)
        {
            StopCoroutine(spawnerCoroutine);
        }
        spawnerCoroutine = StartCoroutine(Spawner());


    }


    IEnumerator Spawner()
    {
        while (true)
        {
            for (int i = 0; i < brickList.Count; i++)
            {
                if (brickList[i].IsCollected && Random.value > .5f)
                {
                    Brick brick = LeanPool.Spawn(GameAssets.Instance.Brick, brickList[i].position, Quaternion.identity);
                    brickList[i].IsCollected = false;
                    brickList[i].Color = GameManager.Instance.RandomColor;
                    brickList[i].brick = brick;
                    brick.SetBrickData(brickList[i]);
                }
            }
            yield return spawnerDelay;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 0.5f, 0), new Vector3(gridX * gridSpacingOffset, 1, gridZ * gridSpacingOffset));
    }
#endif

}

public class BrickGridData
{
    public Vector3 position;
    public Brick brick;
    public bool IsCollected;
    public Color Color;

    public BrickGridData(Vector3 position, Brick brick, Color color)
    {
        this.position = position;
        this.brick = brick;
        Color = color;
        IsCollected = false;
    }
}