using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.DataAccessService.Entities
{
    public class AttributesEntity
    {
        /// <summary>
        /// Идентификатор атрибута
        /// </summary>
        public int AttributeId { get; set; }

        /// <summary>
        /// Ключевое имя атрибута
        /// </summary>
        public string AttributeKeyName { get; set; }

        /// <summary>
        /// Наименование атрибута атрибута по-русски
        /// </summary>
        public string AttributeName { get; set; }
    }
}
