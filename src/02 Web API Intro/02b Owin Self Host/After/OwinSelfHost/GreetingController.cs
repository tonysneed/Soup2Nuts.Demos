using System;
using System.Collections.Generic;
using System.Web.Http;

namespace OwinSelfHost
{
    public class GreetingController : ApiController
    {
        readonly string[] _greetings = { "Hello", "Howdy", "Ciao", "Yo" };

        // GET api/greeting
        public IEnumerable<string> Get()
        {
            return _greetings;
        }

        // GET api/greeting/1 
        public string Get(int id)
        {
            return _greetings[id - 1];
        }
    }
}
