using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
public class SpawnerManager : MonoBehaviour {

    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;
    public GameObject Prefab;
    public float Count = 100;
    public float rangePos = 20;

    void Start () {

        /* 创建实体时需要指定配置，这里涉及到World的概念，可以先不管，照抄就是了 */
        var settings = GameObjectConversionSettings.FromWorld (World.DefaultGameObjectInjectionWorld, null);

        /* 从我们的prefab中创建一个实体对象 */
        var entityFromPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy (Prefab, settings);

        /* 实体管理器 */
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype entityArchetype = entityManager.CreateArchetype (
            typeof (RenderBounds),
            typeof (Translation),
            typeof (RenderMesh),
            typeof (LocalToWorld),
            typeof (MoveComponent)
        );

        // NativeArray<Entity> entityArray = new NativeArray<Entity> (10, Allocator.Temp);
        // entityManager.CreateEntity (entityArchetype, entityArray);

        /* 循环创建多个实体 */
        for (var i = 0; i < Count; i++) {
            /* 赋值新的实体 */
            var entity = entityManager.Instantiate (entityFromPrefab);
            // var entity = entityArray[i];
            // 设置原型
            entityManager.SetArchetype (entity, entityArchetype);
            /* 修改实体的组件 */
            var position = transform.TransformPoint (UnityEngine.Random.Range (-rangePos, rangePos), 0, UnityEngine.Random.Range (-rangePos, rangePos));
            // entityManager.SetComponentData (entity, new RotationComponent { Speed = UnityEngine.Random.Range (-10, 10) });
            entityManager.SetComponentData (entity, new MoveComponent { Speed = UnityEngine.Random.Range (-10, 10), Range = rangePos});
            entityManager.SetComponentData (entity, new Translation { Value = position });
            // RenderMesh renderMesh = new RenderMesh ();
            // renderMesh.material = material;
            // renderMesh.mesh = mesh;
            // entityManager.SetSharedComponentData (entity, renderMesh);
        }
    }
}