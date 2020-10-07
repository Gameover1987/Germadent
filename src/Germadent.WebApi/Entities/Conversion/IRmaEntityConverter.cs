using Germadent.Rma.Model;

namespace Germadent.WebApi.Entities.Conversion
{
    public interface IRmaEntityConverter
    {
        OrderDto ConvertToOrder(OrderEntity entity);

        ToothDto ConvertToTooth(ToothEntity entity);

        OrderLiteDto ConvertToOrderLite(OrderLiteEntity entity);

        DictionaryItemDto ConvertToDictionaryItem(DictionaryItemEntity entity);

        AdditionalEquipmentDto ConvertToAdditionalEquipment(AdditionalEquipmentEntity entity);

        ReportListDto ConvertToExcel(ExcelEntity entity);

        CustomerDto ConvertToCustomer(CustomerEntity entity);

        ResponsiblePersonDto ConvertToResponsiblePerson(ResponsiblePersonEntity entity);

        PriceGroupDto ConvertToPriceGroup(PriceGroupEntity entity);

        PricePositionDto ConvertToPricePosition(PricePositionEntity entity);

        ProductSetDto ConvertToProductSetForTooth(ProductSetForToothEntity entity);
    }
}
