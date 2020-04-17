using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.DataAccessService.Entities
{
    public class AttributesSetEntity
    {
        public int WorkOrderId { get; set; }
        public int AttributeId { get; set; }
        public string AttributeKeyName { get; set; }
        public int AttrValueId { get; set; }
        public string AttributeValue { get; set; }
    }
}
