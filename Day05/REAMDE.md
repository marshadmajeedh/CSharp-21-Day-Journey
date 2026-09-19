Task
→ asynchronous operation with no result value

Task<T>
→ asynchronous operation that eventually returns T

async
→ allows the method to use await

await
→ asynchronously wait for a Task

Task.Delay()
→ asynchronous waiting

Thread.Sleep()
→ blocks the current thread

sequential async
→ await one operation, then start/await next

concurrent async
→ start independent Tasks first,
  then await Task.WhenAll()

Task.WhenAll()
→ completes successfully only when all Tasks succeed

I/O-bound work
→ database, network, files, APIs
→ good candidate for async

CPU-bound/simple calculations
→ normally stay synchronous

async void
→ generally avoid except special event-handler cases

.Result / .Wait()
→ generally avoid in async flows

"async all the way"
→ preferred approach