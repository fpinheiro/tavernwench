using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndustryWench.Exceptions;

namespace IndustryWench {

    /// <summary>
    /// Configure, Declare and Get instances of an object
    /// </summary>
    public static class IndustryWench {

        private static Dictionary<Type, IndustryWenchClassMap> _configurations;

        static IndustryWench() { Clear(); }

        /// <summary>
        /// Resgisters the configuration of a certain POCO
        /// Which attribute is the id?
        /// Should we persist th object on the database?
        /// </summary>
        public static IndustryWenchClassMap Config<T>(Action<IndustryWenchClassMap<T>> mapConfiguration) {
            var classMap = new IndustryWenchClassMap<T>(mapConfiguration);
            
            if(_configurations.ContainsKey(typeof(T))) 
                _configurations[typeof(T)] = classMap;
            else 
                _configurations.Add(typeof(T), classMap);
            
            return classMap;
        }

        /// <summary>
        /// Returns the configuration of a certain type
        /// </summary>
        public static IndustryWenchClassMap Config<T>(){
            IndustryWenchClassMap classMap;
            if (!_configurations.TryGetValue(typeof(T), out classMap)) throw new UnmappedClassException();
            return classMap;
        }

        public static void Clear() {
            _configurations = new Dictionary<Type, IndustryWenchClassMap>();
        }
    }
}
