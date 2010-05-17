using System;
using System.Reflection;
using System.Collections.Generic;

namespace FlightsNorway.Phone
{
    public class DuplicateRegistrationException : Exception
    {
        public DuplicateRegistrationException() { }
        public DuplicateRegistrationException(string message) : base(message) { }
        public DuplicateRegistrationException(string message, Exception inner) : base(message, inner) { }
    }

    public class MicroContainer
    {
        private readonly Dictionary<Type, Type> _typeRegistrations;
        private readonly Dictionary<Type, object> _instanceRegistrations;

        public MicroContainer()
        {
            _typeRegistrations = new Dictionary<Type, Type>();
            _instanceRegistrations = new Dictionary<Type, object>();
        }

        public void Register<TInterface, TClass>()
            where TInterface : class
            where TClass : TInterface
        {
            if (DoesRegistrationExist<TInterface>())
                throw new DuplicateRegistrationException("Can only contain one registration per type");

            _typeRegistrations.Add(typeof(TInterface), typeof(TClass));
        }

        public TInterface Resolve<TInterface>() where TInterface : class
        {
            return Resolve(typeof(TInterface)) as TInterface;
        }

        private object Resolve(Type type)
        {   
            if(!_typeRegistrations.ContainsKey(type) && 
               !_instanceRegistrations.ContainsKey(type))
                throw new NotSupportedException("Cannot find registration for type " + type.FullName + ".");

            if(_instanceRegistrations.ContainsKey(type))
                return _instanceRegistrations[type];
     
            var createdType = _typeRegistrations[type];

            ConstructorInfo[] constructors = createdType.GetConstructors();
            ConstructorInfo mostSpecificConstructor = null;

            foreach (var c in constructors)
            {
                if (mostSpecificConstructor == null || mostSpecificConstructor.GetParameters().Length < c.GetParameters().Length)
                {
                    mostSpecificConstructor = c;
                }
            }

            var constructorParameters = new List<object>();
            foreach (var parameter in mostSpecificConstructor.GetParameters())
            {
                constructorParameters.Add(Resolve(parameter.ParameterType));
            }

            var instance = Activator.CreateInstance(createdType, constructorParameters.ToArray());
            _instanceRegistrations.Add(type, instance);

            return instance;
        }

        private bool DoesRegistrationExist<T>()
        {
            return _instanceRegistrations.ContainsKey(typeof(T)) || _typeRegistrations.ContainsKey(typeof(T));
        }

        public void RegisterInstance<TInterface>(TInterface instance)
        {
            if (DoesRegistrationExist<TInterface>())
            {
                throw new DuplicateRegistrationException("Can only contain one registration per type");
            }
            _instanceRegistrations.Add(typeof(TInterface), instance);
        }
    }
}