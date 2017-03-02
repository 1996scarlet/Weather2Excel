using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherToExcel
{
    class WeatherResults
    {
        public WeatherLocation location { get; set; }
        public WeatherNow now { get; set; }
        public string last_update { get; set; }
    }

    class WeatherLocation
    {
        public string id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public string path { get; set; }
        public string timezone { get; set; }
        public string timezone_offset { get; set; }
    }

    class WeatherNow
    {
        public string text { get; set; }
        public string code { get; set; }
        public string temperature { get; set; }
        public string feels_like { get; set; }
        public string pressure { get; set; }
        public string humidity { get; set; }
        public string visibility { get; set; }
        public string wind_direction { get; set; }
        public string wind_direction_degree { get; set; }
        public string wind_speed { get; set; }
        public string wind_scale { get; set; }
        public string clouds { get; set; }
        public string dew_point { get; set; }
    }
}
