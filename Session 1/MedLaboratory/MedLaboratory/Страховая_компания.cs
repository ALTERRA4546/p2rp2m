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
    
    public partial class Страховая_компания
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Страховая_компания()
        {
            this.Другое = new HashSet<Другое>();
        }
    
        public int Код_страховой_компании { get; set; }
        public string Название { get; set; }
        public string Адрес { get; set; }
        public Nullable<long> ИНН { get; set; }
        public Nullable<long> Расчётный_счёт { get; set; }
        public Nullable<long> БИК { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Другое> Другое { get; set; }
    }
}
