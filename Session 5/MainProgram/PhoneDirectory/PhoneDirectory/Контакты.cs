//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PhoneDirectory
{
    using System;
    using System.Collections.Generic;
    
    public partial class Контакты
    {
        public int Код_контакта { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        public string Номер_телефона { get; set; }
        public string E_mail { get; set; }
        public Nullable<int> Код_компании { get; set; }
        public Nullable<int> Код_должности { get; set; }
        public Nullable<int> Код_группы_контактов { get; set; }
        public Nullable<System.DateTime> Дата_рождения { get; set; }
        public byte[] Фотография { get; set; }
    
        public virtual Группа_контактов Группа_контактов { get; set; }
        public virtual Должности Должности { get; set; }
        public virtual Компания Компания { get; set; }
    }
}
