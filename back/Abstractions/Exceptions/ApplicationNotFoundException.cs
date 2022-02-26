using Abstractions.Enums;
using Abstractions.Models;

namespace Abstractions.Exceptions
{
    [Serializable]
    public class ApplicationNotFoundException : Exception
    {
        public ApplicationNotFoundException(string name) : base($"There are no application named {name}")
        {
        }

        public ApplicationNotFoundException(string name, AppVersion version) : base($"There are no application named {name} for version {version}")
        {
        }
        public ApplicationNotFoundException(string name, AppVersion version, AppArch arch) : base($"There are no application named {name} for version {version} on {arch.ToString()}")
        {
        }
    }
}