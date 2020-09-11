using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 组件系统
/// </summary>
class RotationSystem : ComponentSystem {

    protected override void OnUpdate () {
        float deltaTime = Time.DeltaTime;
        // 遍历实体
        Entities.ForEach ((RotationComponent rotationCom, ref Rotation rotation) => {
            rotation.Value = math.mul (math.normalize (rotation.Value), quaternion.AxisAngle (math.up (), rotationCom.Speed * deltaTime));
        });
    }
}