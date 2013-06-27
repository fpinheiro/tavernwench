
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

        protected Type _classType;
        
        protected MemberInfo _keyMemberInfo;

        /// <summary>
        /// le id's MemberInfo
        /// </summary>
        public MemberInfo KeyMemberInfo {
            get {
                return _keyMemberInfo;
            }
        }

        /// <summary>
        /// le id attribute's name
        /// </summary>
        public string Key {
            get {
                return _keyMemberInfo != null ? _keyMemberInfo.Name : null;
            }

        }
        
        /// <summary>
        /// should I try to store this actor on a database?
        /// </summary>
        public bool Persist { get; set; }

        /// <summary>
        /// protected constructor cause people should not be able to instance this class directly
        /// it must be created through the TavernWenchClassMap<T>
        /// </summary>
        protected Config(Type classType) {
            _classType = classType;
        }
    }

    /// <summary>
    /// Typed configuration to store stuff via lambda expressions
    /// </summary>
    public class TavernWenchClassMap<T> : Config {

        public TavernWenchClassMap(Action<TavernWenchClassMap<T>> mapConfiguration) : base(typeof(T)) {
            mapConfiguration(this);
        }

        /// <summary>
        /// saves le id member from lambda
        /// </summary>
        public void SetKey<TMember>(Expression<Func<T, TMember>> memberLambda) {
            var body = memberLambda.Body;

            switch(body.NodeType) {
                case ExpressionType.MemberAccess:
                    _keyMemberInfo = ((MemberExpression)body).Member; break;
                case ExpressionType.Call:
                    var parametersCount = ((MethodCallExpression)body).Arguments.Count;
                    if (parametersCount > 0) throw new CantUseMethodWithParametersAsKeyException();
                    _keyMemberInfo = ((MethodCallExpression)body).Method; break;
                default:
                    throw new KeyIsUnsupportedMemberType();
            }
        }
    }
}
