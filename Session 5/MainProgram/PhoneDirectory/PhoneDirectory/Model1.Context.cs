﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PhoneDirectoryEntities : DbContext
    {
        public PhoneDirectoryEntities()
            : base("name=PhoneDirectoryEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Группа_контактов> Группа_контактов { get; set; }
        public virtual DbSet<Должности> Должности { get; set; }
        public virtual DbSet<Компания> Компания { get; set; }
        public virtual DbSet<Контакты> Контакты { get; set; }
    }
}
