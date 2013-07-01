
using System;
using System.Linq.Expressions;
using System.Reflection;
using TavernWench.Exceptions;

namespace TavernWench {

    /// <summary>
    /// Stores the configuration of a class 
    /// * which one is the id column?
    /// * should I persist this class's actors?
    /// </summary>
    public class Config {

        public Type ClassType { get; private set; }

        /// <summary>
        /// protected constructor cause people should not be able to instance this class directly
        /// it must be created through the TavernWenchClassMap<T>
        /// </summary>
        protected Config(Type classType) {
            ClassType = classType;
        }

        #region Wench Configuration
        
        protected MemberInfo _wenchKey;

        /// <summary>
        /// le id's MemberInfo
        /// </summary>
        public MemberInfo KeyInfo {
            get {
                return _wenchKey;
            }
        }

        /// <summary>
        /// le id attribute's name
        /// </summary>
        public string Key {
            get {
                return _wenchKey != null ? _wenchKey.Name : null;
            }

        }

        #endregion

        #region Persistence

        /// <summary>
        /// should I try to store this actor on a database?
        /// </summary>
        public bool? Persist { get; set; }

        public string TableName { get; set; }

        protected PropertyInfo _databasePkInfo;

        public PropertyInfo DatabasePkInfo {
            get {
                return _databasePkInfo;
            }
        }

        /// <summary>
        /// le id attribute's name
        /// </summary>
        public string DatabasePk {
            get {
                return _databasePkInfo != null ? _databasePkInfo.Name : null;
            }
        }

        #endregion        
    
        /// <summary>
        /// Merges this configuration with the attributes defined in new one
        /// </summary>
        internal void Merge(Config newConfig) {
            this._wenchKey = newConfig.KeyInfo ?? this._wenchKey;
            this._databasePkInfo = newConfig.DatabasePkInfo ?? this._databasePkInfo;
            this.Persist = newConfig.Persist ?? this.Persist;
            this.TableName = newConfig.TableName ?? this.TableName;
        }
    }

    /// <summary>
    /// Typed configuration to store stuff via lambda expressions
    /// </summary>
    public class Config<T> : Config {

        public Config(Action<Config<T>> mapConfiguration) : base(typeof(T)) {
            mapConfiguration(this);
        }

        /// <summary>
        /// saves le id member from lambda
        /// </summary>
        public void SetKey<TMember>(Expression<Func<T, TMember>> memberLambda) {
            var body = memberLambda.Body;

            switch(body.NodeType) {
                case ExpressionType.MemberAccess:
                    _wenchKey = ((MemberExpression)body).Member; break;
                case ExpressionType.Call:
                    var parametersCount = ((MethodCallExpression)body).Arguments.Count;
                    if (parametersCount > 0) throw new CantUseMethodWithParametersAsKeyException();
                    _wenchKey = ((MethodCallExpression)body).Method; break;
                default:
                    throw new KeyIsUnsupportedMemberType();
            }
        }

        /// <summary>
        /// tell me which attritube is your db pk?
        /// </summary>
        public void SetDatabasePk<TMember>(Expression<Func<T, TMember>> memberLambda) {
            var body = memberLambda.Body;
            
            if (body.NodeType != ExpressionType.MemberAccess
                || ((MemberExpression)body).Member.MemberType != MemberTypes.Property)
                throw new DatabasePKMustBeProperty();
         
            _databasePkInfo = (PropertyInfo)((MemberExpression)body).Member;
        }
    }
}
