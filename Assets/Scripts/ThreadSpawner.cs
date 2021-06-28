using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class ThreadSpawner : MonoBehaviour
{
    [SerializeField] private int instances;
    [SerializeField] private GameObject spawnerPrefab;
    [SerializeField] private GameObject boxPrefab;

    private Entity entityPrefab;
    private World defaultWorld;
    private EntityManager entityManager;
    private BlobAssetStore blobAssetStore;

    private void Awake()
    {
        defaultWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = defaultWorld.EntityManager;
        blobAssetStore = new BlobAssetStore();
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, blobAssetStore);
        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(spawnerPrefab, settings);
        entityManager.Instantiate(entityPrefab);

        float boxBottomPosition = 0;
        float boxHeight = 0;
        GameObject boxTemp = Instantiate(boxPrefab);
        int k = 0;
        foreach (Transform wallGameObject in boxTemp.transform)
        {
            if (k == 2)
            {
                boxBottomPosition = wallGameObject.transform.position.y;
            }

            if (k == 3)
            {
                boxHeight = wallGameObject.transform.position.y - boxBottomPosition;
                boxHeight = boxHeight * 1.2f;
            }

            k++;
        }

        Destroy(boxTemp);

        for (int i = 0; i < instances; i++)
        {
            Entity spawnerEntity = entityManager.Instantiate(entityPrefab);
            //Debug.Log(boxHeight * -i);
            entityManager.SetComponentData(spawnerEntity, new Translation
            { 
                Value = new float3(0, boxHeight * -i, 0)
            });
        }
    }

    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }
}
