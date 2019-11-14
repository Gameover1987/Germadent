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

        public Material[] GetMaterials()
        {
            var materials = new[]
            {
                new Material {Name = "ZrO"},
                new Material {Name = "PMMA mono"},
                new Material {Name = "PMMA multi"},
                new Material {Name = "WAX"},
                new Material {Name = "MIK"},
                new Material {Name = "CAD-Temp mono"},
                new Material {Name = "CAD-Temp multi"},
                new Material {Name = "Enamik mono"},
                new Material {Name = "Enamik multi"},
                new Material {Name = "SUPRINITY"},
                new Material {Name = "Mark II"},
                new Material {Name = "WAX"},
                new Material {Name = "TriLuxe forte"},
                new Material {Name = "Ti"},
                new Material {Name = "E.MAX"},
            };

            return materials;
        }
    }
}