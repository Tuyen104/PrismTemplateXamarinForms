using System;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace PrismTemplate.Services
{
    public interface IConfigurationService
    {
        string MyLocationMapURL { get; }
        Dictionary<string, Location> DefaultLocations { get; }

    }

    public class ConfigurationService : IConfigurationService
    {
        public string MyLocationMapURL => "https://www.google.com/maps/d/viewer?mid=10PuesF-R9GFIDURJdH00mxcodzVDN-CT&hl=ja&z=15&ll={0}%2C{1}";
        public virtual Dictionary<string, Location> DefaultLocations => new Dictionary<string, Location> {
            {"Chiang Mai", new Location(18.783462, 98.995553)},
        };
    }
}
