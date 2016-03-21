using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetDiskHelper
{
    static public class AssertHelper
    {
        static public void Against<T>(T val, Func<T, bool> match, Type type)
        {
            if (match(val))
            {
                throw new ArgumentException(string.Format("value {0} is not allowed by type {1} ", val, type.ToString()));
            }
        }

        static public void Against<T>(T val, Func<T, bool> match, MethodInfo method)
        {
            if (match(val))
            {
                throw new ArgumentException(string.Format("value {0} is not allowed by type {1} method {2} ", val, method.DeclaringType.ToString(), method.Name));
            }
        }

        static public void Against<T>(T val, Func<T, bool> match, string info)
        {
            if (match(val))
            {
                throw new ArgumentException(string.Format("value {0} is not allowed{1} ", val, string.IsNullOrWhiteSpace(info) ? "" : " " + info));
            }
        }

        //static public void 
    }
}
