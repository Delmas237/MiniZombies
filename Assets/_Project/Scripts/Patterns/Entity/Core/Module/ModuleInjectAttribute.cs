using System;

namespace EntityLib
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ModuleInjectAttribute : Attribute { }
}