﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Api.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Bulletins> Bulletins { get; set; }
        public virtual DbSet<operationFeatures> operationFeatures { get; set; }
        public virtual DbSet<Operations> Operations { get; set; }
        public virtual DbSet<Property> Property { get; set; }
        public virtual DbSet<Senders> Senders { get; set; }
        public virtual DbSet<SendersUsers> SendersUsers { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Names> Names { get; set; }
    }
}
