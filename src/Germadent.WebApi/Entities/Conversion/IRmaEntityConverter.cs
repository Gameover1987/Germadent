using Germadent.Model;
using Germadent.Model.Pricing;

namespace Germadent.WebApi.Entities.Conversion
{
    public interface IRmaEntityConverter
    {
        OrderDto ConvertToOrder(OrderEntity entity);

        ToothDto[] ConvertToToothCard(ToothEntity[] entities, bool getPricesAsStl);

        OrderLiteDto ConvertToOrderLite(OrderLiteEntity entity);

        DictionaryItemDto ConvertToDictionaryItem(DictionaryItemEntity entity);

        AdditionalEquipmentDto ConvertToAdditionalEquipment(AdditionalEquipmentEntity entity);

        CustomerDto ConvertToCustomer(CustomerEntity entity);

        ResponsiblePersonDto ConvertToResponsiblePerson(ResponsiblePersonEntity entity);

        AttributeDto ConvertToAttribute(AttributesEntity entity);

        PriceGroupDto ConvertToPriceGroup(PriceGroupEntity entity);

        PricePositionDto ConvertToPricePosition(PricePositionEntity entity);

        PriceDto ConvertToPrice(PriceEntity entity);

        ProductDto ConvertToProduct(ProductEntity entity);

        ToothEntity[] ConvertFromToothDto(ToothDto toothDto, bool setPriceStl);
    }
}
