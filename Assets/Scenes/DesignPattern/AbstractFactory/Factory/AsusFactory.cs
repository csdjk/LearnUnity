namespace DesignPattern {
    public class AsusFactory : AbstractFactory {
        public override IMouse CreateMouse () {
            return new AsusMouse ();
        }
    }
}