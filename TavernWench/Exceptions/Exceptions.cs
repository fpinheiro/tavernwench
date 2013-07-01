using System;

namespace TavernWench.Exceptions {
    /// <summary>
    /// you must configure the class before using it
    /// </summary>
    public class NoConfigurationFoundForThisClassException : Exception { }

    /// <summary>
    /// you cannot map a methodwith parameters as a Key
    /// </summary>
    public class CantUseMethodWithParametersAsKeyException : Exception { }

    /// <summary>
    /// no object of the type was declared using TavernWench.Remember
    /// </summary>
    public class NeverHeardAboutThisClassException : Exception { }

    /// <summary>
    /// no object of the with the given key was declared using TavernWench.Remember
    /// </summary>
    public class NeverHeardAboutThisKeyException : Exception { }

    /// <summary>
    /// dafuq you're doing man?
    /// </summary>
    public class KeyIsUnsupportedMemberType : Exception { }

    /// <summary>
    /// 
    /// </summary>
    public class DatabasePKMustBeProperty : Exception { }
    
}
