EF with Web API Demo

NOTE: To inspect generated SQL you can use SQL Profiler
      and connect to (localdb)\v11.0

Part A: Reverse Engineer Additional Entities

1. Re-add Data Model to generate additional entities
   - Rename NorthwindSlim to NorthwindSlim_Old
   - Delete files Product.cs and Category.cs from Data project
     > To see them, show all files for the project
   - Add an ADO.NET Entity Data Model
     > Name it NorthwindSlim
     > Select Code First from Database
     > Select NorthwindSlim data connection
     > Uncheck option to save connection string
	 > Answer 'No' to the prompt for copying the database
     > Select Customer, Order, OrderDetail, Product, Category
  - Copy code from ctor and OnModelCreating of NorthwindSlim_Old

    // Instruct Code First to use an existing database
    Database.SetInitializer<NorthwindSlim>(null);

    // Remove the pluralizing table name convention
    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
     
 2. Link added classes from the Entities project
    - Add existing items, navigate to Data project
    - Add As Link: Customer.cs, Order.cs, OrderDetail.cs
    - Exclude entities from the Data project
    - Re-build solution
    
Part B: Customers and Orders Controllers

1. Add a Customers controller to the Web project
   - Right-click Controllers folder, Add Controller
     > Select Web API 2 Controller - Empty
   - Add Get method returning customers sorted by CustomerId
   
	// GET api/customers
	[ResponseType(typeof(IEnumerable<Customer>))]
	public async Task<IHttpActionResult> Get()
	{
		using (var dbContext = new NorthwindSlim())
		{
			var customers = await dbContext.Customers
				.OrderBy(p => p.CustomerId)
				.ToListAsync();
			return Ok(customers);
		}
	}
	
2. Add Orders Controller
   - Select Web API 2 Controller with actions using EF
     > Model class: Order
     > Data context class: NorthwindSlim
     > Check use async controller actions
     > Controller name: OrdersController
     
3. Refactor the Orders Controller
   - Rename the NorthwindSlim db field to _dbContext
   - Replace the GetOrders method as follows:
   
	// GET: api/Orders?customerId = 5
	[ResponseType(typeof(IEnumerable<Order>))]
	public async Task<IHttpActionResult> GetOrders(string customerId)
	{
		var orders = await _dbContext.Orders
			.Include(o => o.Customer)
			.Include("OrderDetails.Product")
			.Where(o => o.CustomerId == customerId)
			.ToListAsync();
		return Ok(orders);
	}

- Replace GetOrder as follows:

	// GET: api/Orders/5
	[ResponseType(typeof(Order))]
	public async Task<IHttpActionResult> GetOrder(int id)
	{
		var order = await _dbContext.Orders
			.Include(o => o.Customer)
			.Include("OrderDetails.Product")
			.SingleOrDefaultAsync(o => o.OrderId == id);
		if (order == null) return NotFound();
		return Ok(order);
	}

4. Update PostOrder method to populate detail products
   - Add the following code to PostOrder before calling Orders.Add:

    // Populate detail products
    foreach (var detail in order.OrderDetails)
    {
        var detail1 = detail;
        detail.Product = await _dbContext.Products.SingleAsync(
            p => p.ProductId == detail1.ProductId);
    }

5. Refactor PutOrder method
   - Change ResponseType from void to Order
   - Remove id parameter
     > Also remove code checking for incorrect id
     > Change id to orderId in catch block
   - Change return to Ok(order)
   - Refactor OrderExists method to use AnyAsync

    private async Task<bool> OrderExists(int id)
    {
        return await _dbContext.Orders.AnyAsync(e => e.OrderId == id);
    }

   - Refactor try/catch to await OrderExists outside catch:
   
    Exception exception = null;
    try
    {
        await _dbContext.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException updateEx)
    {
        exception = updateEx;
    }
    if (exception != null)
    {
        if (!await OrderExists(order.OrderId))
            return NotFound();
        throw exception;
    }

