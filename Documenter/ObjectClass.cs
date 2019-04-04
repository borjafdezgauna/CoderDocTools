using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documenter
{
    public class ObjectClass
    {
        public string Namespace { get; }
        public string Name { get; }
        public string SrcFileName { get; }

        public List<ClassMethod> Constructors { get; } = new List<ClassMethod>();
        public List<ClassMethod> Destructors { get; } = new List<ClassMethod>();
        public List<ClassMethod> Methods { get; } = new List<ClassMethod>();

        public ObjectClass(string srcFileName, string name, string namespac= null)
        {
            SrcFileName = srcFileName;
            Name = name;
            Namespace = namespac;   
        }

        public void AddMethod(ClassMethod method)
        {
            if (method.Type == ClassMethod.MethodType.Regular)
                Methods.Add(method);
            else if (method.Type == ClassMethod.MethodType.Constructor)
                Constructors.Add(method);
            else Destructors.Add(method);
        }
    }
}
