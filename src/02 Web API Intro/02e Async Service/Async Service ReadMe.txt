Async Services Demo ReadMe

This demonstrates how to build scalable service.

NOTE: Controller actions use Task.Delay to simulate I/O latency.

1. Refactor the methods in ValueController to perform async I/O.
   - Change signature to return Task<IHttpActionResult>
   - Insert async keyword after public
   - Remove .Wait() from Task.Delay
   - Add await keyword before Task.Delay

Although the response time is the same, the service is actually
creating fewer threads when multiple clients invoke service
actions which perform async I/O.