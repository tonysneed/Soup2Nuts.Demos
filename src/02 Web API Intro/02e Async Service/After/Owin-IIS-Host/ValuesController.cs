using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Owin_IIS_Host
{
    public class ValuesController : ApiController
    {
        private const int DelaySeconds = 3;

        private static readonly Dictionary<int, string> Values = 
            new Dictionary<int, string>
        {
            { 1, "value1"}, { 2, "value2"}, { 3, "value3"}, { 4, "value4"}, { 5, "value5"},
        }; 

        // GET api/values
        [ResponseType(typeof(IEnumerable<string>))]
        //public IHttpActionResult Get()
        public async Task<IHttpActionResult> Get()
        {
            // Simulate I/O latency
            //Task.Delay(TimeSpan.FromSeconds(DelaySeconds)).Wait();
            await Task.Delay(TimeSpan.FromSeconds(DelaySeconds));

            IEnumerable<string> values = Values.Values.AsEnumerable();
            return Ok(values);
        }

        // GET api/values/5
        [ResponseType(typeof(string))]
        //public IHttpActionResult Get(int id)
        public async Task<IHttpActionResult> Get(int id)
        {
            // Simulate I/O latency
            //Task.Delay(TimeSpan.FromSeconds(DelaySeconds)).Wait();
            await Task.Delay(TimeSpan.FromSeconds(DelaySeconds));

            if (!Values.ContainsKey(id))
                return NotFound();
            return CreatedAtRoute("DefaultApi", new { id }, Values[id]);
        }

        // POST api/values
        [ResponseType(typeof(string))]
        //public IHttpActionResult Post([FromBody]string value)
        public async Task<IHttpActionResult> Post([FromBody]string value)
        {
            // Simulate I/O latency
            //Task.Delay(TimeSpan.FromSeconds(DelaySeconds)).Wait();
            await Task.Delay(TimeSpan.FromSeconds(DelaySeconds));

            var id = Values.Count + 1;
            Values.Add(id, value);
            return CreatedAtRoute("DefaultApi", new { id }, Values[id]);
        }

        // PUT api/values/5
        [ResponseType(typeof(string))]
        //public IHttpActionResult Put(int id, [FromBody]string value)
        public async Task<IHttpActionResult> Put(int id, [FromBody]string value)
        {
            // Simulate I/O latency
            //Task.Delay(TimeSpan.FromSeconds(DelaySeconds)).Wait();
            await Task.Delay(TimeSpan.FromSeconds(DelaySeconds));

            Values[id] = value;
            return Ok(Values[id]);
        }

        // DELETE api/values/5
        //public IHttpActionResult Delete(int id)
        public async Task<IHttpActionResult> Delete(int id)
        {
            // Simulate I/O latency
            //Task.Delay(TimeSpan.FromSeconds(DelaySeconds)).Wait();
            await Task.Delay(TimeSpan.FromSeconds(DelaySeconds));

            if (!Values.ContainsKey(id))
                return NotFound();
            Values.Remove(id);
            return Ok();
        }
    }
}
