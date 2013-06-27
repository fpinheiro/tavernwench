
using TavernWench.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace TavernWench {

    /// <summary>
    /// Configure, Declare and Get instances of an object
    /// </summary>
    public static class TavernWench {

        private static Dictionary<Type, Config> _configurations;
        private static Dictionary<Type, Dictionary<object, Memory>> _memories;

        //clear dictionary
        static TavernWench() { Forget(); }

        /// <summary>
        /// Resgisters the configuration of a certain POCO
        /// Which attribute is the id?
        /// Should we persist th object on the database?
        /// </summary>
        public static Config Config<T>(Action<TavernWenchClassMap<T>> mapConfiguration) {
            var classMap = new TavernWenchClassMap<T>(mapConfiguration);

            if (_configurations.ContainsKey(typeof(T)))
                _configurations[typeof(T)] = classMap;
            else
                _configurations.Add(typeof(T), classMap);

            return classMap;
        }

        /// <summary>
        /// Returns the configuration of a certain type
        /// </summary>
        public static Config Config<T>() {
            Config classConfig;
            if (!_configurations.TryGetValue(typeof(T), out classConfig)) throw new NoConfigurationFoundForThisClassException();
            return classConfig;
        }

        /// <summary>
        /// Records the builder of an object
        /// </summary>
        public static void Remember<T>(Func<T> builder) {
            var newMemory = new Memory<T>(builder);

            Dictionary<object, Memory> memoryDic;
            //do I remember the type T ?
            if (!_memories.TryGetValue(typeof(T), out memoryDic)) {
                _memories.Add(typeof(T), new Dictionary<object, Memory>());
                memoryDic = _memories[typeof(T)];
            }

            //getting the type of the key configured to T
            MemberInfo keyInfo;
            try {
                keyInfo = Config<T>().KeyMemberInfo;
            } catch (NoConfigurationFoundForThisClassException) {
                keyInfo = TavernWench.Config<T>(m => m.SetKey(x => x.ToString())).KeyMemberInfo;
            }

            object keyValue;
            switch (keyInfo.MemberType) {
                case MemberTypes.Field:
                    keyValue = ((FieldInfo)keyInfo).GetValue(newMemory.TargetObject); break;
                case MemberTypes.Property:
                    keyValue = ((PropertyInfo)keyInfo).GetValue(newMemory.TargetObject, null); break;
                case MemberTypes.Method:
                    var method = (MethodInfo)keyInfo;
                    if (method.GetParameters().Length > 0) throw new CantUseMethodWithParametersAsKeyException();
                    keyValue = method.Invoke(newMemory.TargetObject, null);
                    break;
                default:
                    throw new KeyIsUnsupportedMemberType();
            }


            //overwriting the memory defined by this builder
            if (memoryDic.ContainsKey(keyValue))
                memoryDic[keyValue] = newMemory;
            else
                memoryDic.Add(keyValue, newMemory);
        }

        /// <summary>
        /// Returns the instance saved in Remember  
        /// </summary>
        public static T Gimme<T>(object keyValue) {
            Dictionary<object, Memory> classMemory;
            Memory targetMemory;

            if (!_memories.TryGetValue(typeof(T), out classMemory)) throw new NeverHeardAboutThisClassException();
            if (!classMemory.TryGetValue(keyValue, out targetMemory)) throw new NeverHeardAboutThisKeyException();

            return ((Memory<T>)targetMemory).TargetObject;
        }

        /// <summary>
        /// Forget everything
        /// </summary>
        public static void Forget() {
            _configurations = new Dictionary<Type, Config>();
            _memories = new Dictionary<Type, Dictionary<object, Memory>>();
        }
    }
}
