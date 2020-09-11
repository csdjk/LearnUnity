using Unity.Entities;
using UnityEngine;
/// <summary>
/// 实体管理器
/// </summary>
public class EntitiesManager : MonoBehaviour, IConvertGameObjectToEntity {

    public float Speed = 10f;


     void Start ()
    {

    }

    /// <summary>
    /// 把GameObject转为 实体(Entity)
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="dstManager"></param>
    /// <param name="conversionSystem"></param>
    public void Convert (Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        /// 创建一个组件
        RotationComponent data = new RotationComponent { Speed = Speed };
        // 把  组件 加入EntityManager 中,让Untiy 内置的环境实体管理器 管理
        dstManager.AddComponentData (entity, data);
    }
}
