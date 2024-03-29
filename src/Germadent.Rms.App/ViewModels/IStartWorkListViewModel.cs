﻿using Germadent.Client.Common.Converters;
using Germadent.Model;
using Germadent.Model.Production;

namespace Germadent.Rms.App.ViewModels
{
    public interface IStartWorkListViewModel
    {
        void Initialize(OrderDto orderDto);

        WorkDto[] GetWorks();
    }
}
