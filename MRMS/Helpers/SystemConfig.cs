using MRMS.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MRMS.Helpers
{
    class SystemConfig : ISystemConfig
    {
        private JObject config;
        public T GetConfig<T>(string key)
        {
            if (null == config)
            {
                ReadConfig();
            }
            return config.Value<T>(key);
        }
        public IEnumerable<T> GetArrayConfig<T>(string key)
        {
            if (null == config)
            {
                ReadConfig();
            }
            JArray ja = config.Value<JArray>(key);
            if (null == ja) return null;
            return ja.ToObject<IEnumerable<T>>();
        }

        public void ReadConfig()
        {
            var currentDirectory = new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName;
            if (currentDirectory == null)
            {
                return;
            }
            if (File.Exists(Path.Combine(currentDirectory, @"config\system.cfg")))
            {
                FileStream fs = new FileStream(Path.Combine(currentDirectory, @"config\system.cfg"), FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                try
                {
                    config = JObject.Parse(sr.ReadToEnd());
                }
                finally
                {
                    sr.Close();
                    fs.Close();
                }
            }
            else
            {
                config = new JObject();
            }
        }
        public void SetConfig<T>(string key, T value)
        {
            if (null == config)
            {
                config = new JObject();
            }
            if (null != value)
            {
                Type p = typeof(T);
                if (p.IsPrimitive || p.Name == "String" || p.Name == "Decimal")
                {
                    config[key] = JToken.FromObject(value);
                }
                else
                {
                    config[key] = JObject.FromObject(value);
                }
            }
        }

        public void SetArrayConfig<T>(string key, IEnumerable<T> value)
        {
            if (null == config)
            {
                config = new JObject();
            }
            if (null != value)
            {
                config[key] = JArray.FromObject(value);
            }
        }

        public void WriteConfig()
        {
            if (null == config)
            {
                return;
            }
            var currentDirectory = new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName;
            if (currentDirectory == null)
            {
                return;
            }
            if (!Directory.Exists(Path.Combine(currentDirectory, @"config")))//若文件夹不存在则新建文件夹   
            {
                Directory.CreateDirectory(Path.Combine(currentDirectory, @"config")); //新建文件夹   
            }
            FileStream fs = new FileStream(Path.Combine(currentDirectory, @"config\system.cfg"), FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            try
            {
                sw.Write(config.ToString());
            }
            finally
            {
                sw.Close();
                fs.Close();
            }
        }
    }
}
