using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Servicing.Models.sql_rendimento_consignado;

namespace Servicing.Data
{
    public partial class sql_rendimento_consignadoContext : DbContext
    {
        public sql_rendimento_consignadoContext()
        {
        }

        public sql_rendimento_consignadoContext(DbContextOptions<sql_rendimento_consignadoContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.EnderecosCliente>()
              .HasOne(i => i.Clientes)
              .WithMany(i => i.EnderecosCliente)
              .HasForeignKey(i => i.IdCliente)
              .HasPrincipalKey(i => i.IdCliente);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Lotes>()
              .HasOne(i => i.Cedentes)
              .WithMany(i => i.Lotes)
              .HasForeignKey(i => i.IdCedente)
              .HasPrincipalKey(i => i.IdCedente);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Operacoes>()
              .HasOne(i => i.Clientes)
              .WithMany(i => i.Operacoes)
              .HasForeignKey(i => i.IdCliente)
              .HasPrincipalKey(i => i.IdCliente);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Operacoes>()
              .HasOne(i => i.Lotes)
              .WithMany(i => i.Operacoes)
              .HasForeignKey(i => i.IdLote)
              .HasPrincipalKey(i => i.IdLote);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.TelefonesCliente>()
              .HasOne(i => i.Clientes)
              .WithMany(i => i.TelefonesCliente)
              .HasForeignKey(i => i.IdCliente)
              .HasPrincipalKey(i => i.IdCliente);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Transacoes>()
              .HasOne(i => i.Operacoes)
              .WithMany(i => i.Transacoes)
              .HasForeignKey(i => i.IdOperacao)
              .HasPrincipalKey(i => i.IdOperacao);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Clientes>()
              .Property(p => p.DataNascimento)
              .HasColumnType("datetime2(0)");

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Lotes>()
              .Property(p => p.DataAquisicao)
              .HasColumnType("datetime2(0)");

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Lotes>()
              .Property(p => p.DataFInalizacao)
              .HasColumnType("datetime2");

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Operacoes>()
              .Property(p => p.InicioContrato)
              .HasColumnType("datetime2(0)");

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Operacoes>()
              .Property(p => p.FinalContrato)
              .HasColumnType("datetime2(0)");

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Operacoes>()
              .Property(p => p.UltimoDesconto)
              .HasColumnType("datetime2(0)");

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Operacoes>()
              .Property(p => p.ProximoDesconto)
              .HasColumnType("datetime2(0)");

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Transacoes>()
              .Property(p => p.DataDesconto)
              .HasColumnType("datetime2(0)");

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Clientes>()
              .Property(p => p.DataNascimento)
              .HasPrecision(0);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Lotes>()
              .Property(p => p.DataAquisicao)
              .HasPrecision(0);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Operacoes>()
              .Property(p => p.ValorOriginal)
              .HasPrecision(18,2);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Operacoes>()
              .Property(p => p.ValorParcela)
              .HasPrecision(18,2);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Operacoes>()
              .Property(p => p.InicioContrato)
              .HasPrecision(0);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Operacoes>()
              .Property(p => p.FinalContrato)
              .HasPrecision(0);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Operacoes>()
              .Property(p => p.UltimoDesconto)
              .HasPrecision(0);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Operacoes>()
              .Property(p => p.ValorUltimoDesconto)
              .HasPrecision(18,2);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Operacoes>()
              .Property(p => p.ProximoDesconto)
              .HasPrecision(0);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Operacoes>()
              .Property(p => p.ValorProximoDesconto)
              .HasPrecision(18,2);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Transacoes>()
              .Property(p => p.ValorOriginal)
              .HasPrecision(18,2);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Transacoes>()
              .Property(p => p.ValorParcela)
              .HasPrecision(18,2);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Transacoes>()
              .Property(p => p.DataDesconto)
              .HasPrecision(0);

            builder.Entity<Servicing.Models.sql_rendimento_consignado.Transacoes>()
              .Property(p => p.ValorDesconto)
              .HasPrecision(18,2);
            this.OnModelBuilding(builder);
        }

        public DbSet<Servicing.Models.sql_rendimento_consignado.Cedentes> Cedentes { get; set; }

        public DbSet<Servicing.Models.sql_rendimento_consignado.Clientes> Clientes { get; set; }

        public DbSet<Servicing.Models.sql_rendimento_consignado.EnderecosCliente> EnderecosCliente { get; set; }

        public DbSet<Servicing.Models.sql_rendimento_consignado.Lotes> Lotes { get; set; }

        public DbSet<Servicing.Models.sql_rendimento_consignado.Operacoes> Operacoes { get; set; }

        public DbSet<Servicing.Models.sql_rendimento_consignado.TelefonesCliente> TelefonesCliente { get; set; }

        public DbSet<Servicing.Models.sql_rendimento_consignado.Transacoes> Transacoes { get; set; }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}