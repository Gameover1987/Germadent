using Germadent.Rma.Model;
using System.Linq;
using FluentAssertions;
using Germadent.Rma.App.ViewModels.ToothCard;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Germadent.Rma.App.Test
{
    [TestClass]
    public class ToothViewModelTest
    {
        /// <summary>
        /// Должен инициализироваться из DTO
        /// </summary>
        /// <param name="hasBridge"></param>
        /// <param name="material"></param>
        /// <param name="prostheticType"></param>
        [DataRow(true, "ZrO", "Каркас")]
        [DataRow(false, "E.MAX", "другая конструкция")]
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

        [DataRow(10, 1, "ZrO", 1, "Каркас", true)]
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
        [TestMethod]
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
        [TestMethod]
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
        [DataRow("Культя", "Каркас", "ZrO", true, "0 - Культя/Каркас/ZrO/Мост")]
        [DataRow(null, "Каркас", "ZrO", true, "0 - Каркас/ZrO/Мост")]
        [DataRow(null, null, "ZrO", true, "0 - ZrO/Мост")]
        [DataRow(null, null, null, true, "0 - Мост")]
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

        private static DictionaryItemDto[] GetProstheticConditions()
        {
            var conditions = new[]
            {
                new DictionaryItemDto{Name = "Культя", Id = 1},
                new DictionaryItemDto{Name = "Имплант", Id = 2},
            };
            return conditions;
        }

        private static DictionaryItemDto[] GetMaterials()
        {
            var materials = new[]
            {
                new DictionaryItemDto {Name = "ZrO"},
                new DictionaryItemDto {Name = "PMMA mono"},
                new DictionaryItemDto {Name = "PMMA multi"},
                new DictionaryItemDto {Name = "WAX"},
                new DictionaryItemDto {Name = "MIK"},
                new DictionaryItemDto {Name = "CAD-Temp mono"},
                new DictionaryItemDto {Name = "CAD-Temp multi"},
                new DictionaryItemDto {Name = "Enamik mono"},
                new DictionaryItemDto {Name = "Enamik multi"},
                new DictionaryItemDto {Name = "SUPRINITY"},
                new DictionaryItemDto {Name = "Mark II"},
                new DictionaryItemDto {Name = "WAX"},
                new DictionaryItemDto {Name = "TriLuxe forte"},
                new DictionaryItemDto {Name = "Ti"},
                new DictionaryItemDto {Name = "E.MAX"},
            };

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].Id = i + 1;
            }

            return materials;
        }

        private static DictionaryItemDto[] GetProsthticTypes()
        {
            var ptostheticTypes = new[]
            {
                new DictionaryItemDto {Name = "Каркас", Id = 1},
                new DictionaryItemDto {Name = "Каркас винт. фикс", Id = 2},
                new DictionaryItemDto {Name = "Абатмент", Id = 3},
                new DictionaryItemDto {Name = "Полная анатомия", Id = 4},
                new DictionaryItemDto {Name = "Временная конструкция", Id = 5},
                new DictionaryItemDto {Name = "другая конструкция", Id = 6},
            };

            return ptostheticTypes;
        }
    }
}
