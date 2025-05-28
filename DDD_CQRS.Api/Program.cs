// See https://aka.ms/new-console-template for more information

using DDD_CQRS;
using DDD_CQRS.Application;
using DDD_CQRS.Domain.Repository;
using DDD_CQRS.Infrastructure.Repository;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var serviceCollection = new ServiceCollection()
    .AddApplication()
    .AddSingleton<IOrderRepository, OrderRepository>()
    .AddSingleton<IOrderViewRepository, OrderViewRepository>()
    .AddSingleton<IReportRepository, ReportRepository>()
    .BuildServiceProvider();
    
var mediator = serviceCollection.GetRequiredService<IMediator>();

new ConsoleUI(mediator).Start();
