using UnityEngine;

namespace DesignPattern {
    public class DellMouse : IMouse {
        public void Print () { 
            Debug.Log("dell mouse");
        }

    }
}