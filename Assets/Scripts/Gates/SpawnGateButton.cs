using UnityEngine;

namespace Gates
{
	public class SpawnGateButton : MonoBehaviour
	{
		public void SpawnAndGate()
		{
			Spawn(Constants.C.andGatePrefab, Constants.C.gateSpawnOffset, Constants.C.gateParent);
		}
		
		public void SpawnNotGate()
		{
			Spawn(Constants.C.notGatePrefab, Constants.C.gateSpawnOffset, Constants.C.gateParent);
		}

		private void Spawn(GameObject prefab, Vector2 offset, Transform parent)
		{
			var screenToWorldPoint = Camera.main.ScreenToWorldPoint(transform.position);
			var worldPosition = new Vector3(screenToWorldPoint.x + offset.x * Camera.main.orthographicSize, screenToWorldPoint.y + offset.y * Camera.main.orthographicSize, 0);
			var instantiate = Instantiate(prefab, GridObject.GridToWorldPos(GridObject.WorldToGridPos(worldPosition)), new Quaternion());
			instantiate.transform.parent = parent;
		}
	}
}