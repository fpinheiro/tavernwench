
using System;
using System.Linq.Expressions;
using System.Reflection;
using IndustryWench.Exceptions;

namespace IndustryWench {

    /// <summary>
    /// Stores the configuration of a class 
    /// * which one is the id column?
    /// * should I persist this class's actors?
    /// </summary>
    public class Config {

        protected Type _classType;
        
        protected MemberInfo _idMemberInfo;

        /// <summary>
        /// le id's MemberInfo
        /// </summary>
        public MemberInfo IdMemberInfo {
            get {
                return _idMemberInfo;
            }
        }

        /// <summary>
        /// le id attribute's name
        /// </summary>
        public string Id {
            get {
                return _idMemberInfo != null ? _idMemberInfo.Name : null;
            }

        }
        
        /// <summary>
        /// should I try to store this actor on a database?
        /// </summary>
        public bool Persist { get; set; }

        /// <summary>
        /// protected constructor cause people should not be able to instance this class directly
        /// it must be created through the IndustryWenchClassMap<T>
        /// </summary>
        protected Config(Type classType) {
            _classType = classType;
        }
    }

    /// <summary>
    /// Typed configuration to store stuff via lambda expressions
    /// </summary>
    public class IndustryWenchClassMap<T> : Config {

        public IndustryWenchClassMap(Action<IndustryWenchClassMap<T>> mapConfiguration) : base(typeof(T)) {
            mapConfiguration(this);
        }

        /// <summary>
        /// saves le id member from lambda
        /// </summary>
        public void SetId<TMember>(Expression<Func<T, TMember>> memberLambda) {
            var body = memberLambda.Body;

            if (body.NodeType != ExpressionType.MemberAccess) throw new IdMustBeAPropertyOrFieldException();

            _idMemberInfo = ((MemberExpression)body).Member;
        }
    }
}
