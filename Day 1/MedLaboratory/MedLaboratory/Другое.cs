//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedLaboratory
{
    using System;
    using System.Collections.Generic;
    
    public partial class Другое
    {
        public int Код_другого { get; set; }
        public Nullable<int> Код_пользователя { get; set; }
        public Nullable<int> Серия_паспорта { get; set; }
        public Nullable<int> Номер_паспорта { get; set; }
        public string Телефон { get; set; }
        public string E_mail { get; set; }
        public Nullable<int> Номер_страхового_полиса { get; set; }
        public Nullable<int> Код_типа_страхового_полиса { get; set; }
        public Nullable<int> Код_страховой_компании { get; set; }
        public Nullable<System.DateTime> Дата_рождения { get; set; }
    
        public virtual Пользователи Пользователи { get; set; }
        public virtual Страховая_компания Страховая_компания { get; set; }
        public virtual Тип_страхового_полиса Тип_страхового_полиса { get; set; }
    }
}
