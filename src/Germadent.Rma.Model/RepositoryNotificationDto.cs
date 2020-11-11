using System;
using System.Collections.Generic;
using System.Text;

namespace Germadent.Rma.Model
{
    public enum RepositoryType
    {
        PriceGroup,
        PricePosition
    }

    public class RepositoryNotificationDto
    {
        public RepositoryNotificationDto(RepositoryType repositoryType)
        {
            RepositoryType = repositoryType;
        }

        public RepositoryType RepositoryType { get;  }

        public object[] AddedItems { get; set; }

        public object[] ChangedItems { get; set; }

        public int[] DeletedItems { get; set; }
    }
}
