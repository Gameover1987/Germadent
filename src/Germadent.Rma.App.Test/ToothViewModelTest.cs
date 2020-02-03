using Germadent.Rma.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Rma.App.ViewModels;

namespace Germadent.Rma.App.Test
{
    [TestFixture]
    public class ToothViewModelTest
    {
        [TestCase(true, "Zro", "Каркас")]
        public void ShouldInitialize(bool hasBridge, string material, string prostheticType)
        {
            // Given
            var target = new ToothViewModel(GetMaterials(), GetProsthticTypes());

            // When
            target.Initialize(new ToothDto
            {
                HasBridge = hasBridge,
                MaterialName = material,
                ProstheticsName = prostheticType
            });

            // Then
            Assert.AreEqual(hasBridge, target.HasBridge);
            //Assert.AreEqual(material, target.);
            //Assert.AreEqual(hasBridge, target.HasBridge);

        }

        private static MaterialDto[] GetMaterials()
        {
            var materials = new[]
            {
                new MaterialDto {Name = "ZrO"},
                new MaterialDto {Name = "PMMA mono"},
                new MaterialDto {Name = "PMMA multi"},
                new MaterialDto {Name = "WAX"},
                new MaterialDto {Name = "MIK"},
                new MaterialDto {Name = "CAD-Temp mono"},
                new MaterialDto {Name = "CAD-Temp multi"},
                new MaterialDto {Name = "Enamik mono"},
                new MaterialDto {Name = "Enamik multi"},
                new MaterialDto {Name = "SUPRINITY"},
                new MaterialDto {Name = "Mark II"},
                new MaterialDto {Name = "WAX"},
                new MaterialDto {Name = "TriLuxe forte"},
                new MaterialDto {Name = "Ti"},
                new MaterialDto {Name = "E.MAX"},
            };

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].Id = i + 1;
            }

            return materials;
        }

        private static ProstheticsTypeDto[] GetProsthticTypes()
        {
            var ptostheticTypes = new[]
            {
                new ProstheticsTypeDto {Name = "Каркас", Id = 1},
                new ProstheticsTypeDto {Name = "Каркас винт. фикс", Id = 2},
                new ProstheticsTypeDto {Name = "Абатмент", Id = 3},
                new ProstheticsTypeDto {Name = "Полная анатомия", Id = 4},
                new ProstheticsTypeDto {Name = "Временная конструкция", Id = 5},
                new ProstheticsTypeDto {Name = "другая конструкция", Id = 6},
            };

            return ptostheticTypes;
        }
    }
}
