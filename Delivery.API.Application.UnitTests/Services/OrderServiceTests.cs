namespace Delivery.API.Application.UnitTests.Services;

 public class OrderServiceTests
 {
     [Fact]
     public async Task Create_CreateOrderAndReturnOrderId()
     {
         // Arrange
        var fakeOrderRepository = new FakeOrderRepository();
         var distanceCalculator = new DistanceCalculator();
         var settings = new OrderSettings
         {
             CostPerKm = 1.8f
         };
         var costCalculator = new CostCalculator(settings);
         var orderService = new OrderService(fakeOrderRepository, distanceCalculator, costCalculator);
         
         var dto = new CreateOrderDto
         {
             UserId = Guid.NewGuid(),
             Pickup = new CoordinateDto
             {
                 Latitude = 10.0,
                 Longitude = 11.0
             },
             Dropoff = new CoordinateDto
             {
                 Latitude = 11.0,
                 Longitude = 12.0
             }
         };
         
         // Act
         var result = await orderService.Create(dto, CancellationToken.None);

         // Assert
         Assert.NotEqual(Guid.Empty, result);
     }
     
     [Fact]
     public async Task FindById_ReturnOrderDetailsDtoWhenOrderFound()
     {
         // Arrange
         var fakeOrderRepository = new FakeOrderRepository();
         var distanceCalculator = new DistanceCalculator();
         var settings = new OrderSettings
         {
             CostPerKm = 1.8f
         };
         var costCalculator = new CostCalculator(settings);
         var orderService = new OrderService(fakeOrderRepository, distanceCalculator, costCalculator);
         
         var dto = new CreateOrderDto
         {
             UserId = Guid.NewGuid(),
             Pickup = new CoordinateDto
             {
                 Latitude = 13.0,
                 Longitude = 13.0
             },
             Dropoff = new CoordinateDto
             {
                 Latitude = 10.0,
                 Longitude = 10.0
             }
         };
         var order = await orderService.Create(dto, CancellationToken.None);
         
         // Act
         var result = await orderService.FindById(order, CancellationToken.None);
         
         // Assert
         Assert.NotNull(result);
         Assert.Equal(dto.UserId, result.UserId);
         Assert.Equal(dto.Pickup.Longitude, result.Pickup.Longitude);
         Assert.Equal(dto.Pickup.Latitude, result.Pickup.Latitude);
         Assert.Equal(dto.Dropoff.Longitude, result.Dropoff.Longitude);
         Assert.Equal(dto.Dropoff.Latitude, result.Dropoff.Latitude);
     }
     
     [Fact]
     public async Task FindById_ReturnOrderDetailsDtoWhenOrderNotFound()
     {
         // Arrange
         var fakeOrderRepository = new FakeOrderRepository();
         var distanceCalculator = new DistanceCalculator();
         var settings = new OrderSettings
         {
             CostPerKm = 1.8f
         };
         var costCalculator = new CostCalculator(settings);
         var orderService = new OrderService(fakeOrderRepository, distanceCalculator, costCalculator);
         
         // Act
         var orderDetails = await orderService.FindById(Guid.NewGuid(), CancellationToken.None);
     
         // Assert
         Assert.Null(orderDetails);
     }
     
     [Fact]
     public async Task DeleteById()
     {
         // Arrange
         var fakeOrderRepository = new FakeOrderRepository();
         var distanceCalculator = new DistanceCalculator();
         var settings = new OrderSettings
         {
             CostPerKm = 1.8f
         };
         var costCalculator = new CostCalculator(settings);
         var orderService = new OrderService(fakeOrderRepository, distanceCalculator, costCalculator);
         
         var dto = new CreateOrderDto
         {
             UserId = Guid.NewGuid(),
             Pickup = new CoordinateDto
             {
                 Latitude = 9.0,
                 Longitude = 9.0
             },
             Dropoff = new CoordinateDto
             {
                 Latitude = 9.0,
                 Longitude = 9.0
             }
         };
         var orderId = await orderService.Create(dto, CancellationToken.None);
         
         // Act
         var IsDeleted = await orderService.DeleteById(orderId, dto.UserId, CancellationToken.None);
         
         // Assert
         Assert.True(IsDeleted);
     }
}

 