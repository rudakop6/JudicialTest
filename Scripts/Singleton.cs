using System;
using System.Reflection;

namespace JudicialTest
{
    public class Singleton<T> : BaseViewModel where T : class
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = CreateInstance();

                return _instance;
            }
        }
        private static T CreateInstance()
        {
            ConstructorInfo cInfo = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                                                             null, new Type[0], new ParameterModifier[0]);
            return (T)cInfo.Invoke(null);
        }
    }
}
