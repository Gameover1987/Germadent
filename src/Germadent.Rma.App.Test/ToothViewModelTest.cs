using System.Linq;
using FluentAssertions;
using Germadent.Rma.Model;
using NUnit.Framework;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.ToothCard;

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
        public void ShouldGetDto(int toothNumber, int materialId, string materialName, int prostheticsId, string prosthtics, bool hasBridge)
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
            target.Initialize(expectedDto);

            // When
            var actualDto = target.ToDto();

            // Then
            actualDto.Should().BeEquivalentTo(expectedDto);
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
            Assert.IsTrue(target.IsChanged);
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
            Assert.IsTrue(target.IsChanged);
        }

        /// <summary>
        /// Должен возвращать описание для зуба
        /// </summary>
        [TestCase("Культя", "Каркас", "ZrO", true, "Культя/Каркас/ZrO/Мост")]
        [TestCase(null, "Каркас", "ZrO", true, "Каркас/ZrO/Мост")]
        [TestCase(null, null, "ZrO", true, "ZrO/Мост")]
        [TestCase(null, null, null, true, "Мост")]
        public void ShouldGetCorrectDescription(string prostheticsCondition, string prosthetics, string material, bool hasBridge, string expectedDescription)
        {
            // Given
            var target = CreateTarget();
            var selectedProstheticCondition = target.ProstheticConditions.FirstOrDefault(x => x.DisplayName == prostheticsCondition);
            if (selectedProstheticCondition != null)
                selectedProstheticCondition.IsChecked = true;

            var selectedProsthetics = target.ProstheticTypes.FirstOrDefault(x => x.DisplayName == prosthetics);
            if (selectedProsthetics != null)
                selectedProsthetics.IsChecked = true;

            var selectedMaterial = target.Materials.FirstOrDefault(x => x.DisplayName == material);
            if (selectedMaterial != null)
                selectedMaterial.IsChecked = true;

            target.HasBridge = hasBridge;

            // When
            var actualDescription = target.Description;

            // Then
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        private ToothViewModel CreateTarget()
        {
            return new ToothViewModel(GetProstheticConditions(), GetProsthticTypes(), GetMaterials());
        }

        private static ProstheticConditionDto[] GetProstheticConditions()
        {
            var conditions = new[]
            {
                new ProstheticConditionDto{Name = "Культя", Id = 1},
                new ProstheticConditionDto{Name = "Имплант", Id = 2},
            };
            return conditions;
        }

        private static MaterialDto[] GetMaterials()
        {
            var materials = new[]
            {
                new MaterialDto {MaterialName = "ZrO"},
                new MaterialDto {MaterialName = "PMMA mono"},
                new MaterialDto {MaterialName = "PMMA multi"},
                new MaterialDto {MaterialName = "WAX"},
                new MaterialDto {MaterialName = "MIK"},
                new MaterialDto {MaterialName = "CAD-Temp mono"},
                new MaterialDto {MaterialName = "CAD-Temp multi"},
                new MaterialDto {MaterialName = "Enamik mono"},
                new MaterialDto {MaterialName = "Enamik multi"},
                new MaterialDto {MaterialName = "SUPRINITY"},
                new MaterialDto {MaterialName = "Mark II"},
                new MaterialDto {MaterialName = "WAX"},
                new MaterialDto {MaterialName = "TriLuxe forte"},
                new MaterialDto {MaterialName = "Ti"},
                new MaterialDto {MaterialName = "E.MAX"},
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
