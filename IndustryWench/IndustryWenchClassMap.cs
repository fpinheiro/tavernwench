
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace IndustryWench {

    public class IndustryWenchClassMap {

        protected Type _classType;
        protected MemberInfo _idMemberInfo;

        protected IndustryWenchClassMap(Type classType) {
            _classType = classType;
        }

        public MemberInfo IdMemberInfo {
            get {
                return _idMemberInfo;
            }
        }

        public string Id {
            get {
                return _idMemberInfo != null ? _idMemberInfo.Name : null;
            }

        }
    }

    public class IndustryWenchClassMap<T> : IndustryWenchClassMap {

        public IndustryWenchClassMap(Action<IndustryWenchClassMap<T>> mapConfiguration)
            : base(typeof(T)) {
            mapConfiguration(this);
        }

        public void SetId<TMember>(Expression<Func<T, TMember>> memberLambda) {
            var body = memberLambda.Body;

            if (body.NodeType != ExpressionType.MemberAccess)
                throw new Exception("Lambda expression must be a Member Access");

            _idMemberInfo = ((MemberExpression)body).Member;
        }
    }
}
