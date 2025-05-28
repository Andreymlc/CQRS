using System.Text;
using DDD_CQRS.Application.Command;
using DDD_CQRS.Application.Helper;
using DDD_CQRS.Application.Query;
using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Views;
using MediatR;

namespace DDD_CQRS;

// ReSharper disable once InconsistentNaming
public class ConsoleUI(IMediator mediator)
{
    private static OrderView? _currentOrder;
    private static OrderStatus _currentOrderStatus = OrderStatus.Created;
    
    public void Start()
    {
        int choice;
        do
        {
            ShowMainMenu(GetMenuItems(), out var itemsDictionary);
            choice = ReadIntInput();
            
            try
            {
                Console.WriteLine("\n");
                itemsDictionary[choice]();
            }
            catch (Exception e)
            {
                Console.WriteLine("\nОШИБКА! " + e.Message);
            }
            
        } while (choice != 0);
    }

    private static void ShowMainMenu(List<MenuItem> menuItems, out Dictionary<int, Action> itemsDictionary)
    {
        Console.WriteLine();
        if (_currentOrder is not null)
            Console.WriteLine($"\nТекущий заказ: {_currentOrder.Id} ({_currentOrderStatus.ToRussianString()}) ");
        
        itemsDictionary = new Dictionary<int, Action>();
        var index = 1;
        
        Console.WriteLine("=============== Система управления заказами ===============");
        foreach (var item in menuItems.Where(m => m.IsAvailable()))
        {
            Console.WriteLine($"{index}. {item.Description}");
            
            itemsDictionary.Add(index, item.Action);
            index++;
        }
        
        Console.Write("Выберите действие: ");
    }

    private void ChoiceOrder()
    {
        Console.WriteLine("Все заказы:");
        var orders = GetAllOrders();

        for (var i = 0; i < orders.Count; i++)
            Console.WriteLine($"{i + 1} -> {orders[i].Id}");

        Console.Write("Выберите заказ: ");
        var choice = ReadIntInput();
        
        _currentOrder = orders[choice - 1];
        _currentOrderStatus = _currentOrder.Status;
        
        Console.WriteLine("Заказ выбран");
    }

    private void CreateNewOrder()
    {
        var random = new Random();
        var orderItems = new List<OrderItem>();
        Dish[] dishes =
        [
            new(Guid.NewGuid(), "Шашлык", 479), new(Guid.NewGuid(), "Теплый салат", 320),
            new(Guid.NewGuid(), "Чизкейк", 400)
        ];

        foreach (var dish in dishes)
        {
            var quantity = 1 + random.Next(5);
            orderItems.Add(OrderItem.Create(dish, quantity));
        }

        _currentOrder = OrderHelper.Map(mediator.Send(new CreateOrder { OrderItems = orderItems }).Result);
        _currentOrderStatus = _currentOrder.Status;
    }

    private void GetOrderInfo()
    {
        Console.Write("Чтобы посмотреть информацию по текущему заказу нажмите enter.\n" +
                      "Для просмотра информации по другому заказу введите его Id: ");
        var orderId = ReadGuidInput();

        if (orderId == Guid.Empty)
            Console.WriteLine($"\nИнформация по текущему заказу:\n{_currentOrder}");
        else
        {
            var order = mediator.Send(new GetOrderById { Id = orderId }).Result;

            Console.WriteLine($"Информация по заказу:\n{order}");
        }
    }

    private void GetOrderHistory()
    {
        Console.Write("Введите кол-во заказов: ");
        var numberDays = ReadIntInput();
        
        var history = mediator.Send(new GetOrderHistory { NumberDays = numberDays }).Result;
        
        if (history.Count > 0)
            Console.Write(FormatOrdersTable(history));
        else
            Console.WriteLine("В данный момент нет ни одного заказа");
    }

    private void GetReport()
    {
        Console.Write("Количество последних заказов для получения: ");
        var quantityReports = ReadIntInput();
        
        var reports = mediator.Send(new GetReport { QuantityReports = quantityReports}).Result;

        foreach (var report in reports)
            Console.WriteLine(report + "\n");
    }

    private void AddDishToOrder()
    {
        Console.Write("Введите название блюда: ");
        var name = Console.ReadLine() ?? throw new ArgumentException("Введите название");
        Console.Write("Введите цену блюда: ");
        var price = decimal.Parse(Console.ReadLine()!);
        Console.Write("Введите количество блюда: ");
        var quantity = ReadIntInput();

        var order = OrderHelper.Map(
            mediator.Send(new AddDishToOrder
            {
                OrderId = _currentOrder!.Id,
                Name = name,
                Price = price,
                Quantity = quantity
            }).Result);
        
        _currentOrder = order;
    }

