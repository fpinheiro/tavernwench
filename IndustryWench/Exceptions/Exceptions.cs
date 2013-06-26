using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndustryWench.Exceptions {
    public class UnmappedClassException : Exception { }
    
    public class IdMustBeAPropertyOrFieldException : Exception { }

    public class DuplicateActorException : Exception {
        public Type KnownType { get; set; }

        public DuplicateActorException(Type T) : base() { }    
    }
}
