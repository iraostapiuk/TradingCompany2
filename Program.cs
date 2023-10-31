using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Concreate;
using DAL.Abstract;
using Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Запуск тесту для реальної бази даних SQL Server
            ///??????
            //var testClass = new TbOrderDalTest();
            //testClass.GetAllOrders();


            //Підключення до бази даних через контекст Entity Framework
            var options = new DbContextOptionsBuilder<ImdbContext>()
                .UseSqlServer("Data Source=LAPTOP-BPDBP75F;Initial Catalog=programming;Integrated Security=True;TrustServerCertificate=True;Encrypt=False")
                .Options;
            var dbContext = new ImdbContext(options);

            // Створення об'єктів для роботи з DAL
            var orderDal = new TbOrderDal(dbContext);
            var orderProductDal = new TbOrderProductDal(dbContext);
            var personDal = new TbPersonDal(dbContext);
            var productDal = new TbProductDal(dbContext);
            var productHistoryDal = new TbProductHistoryDal(dbContext);
            var providerDal = new TbProviderDal(dbContext);


            Console.WriteLine("Виберіть дію:");
            Console.WriteLine("1. Зробити дію з замовленням");
            Console.WriteLine("2. Зробити дію з продуктом для  замовлення");
            Console.WriteLine("3. Зробити дію з Персон");
            Console.WriteLine("4. Зробити дію з продуктом");
            Console.WriteLine("5. Зробити дію з історією продукції");
            Console.WriteLine("6. Зробити дію з постачальником");
            Console.WriteLine("7. Вийти");

            while (true)
            {
                Console.Write("Ваш вибір: ");
                var choice = Console.ReadLine();

                switch (choice)
                {

                    case "1":

                        Console.WriteLine("Виберіть дію:");
                        Console.WriteLine("1. Переглянути всі замовлення");
                        Console.WriteLine("2. Додати замовлення");
                        Console.WriteLine("3. Оновити запис");
                        Console.WriteLine("4. Видалити запис");
                        Console.WriteLine("5. Пошук по id ");
                        Console.WriteLine("6. Вийти");
                        string innerChoice = Console.ReadLine();
                        switch (innerChoice)
                        {

                            case "1":
                                Console.WriteLine("Перегляд усіх записів:");
                                // Виводимо всі записи для кожної сутності
                                List<TbOrder> allOrders = orderDal.GetAll(); // Отримуємо всі записи з таблиці TbOrder

                                foreach (var order in allOrders)
                                {
                                    string formattedOrder = TbOrderDal.Format(order); // Форматуємо кожний об'єкт
                                    Console.WriteLine(formattedOrder); // Виводимо на консоль
                                }
                                break;
                            case "2":
                                Console.WriteLine("Додавання нового запису:");
                                // Створюємо новий об'єкт TbOrder
                                TbOrder newOrder = new TbOrder();

                                Console.Write("Введіть PersonId: ");
                                if (int.TryParse(Console.ReadLine(), out int personId))
                                {
                                    newOrder.PersonId = personId;
                                }
                                else
                                {
                                    Console.WriteLine("Невірний формат для PersonId.");
                                    return;
                                }

                                Console.Write("Введіть OrderDate (у форматі yyyy-MM-dd): ");
                                if (DateTime.TryParse(Console.ReadLine(), out DateTime orderDate))
                                {
                                    newOrder.OrderDate = orderDate;
                                }
                                else
                                {
                                    Console.WriteLine("Невірний формат для OrderDate.");
                                    return;
                                }
                                TbOrder insertedOrder = orderDal.Insert(newOrder);

                                Console.WriteLine($"Новий запис був доданий. ID нового запису: {insertedOrder.OrderId}");

                                break;
                            case "3":
                                Console.WriteLine("Оновлення запису:");

                                Console.Write("Введіть ID запису, який ви бажаєте оновити: ");
                                if (int.TryParse(Console.ReadLine(), out int id))
                                {
                                    var existingOrder = orderDal.GetOrderById(id);

                                    if (existingOrder != null)
                                    {
                                        Console.WriteLine("Поточні значення:");
                                        Console.WriteLine($"PersonId: {existingOrder.PersonId}");
                                        Console.WriteLine($"OrderDate: {existingOrder.OrderDate}");
                                        // Виводьте поточні значення для інших полів, якщо потрібно

                                        Console.Write("Введіть нове значення для PersonId: ");
                                        if (int.TryParse(Console.ReadLine(), out int newPersonId))
                                        {
                                            existingOrder.PersonId = newPersonId;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Невірний формат для PersonId.");
                                            return;
                                        }

                                        Console.Write("Введіть нове значення для OrderDate (у форматі yyyy-MM-dd): ");
                                        if (DateTime.TryParse(Console.ReadLine(), out DateTime newOrderDate))
                                        {
                                            existingOrder.OrderDate = newOrderDate;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Невірний формат для OrderDate.");
                                            return;
                                        }

                                        // Оновлюємо запис
                                        var updatedOrder = orderDal.Update(existingOrder);
                                        Console.WriteLine("Запис було оновлено.");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Запис з ID {id} не знайдено.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Невірний формат для ID запису.");
                                }
                                break;
                            case "4":
                                Console.WriteLine("Видалення запису:");
                                Console.Write("Введіть ID запису, який ви бажаєте видалити: ");
                                if (int.TryParse(Console.ReadLine(), out int inputid))
                                {
                                    var existingOrder = orderDal.GetOrderById(inputid);

                                    if (existingOrder != null)
                                    {
                                        Console.WriteLine("Ви впевнені, що бажаєте видалити цей запис? (Y/N)");
                                        string confirmation = Console.ReadLine();

                                        if (confirmation.Equals("Y", StringComparison.OrdinalIgnoreCase))
                                        {
                                            orderDal.Delete(inputid);
                                            Console.WriteLine("Запис було успішно видалено.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Видалення скасовано.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Запис з ID {inputid} не знайдено.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Невірний формат для ID запису.");
                                }
                                break;
                            case "5":
                                Console.WriteLine("Пошук замовлення по id:");
                                Console.Write("Введіть Id замовлення, яке ви шукаєте: ");
                                if (int.TryParse(Console.ReadLine(), out int orderId))
                                {
                                    TbOrder order = orderDal.GetOrderById(orderId);
                                    if (order != null)
                                    {
                                        Console.WriteLine("Знайдене замовлення:");
                                        string formattedOrder = TbOrderDal.Format(order); // Використовуємо функцію Format
                                        Console.WriteLine(formattedOrder);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Замовлення з ID {orderId} не знайдено.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Введено некоректний ID.");
                                }
                                break;

                            case "6":
                                return;

                            default:
                                Console.WriteLine("Невірний вибір. Будь ласка, виберіть правильну опцію.");
                                break;

                        }
                        break;


                    case "2":
                        Console.WriteLine("Виберіть дію:");
                        Console.WriteLine("1. Переглянути всі замовлення");
                        Console.WriteLine("2. Додати замовлення");
                        Console.WriteLine("3. Оновити запис");
                        Console.WriteLine("4. Видалити запис");
                        Console.WriteLine("5. Пошук по id ");
                        Console.WriteLine("6. Вийти");
                        string innerChoice2 = Console.ReadLine();
                        switch (innerChoice2)
                        {
                            case "1":
                                Console.WriteLine("Перегляд усіх записів:");
                                // Виводимо всі записи для кожної сутності
                                List<TbOrderProduct> allOrderProducts = orderProductDal.GetAll(); // Отримуємо всі записи з таблиці TbOrderProduct

                                foreach (var orderProduct in allOrderProducts)
                                {
                                    string formattedOrderProduct = TbOrderProductDal.Format(orderProduct); // Форматуємо кожний об'єкт
                                    Console.WriteLine(formattedOrderProduct); // Виводимо на консоль
                                }
                                break;
                            case "2":
                                Console.WriteLine("Додавання нового запису:");
                                // Створюємо новий об'єкт TbOrderProduct
                                TbOrderProduct newOrderProduct = new TbOrderProduct();

                                Console.Write("Введіть OrderId: ");
                                if (int.TryParse(Console.ReadLine(), out int orderId))
                                {
                                    newOrderProduct.OrderId = orderId;
                                }
                                else
                                {
                                    Console.WriteLine("Невірний формат для OrderId.");
                                    return;
                                }

                                Console.Write("Введіть ProductId: ");
                                if (int.TryParse(Console.ReadLine(), out int productId))
                                {
                                    newOrderProduct.ProductId = productId;
                                }
                                else
                                {
                                    Console.WriteLine("Невірний формат для ProductId.");
                                    return;
                                }

                                Console.Write("Введіть кількість (Count): ");
                                if (int.TryParse(Console.ReadLine(), out int count))
                                {
                                    newOrderProduct.Count = count;
                                }
                                else
                                {
                                    Console.WriteLine("Невірний формат для Count.");
                                    return;
                                }

                                TbOrderProduct insertedOrderProduct = orderProductDal.Insert(newOrderProduct);

                                Console.WriteLine($"Новий запис був доданий. OrderProduct ID: {insertedOrderProduct.OrderId}, Product ID: {insertedOrderProduct.ProductId}");

                                break;
                            case "3":
                                Console.WriteLine("Оновлення запису:");

                                Console.Write("Введіть Order ID запису, який ви бажаєте оновити: ");
                                if (int.TryParse(Console.ReadLine(), out int inputorderId))
                                {
                                    Console.Write("Введіть Product ID запису, який ви бажаєте оновити: ");
                                    if (int.TryParse(Console.ReadLine(), out int inputproductId))
                                    {
                                        var existingOrderProduct = orderProductDal.GetCountById(inputorderId, inputproductId);

                                        if (existingOrderProduct != null)
                                        {
                                            Console.WriteLine("Поточні значення:");
                                            Console.WriteLine($"OrderId: {existingOrderProduct.OrderId}");
                                            Console.WriteLine($"ProductId: {existingOrderProduct.ProductId}");
                                            Console.WriteLine($"Count: {existingOrderProduct.Count}");

                                            Console.Write("Введіть нове значення для Count: ");
                                            if (int.TryParse(Console.ReadLine(), out int newCount))
                                            {
                                                existingOrderProduct.Count = newCount;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Невірний формат для Count.");
                                                return;
                                            }

                                            // Оновлюємо запис
                                            var updatedOrderProduct = orderProductDal.Update(existingOrderProduct);
                                            Console.WriteLine("Запис було оновлено.");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Запис з Order ID {inputorderId} та Product ID {inputproductId} не знайдено.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Невірний формат для Product ID.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Невірний формат для Order ID.");
                                }
                                break;
                            case "4":
                                Console.WriteLine("Видалення запису:");
                                Console.Write("Введіть Order ID: ");
                                if (int.TryParse(Console.ReadLine(), out int inputorderId2))
                                {
                                    Console.Write("Введіть Product ID: ");
                                    if (int.TryParse(Console.ReadLine(), out int inputproductId2))
                                    {
                                        orderProductDal.Delete(inputorderId2, inputproductId2);
                                        Console.WriteLine("Запис було успішно видалено.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Невірний формат для Product ID.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Невірний формат для Order ID.");
                                }
                                break;
                            case "5":
                                Console.WriteLine("Пошук запису в TbOrderProduct за ID:");
                                Console.Write("Введіть ID замовлення: ");
                                if (int.TryParse(Console.ReadLine(), out int inputorderId3))
                                {
                                    Console.Write("Введіть ID продукту: ");
                                    if (int.TryParse(Console.ReadLine(), out int inputproductId3))
                                    {
                                        TbOrderProduct orderProduct = orderProductDal.GetCountById(inputorderId3, inputproductId3);
                                        if (orderProduct != null)
                                        {
                                            Console.WriteLine("Знайдений запис:");
                                            string formattedOrderProduct = TbOrderProductDal.Format(orderProduct);
                                            Console.WriteLine(formattedOrderProduct);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Запис не знайдено.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Введено некоректний ID продукту.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Введено некоректний ID замовлення.");
                                }
                                break;
                            case "6":
                                return;

                            default:
                                Console.WriteLine("Невірний вибір. Будь ласка, виберіть правильну опцію.");
                                break;

                        }

                        break;

                    case "3":

                        break;

                    case "4":

                        break;
                    case "5":

                        break;
                    case "6":

                        break;

                    case "7":
                        Console.WriteLine("Дякую, що скористалися програмою.");
                        return;

                    default:
                        Console.WriteLine("Невірний вибір. Будь ласка, виберіть правильну опцію.");
                        break;
                }
            }
        }
        }

    
}
        
    