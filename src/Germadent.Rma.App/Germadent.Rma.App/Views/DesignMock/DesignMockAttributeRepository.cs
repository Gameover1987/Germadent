﻿using System;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Model;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockAttributeRepository : IAttributeRepository
    {
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public AttributeDto[] Items
        {
            get
            {
                return new AttributeDto[]
                {
                    new AttributeDto {AttributeId = 1, AttributeKeyName = "CarcassColor", AttributeName = "Цвет каркаса", AttributeValue = "Vita Classical", AttributeValueId = 4},
                    new AttributeDto {AttributeId = 1, AttributeKeyName = "CarcassColor", AttributeName = "Цвет каркаса", AttributeValue = "Опак", AttributeValueId = 6},
                    new AttributeDto {AttributeId = 1, AttributeKeyName = "CarcassColor", AttributeName = "Цвет каркаса", AttributeValue = "Транслюцен", AttributeValueId = 6},
                };
            }
        }

        public event EventHandler<RepositoryChangedEventArgs<AttributeDto>> Changed;
    }
}