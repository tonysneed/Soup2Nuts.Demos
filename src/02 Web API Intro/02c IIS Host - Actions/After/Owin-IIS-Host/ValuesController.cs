using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Owin_IIS_Host
{
    public class ValuesController : ApiController
    {
        private static readonly Dictionary<int, string> Values = 
            new Dictionary<int, string>
        {
            { 1, "value1"}, { 2, "value2"}, { 3, "value3"}, { 4, "value4"}, { 5, "value5"},
        }; 

        // GET api/values
        [ResponseType(typeof(IEnumerable<string>))]
        public IHttpActionResult Get()
        {
            IEnumerable<string> values = Values.Values.AsEnumerable();
            //return Request.CreateResponse(HttpStatusCode.OK, values);
            return Ok(values);
        }

        // GET api/values/5
        [ResponseType(typeof(string))]
        public IHttpActionResult Get(int id)
        {
            if (!Values.ContainsKey(id))
                //throw new HttpResponseException(HttpStatusCode.NotFound);
                return NotFound();
            //return Request.CreateResponse(HttpStatusCode.OK, Values[id]);
            return CreatedAtRoute("DefaultApi", new { id }, Values[id]);
        }

        // POST api/values
        [ResponseType(typeof(string))]
        public IHttpActionResult Post([FromBody]string value)
        {
            var id = Values.Count + 1;
            Values.Add(id, value);
            //var response = Request.CreateResponse(HttpStatusCode.Created, Values[id]);
            //response.Headers.Location = new Uri(Request.RequestUri, "values/" + id);
            //return response;
            return CreatedAtRoute("DefaultApi", new { id }, Values[id]);
        }

        // PUT api/values/5
        [ResponseType(typeof(string))]
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            Values[id] = value;
            //return Request.CreateResponse(HttpStatusCode.OK, Values[id]);
            return Ok(Values[id]);
        }

        // DELETE api/values/5
        public IHttpActionResult Delete(int id)
        {
            if (!Values.ContainsKey(id))
                //throw new HttpResponseException(HttpStatusCode.NotFound);
                return NotFound();
            Values.Remove(id);
            //return Request.CreateResponse(HttpStatusCode.OK);
            return Ok();
        }
    }
}
