﻿namespace Germadent.Rma.Model.Pricing
{
    public class PricePositionDto
    {
        public int Id { get; set; }

        public int PriceGroupId { get; set; }

        public string UserCode { get; set; }

        public string Name { get; set; }

        public int MaterialId { get; set; }

        public ProductSetDto[] Products { get; set; }

        public override string ToString()
        {
            return string.Format("Id={0}, UserCode={1}, Name={2}", Id, UserCode, Name);
        }
    }
}