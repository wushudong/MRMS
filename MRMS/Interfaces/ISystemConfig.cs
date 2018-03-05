using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRMS.Interfaces
{
    public interface ISystemConfig
    {
        void SetConfig<T>(string key, T value);
        void SetArrayConfig<T>(string key, IEnumerable<T> value);
        T GetConfig<T>(string key);
        IEnumerable<T> GetArrayConfig<T>(string key);
        void ReadConfig();
        void WriteConfig();
    }
}
