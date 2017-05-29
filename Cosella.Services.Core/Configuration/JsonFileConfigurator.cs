using System;
using Cosella.Services.Core.Interfaces;
using log4net;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace Cosella.Services.Core.Configuration
{
    internal class JsonFileConfigurator : IConfigurator
    {
        private readonly ILog _log;
        private readonly Dictionary<string, Dictionary<string, object>> _config;

        public JsonFileConfigurator(ILog log)
        {
            _log = log;

            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string fileName = $"{path}\\config.json";
            if (File.Exists(fileName))
            {
                string configData = File.ReadAllText(fileName);
                _config = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(configData);
            }
            else
            {
                _config = new Dictionary<string, Dictionary<string, object>>();
            }
        }

        public string GetString(string section, string property)
        {
            return _config?[section]?[property]?.ToString();
        }
    }
}