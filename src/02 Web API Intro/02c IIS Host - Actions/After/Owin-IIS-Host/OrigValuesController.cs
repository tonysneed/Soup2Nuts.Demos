using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Owin_IIS_Host
{
    public class OrigValuesController : ApiController
    {
        private static readonly Dictionary<int, string> Values = 
            new Dictionary<int, string>
        {
            { 1, "value1"}, { 2, "value2"}, { 3, "value3"}, { 4, "value4"}, { 5, "value5"},
        }; 

        // GET api/values
        public HttpResponseMessage Get()
        {
            IEnumerable<string> values = Values.Values.AsEnumerable();
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // GET api/values/5
        public HttpResponseMessage Get(int id)
        {
            if (!Values.ContainsKey(id))
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return Request.CreateResponse(HttpStatusCode.OK, Values[id]);
        }

        // POST api/values
        public HttpResponseMessage Post([FromBody]string value)
        {
            var id = Values.Count + 1;
            Values.Add(id, value);
            var response = Request.CreateResponse(HttpStatusCode.Created, Values[id]);
            response.Headers.Location = new Uri(Request.RequestUri, "origvalues/" + id);
            return response;
        }

        // PUT api/values/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {
            Values[id] = value;
            return Request.CreateResponse(HttpStatusCode.OK, Values[id]);
        }

        // DELETE api/values/5
        public HttpResponseMessage Delete(int id)
        {
            if (!Values.ContainsKey(id))
                throw new HttpResponseException(HttpStatusCode.NotFound);
            Values.Remove(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
