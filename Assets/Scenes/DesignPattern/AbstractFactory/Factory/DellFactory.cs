namespace DesignPattern {
    public class DellFactory : AbstractFactory {
        public override IMouse CreateMouse () {
            return new DellMouse ();
        }
    }
}