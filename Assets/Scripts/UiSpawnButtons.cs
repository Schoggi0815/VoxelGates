using UnityEngine;

public class UiSpawnButtons : MonoBehaviour
{
    public void SpawnOutputButton()
    {
        Spawn(Constants.C.outputKnobPrefab, Constants.C.knobSpawnOffset, Constants.C.knobParent);
    }

    public void SpawnInputButton()
    {
        Spawn(Constants.C.inputKnobPrefab, Constants.C.knobSpawnOffset, Constants.C.knobParent);
    }
    
    private void Spawn(GameObject prefab, Vector2 offset, Transform parent)
    {
        var screenToWorldPoint = Camera.main.ScreenToWorldPoint(transform.position);
        var worldPosition = new Vector3(screenToWorldPoint.x + offset.x * Camera.main.orthographicSize, screenToWorldPoint.y + offset.y * Camera.main.orthographicSize, 0);
        var instantiate = Instantiate(prefab, GridObject.GridToWorldPos(GridObject.WorldToGridPos(worldPosition)), new Quaternion());
        instantiate.transform.parent = parent;
    }
}
