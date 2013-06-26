using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndustryWench {
    /// <summary>
    /// the key value and the actor pointed  
    /// </summary>
    internal class Memory<T> : Memory  {
        private Func<T> _constructor;
        
        public T Actor { 
            get {
                //if there's no actor's cached instance we build it
                if (_actor == null) _actor = _constructor();
                return (T)_actor;
            } 
        }

        public Memory( Func<T> actorConstructor ) : base() {
            _constructor = actorConstructor;
        }
    }

    internal class Memory {
        protected object _actor;

        protected Memory () { }
    }
}
