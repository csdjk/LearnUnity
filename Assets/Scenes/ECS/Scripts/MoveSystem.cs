using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 组件系统
/// </summary>
class MoveSystem : ComponentSystem {

    protected override void OnUpdate () {
        float deltaTime = Time.DeltaTime;
        // 遍历实体
        Entities.ForEach ((MoveComponent moveCom, ref Translation translation) => {
            translation.Value.z += moveCom.Speed * deltaTime;
            
            if (translation.Value.z > moveCom.Range) {
                moveCom.Speed = -math.abs (moveCom.Speed);
            }
            if (translation.Value.z < -moveCom.Range) {
                moveCom.Speed = +math.abs (moveCom.Speed);
            }
        });
    }
}