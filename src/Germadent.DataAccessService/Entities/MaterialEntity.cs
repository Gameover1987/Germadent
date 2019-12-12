using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.DataAccessService.Entities
{

    /// <summary>
    /// Материал
    /// </summary>
    public class MaterialEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int MaterialId { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// Признак неиспользуемости
        /// </summary>
        public bool FlagUnused { get; set; }
    }
}
