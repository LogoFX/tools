using System;

namespace LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto
{
    [Serializable]
    public sealed class ConfigurationVersion : IEquatable<ConfigurationVersion>
    {
        private readonly int _version;

        public ConfigurationVersion(int version)
        {
            _version = version;
        }

        public int Version
        {
            get { return _version;}
        }

        public static bool operator ==(ConfigurationVersion v1, ConfigurationVersion v2)
        {
            if (ReferenceEquals(v1, null))
            {
                return ReferenceEquals(v2, null);
            }

            return v1.Equals(v2);
        }

        public static bool operator !=(ConfigurationVersion v1, ConfigurationVersion v2)
        {
            return !(v1 == v2);
        }

        public bool Equals(ConfigurationVersion other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return _version == other._version;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is ConfigurationVersion && Equals((ConfigurationVersion) obj);
        }

        public override int GetHashCode()
        {
            return _version;
        }
    }
}