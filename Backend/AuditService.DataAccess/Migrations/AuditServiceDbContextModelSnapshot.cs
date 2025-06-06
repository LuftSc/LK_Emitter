﻿// <auto-generated />
using System;
using System.Collections.Generic;
using AuditService.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AuditService.DataAccess.Migrations
{
    [DbContext(typeof(AuditServiceDbContext))]
    partial class AuditServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EmitterPersonalAccount.Core.Domain.Models.Postgres.ActionsReport", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<DateTime>("DateOfGeneration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ActionsReports");
                });

            modelBuilder.Entity("EmitterPersonalAccount.Core.Domain.Models.Postgres.PartialModels.EmitterProjection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("IssuerId")
                        .HasColumnType("integer");

                    b.ComplexProperty<Dictionary<string, object>>("EmitterInfo", "EmitterPersonalAccount.Core.Domain.Models.Postgres.PartialModels.EmitterProjection.EmitterInfo#EmitterInfo", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("FullName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("IO_PERS");

                            b1.Property<string>("INN")
                                .IsRequired()
                                .HasMaxLength(14)
                                .HasColumnType("character varying(14)")
                                .HasColumnName("INN");

                            b1.Property<string>("Jurisdiction")
                                .IsRequired()
                                .HasMaxLength(15)
                                .HasColumnType("character varying(15)")
                                .HasColumnName("COD_CITIZEN");

                            b1.Property<string>("ShortName")
                                .IsRequired()
                                .HasMaxLength(70)
                                .HasColumnType("character varying(70)")
                                .HasColumnName("F_PERS");

                            b1.ComplexProperty<Dictionary<string, object>>("OGRN", "EmitterPersonalAccount.Core.Domain.Models.Postgres.PartialModels.EmitterProjection.EmitterInfo#EmitterInfo.OGRN#OGRNInfo", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<DateOnly>("DateOfAssignment")
                                        .HasColumnType("date")
                                        .HasColumnName("DT_OGRN");

                                    b2.Property<string>("Issuer")
                                        .IsRequired()
                                        .HasMaxLength(60)
                                        .HasColumnType("character varying(60)")
                                        .HasColumnName("GIV_OGRN");

                                    b2.Property<string>("Number")
                                        .IsRequired()
                                        .HasMaxLength(20)
                                        .HasColumnType("character varying(20)")
                                        .HasColumnName("OGRN");
                                });

                            b1.ComplexProperty<Dictionary<string, object>>("Registration", "EmitterPersonalAccount.Core.Domain.Models.Postgres.PartialModels.EmitterProjection.EmitterInfo#EmitterInfo.Registration#RegistrationInfo", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<string>("Issuer")
                                        .HasMaxLength(60)
                                        .HasColumnType("character varying(60)")
                                        .HasColumnName("GIVEN");

                                    b2.Property<string>("Number")
                                        .HasMaxLength(20)
                                        .HasColumnType("character varying(20)")
                                        .HasColumnName("S_N");

                                    b2.Property<DateOnly>("RegistrationDate")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("date")
                                        .HasDefaultValue(new DateOnly(1, 1, 1))
                                        .HasColumnName("DT_DOC");
                                });
                        });

                    b.HasKey("Id");

                    b.ToTable("Emitters", (string)null);
                });

            modelBuilder.Entity("EmitterPersonalAccount.Core.Domain.Models.Postgres.PartialModels.UserProjection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("EmitterPersonalAccount.Core.Domain.Models.Postgres.UserActionLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ActionType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AdditionalDataJSON")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Actions");
                });

            modelBuilder.Entity("EmitterPersonalAccount.Core.Domain.Models.Postgres.UserEmitter", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EmitterId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "EmitterId");

                    b.HasIndex("EmitterId");

                    b.ToTable("UserEmitter");
                });

            modelBuilder.Entity("EmitterPersonalAccount.Core.Domain.Models.Postgres.UserEmitter", b =>
                {
                    b.HasOne("EmitterPersonalAccount.Core.Domain.Models.Postgres.PartialModels.EmitterProjection", null)
                        .WithMany()
                        .HasForeignKey("EmitterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EmitterPersonalAccount.Core.Domain.Models.Postgres.PartialModels.UserProjection", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
