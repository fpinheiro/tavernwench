
using System;

namespace TavernWench {
    /// <summary>
    /// the key value and the actor pointed  
    /// </summary>
    internal class Memory<T> : Memory  {
        private Func<T> _constructor;
        
        internal T TargetObject { 
            get {
                //if there's no actor's cached instance we build it
                if (_object == null) _object = _constructor();
                return (T)_object;
            } 
        }

        internal Memory( Func<T> actorConstructor ) : base() {
            _constructor = actorConstructor;
        }
    }

    internal class Memory {
        protected object _object;

        protected Memory () { }
    }
}
