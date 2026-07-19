using System;

namespace Entity
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ModuleInjectAttribute : Attribute { }
}