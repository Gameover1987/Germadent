using System;
using Germadent.ServiceClient.Model;

namespace Germadent.ServiceClient.Operation
{
    public class MockRmaOperations : IRmaOperations
    {
        public Order[] GetOrders()
        {
            var orders = new Order[]
            {
                new Order
                {
                    Created = DateTime.Now,
                    Patient = "Иванов Иван Иванович",
                    Employee = "Петров Петр Петрович",
                    Customer = "ООО Рога и Копыта",
                    Material = "ZrO"
                },
                new Order
                {
                    Created = DateTime.Now,
                    Patient = "Сергей Сергеевич Серегин",
                    Employee = "Петров Петр Петрович",
                    Customer = "ООО Рога и Копыта",
                    Material = "PMMA multi"
                },
                new Order
                {
                    Created = DateTime.Now,
                    Patient = "Антов Антонович Антонов",
                    Employee = "Петров Петр Петрович",
                    Customer = "ООО Рога и Копыта",
                    Material = "PMMA mono"
                },
                new Order
                {
                    Created = DateTime.Now,
                    Patient = "Иванов Иван Иванович",
                    Employee = "Петров Петр Петрович",
                    Customer = "ООО Рога и Копыта",
                    Material = "ZrO"
                },
                new Order
                {
                    Created = DateTime.Now,
                    Patient = "Сергей Сергеевич Серегин",
                    Employee = "Петров Петр Петрович",
                    Customer = "ООО Рога и Копыта",
                    Material = "PMMA multi"
                },
                new Order
                {
                    Created = DateTime.Now,
                    Patient = "Антов Антонович Антонов",
                    Employee = "Петров Петр Петрович",
                    Customer = "ООО Рога и Копыта",
                    Material = "PMMA mono"
                },
                new Order
                {
                    Created = DateTime.Now,
                    Patient = "Иванов Иван Иванович",
                    Employee = "Петров Петр Петрович",
                    Customer = "ООО Рога и Копыта",
                    Material = "ZrO"
                },
                new Order
                {
                    Created = DateTime.Now,
                    Patient = "Сергей Сергеевич Серегин",
                    Employee = "Петров Петр Петрович",
                    Customer = "ООО Рога и Копыта",
                    Material = "PMMA multi"
                },
                new Order
                {
                    Created = DateTime.Now,
                    Patient = "Антов Антонович Антонов",
                    Employee = "Петров Петр Петрович",
                    Customer = "ООО Рога и Копыта",
                    Material = "PMMA mono"
                },
            };

            return orders;
        }
    }
}