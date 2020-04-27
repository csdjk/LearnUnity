using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------【Trigger Particle】---------------------------
[ExecuteInEditMode]
public class TriggerParticle : MonoBehaviour
{
     ParticleSystem ps;

    // 这些列表用于包含与每帧的触发条件
    // 匹配的粒子。
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

    void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void OnParticleTrigger()
    {
        // 获取与此帧的触发条件匹配的粒子

        // 获取 进入触发器 的粒子
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        // 获取 离开触发器 的粒子
        int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);

        // 迭代进入触发器的粒子并使它们变为红色
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            p.startColor = new Color32(255, 0, 0, 255);
            enter[i] = p;
        }

        // 迭代离开触发器的粒子并使它们变绿
        for (int i = 0; i < numExit; i++)
        {
            ParticleSystem.Particle p = exit[i];
            p.startColor = new Color32(0, 255, 0, 255);
            exit[i] = p;
        }

        // 将修改后的粒子重新分配回粒子系统
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
    }
}
