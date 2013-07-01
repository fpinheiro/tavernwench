
using PetaPoco;
using System;

namespace TavernWench {
    /// <summary>
    /// the key value and the actor pointed  
    /// </summary>
    internal class Memory<T> : Memory  {
        private Func<T> _constructor;
        
        internal T TargetObject { 
            get {
                return (T)_object;
            } 
        }

        internal Memory( Func<T> actorConstructor) : base() {
            _constructor = actorConstructor;
            _object = _constructor();
        }
    }

    internal class Memory {
        protected object _object;

        protected Memory () { }
    }
}
