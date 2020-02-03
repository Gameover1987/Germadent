using System.Linq;
using Germadent.Rma.Model;
using NUnit.Framework;
using Germadent.Rma.App.ViewModels;

namespace Germadent.Rma.App.Test
{
    [TestFixture]
    public class ToothViewModelTest
    {
        /// <summary>
        /// Должен инициализироваться из DTO
        /// </summary>
        /// <param name="hasBridge"></param>
        /// <param name="material"></param>
        /// <param name="prostheticType"></param>
        [TestCase(true, "ZrO", "Каркас")]
        [TestCase(false, "E.MAX", "другая конструкция")]
        public void ShouldInitializeFromDto(bool hasBridge, string material, string prostheticType)
        {
            // Given
            var target = CreateTarget();

            // When
            target.Initialize(new ToothDto
            {
                HasBridge = hasBridge,
                MaterialName = material,
                ProstheticsName = prostheticType
            });

            // Then
            Assert.AreEqual(hasBridge, target.HasBridge);
            Assert.AreEqual(material, target.SelectedMaterial.DisplayName);
            Assert.AreEqual(prostheticType, target.SelectedProstheticsType.DisplayName);
        }

        [TestCase(10, 1, "ZrO", 1, "Каркас", true)]
        public void ShouldGetDto(int toothNumber, int materialId, string materialName, string prosthtics, int prostheticsId, bool hasBridge)
        {
            // Given
            var expectedDto = new ToothDto
            {
                HasBridge = hasBridge,
                MaterialId = materialId,
                MaterialName = materialName,
                ProstheticsId = prostheticsId,
                ProstheticsName = prosthtics,
                ToothNumber = toothNumber
            };
            var target = CreateTarget();

            // When
            var actualDto = target.ToDto();

            // Then
            
        }

        /// <summary>
        /// При выборе должен отмечаться только один материал, и зуб должен становиться отмеченным
        /// </summary>
        [Test]
        public void ShouldCheckOnlyOneMaterial()
        {
            // Given
            var target = CreateTarget();

            // When
            target.Materials.First().IsChecked = true;
            target.Materials.Last().IsChecked = true;

            // Then
            Assert.AreEqual(target.SelectedMaterial, target.Materials.Last());
            Assert.IsTrue(target.Materials.Where(x => x != target.SelectedMaterial).All(x => x.IsChecked == false));
            Assert.IsTrue(target.IsChecked);
        }

        /// <summary>
        /// При выборе должен отмечаться только один тип протезирования, и зуб должен становиться отмеченным
        /// </summary>
        [Test]
        public void ShouldCheckOnlyOneProstheticsType()
        {
            // Given
            var target = CreateTarget();

            // When
            target.ProstheticTypes.First().IsChecked = true;
            target.ProstheticTypes.Last().IsChecked = true;

            // Then
            Assert.AreEqual(target.SelectedProstheticsType, target.ProstheticTypes.Last());
            Assert.IsTrue(target.ProstheticTypes.Where(x => x != target.SelectedProstheticsType).All(x => x.IsChecked == false));
            Assert.IsTrue(target.IsChecked);
        }

        private ToothViewModel CreateTarget()
        {
            return new ToothViewModel(GetMaterials(), GetProsthticTypes());
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
