using System;
namespace PrismTemplate.Services
{
    public class Request
    {
        public Method RequestMethod { get; set; }
        public Uri RequestUri { get; set; }
        public object RequestParam { get; set; }
    }
}
