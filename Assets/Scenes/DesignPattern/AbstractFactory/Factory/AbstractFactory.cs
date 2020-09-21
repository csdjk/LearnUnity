using System;

namespace DesignPattern {

    /// <summary>
    /// 抽象工厂模式
    /// 如果需要新增 工厂 , 只需要 新增 一个工厂类 继承 AbstractFactory 即可
    /// 如果需要新增 物品(例如 键盘), 首先我们需要增加 键盘 这个接口 和 对应每个工厂的 键盘子类, 
    /// 在PC厂商这个父类中，增加生产 键盘 的接口,分别在工厂里实现该接口
    /// </summary>
    public abstract class AbstractFactory {
        public static AbstractFactory CreateFactory (FactoryType factoryType) {
            // 如果增加新的类别  , 违背了开闭原则
            // AbstractFactory factory = null;
            // switch (factoryType) {
            //     case factoryType.Client:
            //         factory = new ClientFactory ();
            //         break;
            //     case factoryType.Server:
            //         factory = new ServerFactory ();
            //         break;
            // }
            // return factory;

            // 利用反射动态创建
            // 遵循命名规则(必须): DesignPattern. + FactoryType + Factory
            Type type = Type.GetType ($"DesignPattern.{factoryType}Factory");
            return Activator.CreateInstance (type) as AbstractFactory;
        }

        public abstract IMouse CreateMouse ();
    }
}