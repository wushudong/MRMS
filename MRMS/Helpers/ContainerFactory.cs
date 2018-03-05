using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRMS.Helpers
{
    public sealed class ContainerFactory
    {
        public static IUnityContainer Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly IUnityContainer instance = new UnityContainer();
        }
    }
}
