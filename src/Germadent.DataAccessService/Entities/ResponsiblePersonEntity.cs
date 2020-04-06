using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.DataAccessService.Entities
{
    public class ResponsiblePersonEntity
    {
        /// <summary>
        /// Идентификатор ответственного лица
        /// </summary>
        public int ResponsiblePersonId { get; set; }
        /// <summary>
        /// Идентификатор заказчика (внешний ключ)
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// ФИО ответственного лица
        /// </summary>
        public string ResponsiblePerson { get; set; }
        /// <summary>
        /// Должность ответственного лица
        /// </summary>
        public string RP_Position  { get; set; }
        /// <summary>
        /// Телефон ответственного лица
        /// </summary>
        public string RP_Phone { get; set; }
        /// <summary>
        /// Электропочта ответственного лица
        /// </summary>
        public string RP_Email { get; set; }
        /// <summary>
        /// Описание ответственного лица
        /// </summary>
        public string RP_Description { get; set; }
    }
}
