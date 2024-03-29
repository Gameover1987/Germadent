﻿using System;

namespace Germadent.WebApi.Entities
{
    /// <summary>
    /// Заказ-наряд (все типы)
    /// </summary>
    public partial class OrderEntity
    {
        /// <summary>
        /// Идентификатор заказ наряда
        /// </summary>
        public int WorkOrderId { get; set; }

        /// <summary>
        /// Тип филиала
        /// </summary>
        public int BranchTypeId { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Дата изменения статуса
        /// </summary>
        public DateTime StatusChanged { get; set; }

        /// <summary>
        /// Номер заказ-наряда
        /// </summary>
        public string DocNumber { get; set; }

        /// <summary>
        /// Идентификатор заказчика
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Заказчик
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Идентификатор ответственного лица - доктора или техника
        /// </summary>
        public int ResponsiblePersonId { get; set; }

        /// <summary>
        /// ФИО доктора
        /// </summary>
        public string DoctorFullName { get; set; }

        /// <summary>
        /// ФИО техника
        /// </summary>
        public string TechnicFullName { get; set; }

        /// <summary>
        /// Телефон ответственного лица
        /// </summary>
        public string TechnicPhone { get; set; }

        /// <summary>
        /// Пациент
        /// </summary>
        public string Patient { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Описание работ
        /// </summary>
        public string WorkDescription { get; set; }

        /// <summary>
        /// ФИО администратора
        /// </summary>
        public string CreatorFullName { get; set; }

        /// <summary>
        /// Коэффициент срочности
        /// </summary>
        public float UrgencyRatio { get; set; }

        /// <summary>
        /// Флаг согласования работ
        /// </summary>
        public bool FlagWorkAccept { get; set; }

        /// <summary>
        /// Флаг наличия stl-файла
        /// </summary>
        public bool FlagStl { get; set; }

        /// <summary>
        /// Флаг безналичного расчёта
        /// </summary>
        public bool FlagCashless { get; set; }

        /// <summary>
        /// Артикул материалов
        /// </summary>
        public string ProstheticArticul { get; set; }

        /// <summary>
        /// Перечень материалов
        /// </summary>
        public string MaterialsEnum { get; set; }

        /// <summary>
        /// Комментарий к датам
        /// </summary>
        public string DateComment { get; set; }
    }
}