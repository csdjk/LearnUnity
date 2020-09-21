using DesignPattern;
using UnityEngine;
namespace DesignPattern {
    /// <summary>
    /// 工厂类型 
    /// </summary>
    public enum FactoryType {
        Asus, //华硕
        Dell //戴尔
    }

    public class DesignPatternTest : MonoBehaviour {
        private void Start () {
            // 创建一个华硕工厂
            AbstractFactory asusFactory = AbstractFactory.CreateFactory (FactoryType.Asus);
            // 创建一个华硕鼠标
            IMouse mouse1 = asusFactory.CreateMouse ();
            mouse1.Print ();

            // 创建一个戴尔工厂
            AbstractFactory dellFactory = AbstractFactory.CreateFactory (FactoryType.Dell);
            // 创建一个戴尔鼠标
            IMouse mouse2 = dellFactory.CreateMouse ();
            mouse2.Print ();
        }
    }
}