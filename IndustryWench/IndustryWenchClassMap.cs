
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
    public class IndustryWenchClassMap {

        protected Type _classType;
        protected MemberInfo _idMemberInfo;

        /// <summary>
        /// should I try to store this actor on the database?
        /// </summary>
        protected bool Persist { get; set; }

        protected IndustryWenchClassMap(Type classType) {
            _classType = classType;
        }

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
    }

    /// <summary>
    /// Typed configuration to store stuff via lambda expressions
    /// </summary>
    public class IndustryWenchClassMap<T> : IndustryWenchClassMap {

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
