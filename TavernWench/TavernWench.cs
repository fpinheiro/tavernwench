
using TavernWench.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using PetaPoco;
using System.Configuration;

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
        public static Config Config<T>(Action<Config<T>> mapConfiguration, bool shouldMergeWithCurrent = true) {
            var newConfig = new Config<T>(mapConfiguration);

            if (_configurations.ContainsKey(typeof(T)))
                if (shouldMergeWithCurrent) _configurations[typeof(T)].Merge(newConfig);
                else _configurations[typeof(T)] = newConfig;
            else
                _configurations.Add(typeof(T), newConfig);

            return _configurations[typeof(T)];
        }

        /// <summary>
        /// Returns the configuration of a certain type
        /// </summary>
        public static Config Config<T>() {
            Config classConfig;
            if (!_configurations.TryGetValue(typeof(T), out classConfig) || classConfig.KeyInfo == null) {
                //returns the default configuration
                classConfig = TavernWench.Config<T>(m => m.SetKey(x => x.ToString()));
            }
            return classConfig;
        }

        /// <summary>
        /// Records the builder of an object
        /// </summary>
        public static void Remember<T>(Func<T> builder) {
            var newMemory = new Memory<T>(builder);

            //getting configuration to see if we need to persist the object
            var config = TavernWench.Config<T>();
            TavernWench.PersistIfYouMust(config, newMemory.TargetObject);

            Dictionary<object, Memory> memoryDic;
            //do I remember the type T ?
            if (!_memories.TryGetValue(typeof(T), out memoryDic)) {
                _memories.Add(typeof(T), new Dictionary<object, Memory>());
                memoryDic = _memories[typeof(T)];
            }

            object keyValue;
            switch (config.KeyInfo.MemberType) {
                case MemberTypes.Field:
                    keyValue = ((FieldInfo)config.KeyInfo).GetValue(newMemory.TargetObject); break;
                case MemberTypes.Property:
                    keyValue = ((PropertyInfo)config.KeyInfo).GetValue(newMemory.TargetObject, null); break;
                case MemberTypes.Method:
                    var method = (MethodInfo)config.KeyInfo;
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

        /// <summary>
        /// saves the object as it is on the database, ovrewriting possible instances
        /// </summary>
        private static void PersistIfYouMust(Config config, object theObject) {
            if (config.Persist ?? false) {
                using (var db = new Database(ConfigurationManager.AppSettings["wench:connString"] ?? "")) {
                    var pkInfo = config.DatabasePkInfo;
                    var autoIncrement = pkInfo == null || pkInfo.GetValue(theObject, null) == null;
                    db.Insert(config.TableName ?? config.ClassType.Name, 
                              config.DatabasePk ?? "Id", 
                              autoIncrement, 
                              theObject);
                }
            }
        }
    }
}
