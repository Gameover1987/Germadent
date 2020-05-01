using System;
using System.Collections.Generic;
using System.Text;

namespace Germadent.Rma.Model
{
    /// <summary>
    /// DTO для данных элемента словаря
    /// </summary>
    public class DictionaryItemDto 
    {
        public  int Id { get; set; }

        public string Name { get; set; }

        public string DictionaryName { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Id, Name, DictionaryName);
        }
    }
}
