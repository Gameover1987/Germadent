using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;

namespace Germadent.WebApi.Entities.Conversion
{
    public interface IRmaEntityConverter
    {
        OrderDto ConvertToOrder(OrderEntity entity);

        ToothDto[] ConvertToToothCard(ToothEntity[] entities);

        OrderLiteDto ConvertToOrderLite(OrderLiteEntity entity);

        DictionaryItemDto ConvertToDictionaryItem(DictionaryItemEntity entity);

        AdditionalEquipmentDto ConvertToAdditionalEquipment(AdditionalEquipmentEntity entity);

        ReportListDto ConvertToExcel(ExcelEntity entity);

        CustomerDto ConvertToCustomer(CustomerEntity entity);

        ResponsiblePersonDto ConvertToResponsiblePerson(ResponsiblePersonEntity entity);

        PriceGroupDto ConvertToPriceGroup(PriceGroupEntity entity);

        PricePositionDto ConvertToPricePosition(PricePositionEntity entity);

        PriceDto ConvertToPrice(PriceEntity entity);

        ProductDto ConvertToProduct(ProductEntity entity);

        ToothEntity[] ConvertFromToothDto(ToothDto toothDto);

        ProductSetForPriceGroupDto ConvertToProductSetForPriceGroup(ProductSetForPriceGroupEntity entity);
    }
}