    private void ChangeDishesQuantityInOrderItem()
    {
        var items = _currentOrder!.Items;
        
        Console.WriteLine("Выберете продукт для изменения количества: ");
        for (var i = 0; i < items.Count ; i++)
            Console.WriteLine($"{i + 1} -> {items[i]}");

        Console.Write("Выберете блюдо: ");
        var choice = ReadIntInput();

        Console.Write("Новое количество: ");
        var quantity = ReadIntInput();

        var order = OrderHelper.Map(
            mediator.Send(new ChangeDishesQuantityInOrderItem
            {
                OrderId = _currentOrder.Id,
                DishId = items[choice - 1].DishView.Id,
                NewQuantity = quantity
            }).Result);
        
        _currentOrder = order;
    }

    private void GetOrderStatus() =>
        Console.WriteLine($"Статус: {mediator.Send(new GetOrderStatus { OrderId = _currentOrder!.Id }).Result.ToRussianString()}");

    private void ChangeOrderStatus()
    {
        var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();
        Console.WriteLine("Доступные статусы:");
        for (var i = 0; i < statuses.Length; i++)
            Console.WriteLine($"{i + 1} -> {statuses[i].ToRussianString()}");
        
        Console.Write("Выберите новый статус:");
        var choice = ReadIntInput();
        
        var order = OrderHelper.Map(
            mediator.Send(new ChangeOrderStatus 
            { 
                OrderId = _currentOrder!.Id,
                OrderStatus = statuses[choice - 1]
            }).Result);

        _currentOrder = order;
        _currentOrderStatus = order.Status;
    }
    
    private void RemoveDishFromOrder()
    {
        var items = _currentOrder!.Items;
        
        Console.WriteLine("Все продукты: ");
        for (var i = 0; i < items.Count ; i++)
            Console.WriteLine($"{i + 1} -> {items[i]}");

        Console.Write("Выберете блюдо для изменения количества: ");
        var choice = ReadIntInput();
        
        var order = OrderHelper.Map(
            mediator.Send(new RemoveDishFromOrder
            {
                OrderId = _currentOrder.Id,
                DishId = items[choice - 1].DishView.Id,
            }).Result);
        
        _currentOrder = order;
    }

    private IReadOnlyList<OrderView> GetAllOrders() => mediator.Send(new GetAllOrders()).Result;
    
    private static string FormatOrdersTable(IReadOnlyList<OrderView> orders)
    {
        var sb = new StringBuilder().Append('\n');

        for (var i = 0; i < orders.Count; i++)
        {
            sb.AppendLine(orders[i].ToString());
        
            if (i < orders.Count - 1)
                sb.AppendLine("---------------------------------------------------------\n");
        }
    
        return sb.ToString();
    }

    private static int ReadIntInput()
    {
        try
        {
            return int.Parse(Console.ReadLine()!);
        }
        catch
        {
            return -1;
        }
    }
    
    private static Guid ReadGuidInput()
    {
        try
        {
            return Guid.Parse(Console.ReadLine()!);
        }
        catch
        {
            return Guid.Empty;
        }
    }
    
    private List<MenuItem> GetMenuItems()
    {
        return
        [
            new MenuItem { Description = "Создать новый заказ", Action = CreateNewOrder },
            
            new MenuItem { Description = "Посмотреть историю заказов (последних n)", Action = GetOrderHistory },
            
            new MenuItem { Description = "Посмотреть отчет", Action = GetReport },
            
            new MenuItem { Description = "Выбрать заказ", Action = ChoiceOrder, IsAvailable = () => _currentOrder is not null },
            
            new MenuItem { Description = "Посмотреть информацию по заказу", Action = GetOrderInfo, IsAvailable = () => _currentOrder is not null },

            new MenuItem
            {
                Description = "Добавить блюдо в текущий заказ",
                Action = AddDishToOrder,
                IsAvailable = () => _currentOrder?.Status == OrderStatus.Created
            },

            new MenuItem
            {
                Description = "Изменить количество определенного блюда в текущем заказе",
                Action = ChangeDishesQuantityInOrderItem,
                IsAvailable = () => _currentOrder?.Status == OrderStatus.Created
            },
            
            new MenuItem
            {
                Description = "Удалить блюдо из текущего заказа",
                Action = RemoveDishFromOrder,
                IsAvailable = () => _currentOrder?.Status == OrderStatus.Created
            },

            new MenuItem
            {
                Description = "Изменить статус текущего заказа",
                Action = ChangeOrderStatus,
                IsAvailable = () => _currentOrder?.Status != OrderStatus.Canceled
            }
        ];
    }
}
