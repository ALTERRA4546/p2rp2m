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
    
    public partial class Заказ
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Заказ()
        {
            this.Счет_страховой = new HashSet<Счет_страховой>();
            this.Услуги_заказа = new HashSet<Услуги_заказа>();
        }
    
        public int Код_заказа { get; set; }
        public Nullable<int> Код_пациент { get; set; }
        public Nullable<int> Статус_заказа { get; set; }
        public Nullable<int> Время_выполнения_заказа { get; set; }
    
        public virtual Пользователи Пользователи { get; set; }
        public virtual Статус Статус { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Счет_страховой> Счет_страховой { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Услуги_заказа> Услуги_заказа { get; set; }
    }
}