6. Refactor DeleteOrder method
   - Change ResponseType to void
   - Update code to retrieve order with details
   - Return Conflict if not found
   - Remove details in reverse order
   - Then remove order
   - Save changes
   - Return Ok()

    // DELETE: api/Orders/5
    public async Task<IHttpActionResult> DeleteOrder(int id)
    {
        // Retrieve existing order
        var order = await _dbContext.Orders
            .Include(o => o.OrderDetails) // Include child entities
            .SingleOrDefaultAsync(o => o.OrderId == id);
        if (order == null) return Conflict();

        // Remove details in reverse order
        for (int i = order.OrderDetails.Count - 1; i > -1; i--)
        {
            var detail = order.OrderDetails.ElementAt(i);
            _dbContext.OrderDetails.Remove(detail);
        }
        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }
   
Part C: Refactor client with CRUD operations

1. Refactor client to retreive customers
   - In client.GetAsync, change products to customers
   - In ReadAsync, change Product to Customer
     > Also update foreach loop
   - Test client with updated web service
     
2. Add method to print an order with details to console

    private static void PrintOrderWithDetails(Order o)
    {
        Console.WriteLine("{0} {1}",
            o.OrderId,
            o.OrderDate.GetValueOrDefault().ToShortDateString());
        foreach (var od in o.OrderDetails)
        {
            Console.WriteLine("\t{0} {1} {2} {3}",
                od.OrderDetailId,
                od.Product.ProductName,
                od.Quantity,
                od.UnitPrice.ToString("c"));
        }
    }

3. Get customer orders

    // Get customer orders
    Console.WriteLine("\nCustomer Id:");
    string customerId = Console.ReadLine();
    response = client.GetAsync("orders?customerId=" + customerId).Result;
    response.EnsureSuccessStatusCode();

    // Read response content
    var orders = response.Content.ReadAsAsync<List<Order>>
        (new[] { formatter }).Result;
    foreach (var o in orders)
    {
        PrintOrderWithDetails(o);
    }

	- Test by running the service, then run the client console app

4. Create a new order and send POST request

    // Create a new order
    Console.WriteLine("\nPress Enter to create a new order");
    Console.ReadLine();
    var newOrder = new Order
    {
        CustomerId = customerId,
        OrderDate = DateTime.Today,
        ShippedDate = DateTime.Today.AddDays(1),
        OrderDetails = new List<OrderDetail>
        {
            new OrderDetail { ProductId = 1, Quantity = 5, UnitPrice = 10 },
            new OrderDetail { ProductId = 2, Quantity = 10, UnitPrice = 20 },
            new OrderDetail { ProductId = 4, Quantity = 40, UnitPrice = 40 }
        }
    };

    // Post the new order
    response = client.PostAsync("orders", newOrder, formatter).Result;
    response.EnsureSuccessStatusCode();
    var order = response.Content.ReadAsAsync<Order>(new[] { formatter }).Result;
    PrintOrderWithDetails(order);

	- With the service still running, run the client console app

5. Advance the order date to update the order you just created

    // Update the order date
    Console.WriteLine("\nPress Enter to update order date");
    Console.ReadLine();
    order.OrderDate = order.OrderDate.GetValueOrDefault().AddDays(1);

    // Put the updated order
    response = client.PutAsync("orders", order, formatter).Result;
    response.EnsureSuccessStatusCode();
    order = response.Content.ReadAsAsync<Order>(new[] { formatter }).Result;
    PrintOrderWithDetails(order);

	- With the service still running, run the client console app

6. Delete the updated order, then verify that it was deleted

    // Delete the order
    Console.WriteLine("\nPress Enter to delete the order");
    Console.ReadLine();

    // Send delete
    response = client.DeleteAsync("Orders/" + order.OrderId).Result;
    response.EnsureSuccessStatusCode();

    // Verify delete
    response = client.GetAsync("Orders/" + order.OrderId).Result;
    if (!response.IsSuccessStatusCode)
        Console.WriteLine("Order deleted");
    Console.WriteLine("Press Enter to exit");
    Console.ReadLine();

	- With the service still running, run the client console app

