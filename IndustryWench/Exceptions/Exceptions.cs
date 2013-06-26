using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndustryWench.Exceptions {
    /// <summary>
    /// you must configure the class before using it
    /// </summary>
    public class UnmappedClassException : Exception { }

    /// <summary>
    /// you cannot map a method as a Key
    /// </summary>
    public class KeyMustBeAPropertyOrFieldException : Exception { }

    /// <summary>
    /// no object of the type was declared using IndustryWench.Remember
    /// </summary>
    public class NeverHeardAboutThisClassException : Exception { }

    /// <summary>
    /// no object of the with the given key was declared using IndustryWench.Remember
    /// </summary>
    public class NeverHeardAboutThisKeyException : Exception { }

    
}
