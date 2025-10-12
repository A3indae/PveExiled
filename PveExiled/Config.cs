using Exiled;
using Exiled.API.Interfaces;

namespace PveExiled
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = true;
    }
}