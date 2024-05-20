﻿// <auto-generated />
using System;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Hv.Ppb302.DigitalThesis.WebClient.Migrations
{
    [DbContext(typeof(DigitalThesisDbContext))]
    [Migration("20240520112627_RemoveConnectorsBecomingsFromGeoTag")]
    partial class RemoveConnectorsBecomingsFromGeoTag
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ConnectorTagMolarMosaic", b =>
                {
                    b.Property<Guid>("ConnectorTagsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MolarMosaicsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ConnectorTagsId", "MolarMosaicsId");

                    b.HasIndex("MolarMosaicsId");

                    b.ToTable("ConnectorTagMolarMosaic");
                });

            modelBuilder.Entity("ConnectorTagMolecularMosaic", b =>
                {
                    b.Property<Guid>("ConnectorTagsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MolecularMosaicsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ConnectorTagsId", "MolecularMosaicsId");

                    b.HasIndex("MolecularMosaicsId");

                    b.ToTable("ConnectorTagMolecularMosaic");
                });

            modelBuilder.Entity("Hv.Ppb302.DigitalThesis.WebClient.Models.AssemblageTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AssemblageTags");
                });

            modelBuilder.Entity("Hv.Ppb302.DigitalThesis.WebClient.Models.ConnectorTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ConnectorTags");
                });

            modelBuilder.Entity("Hv.Ppb302.DigitalThesis.WebClient.Models.GeoTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AudioFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ConnectorTagId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PdfFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ConnectorTagId");

                    b.ToTable("GeoTags");
                });

            modelBuilder.Entity("Hv.Ppb302.DigitalThesis.WebClient.Models.KaleidoscopeTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("KaleidoscopeTags");
                });

            modelBuilder.Entity("Hv.Ppb302.DigitalThesis.WebClient.Models.MolarMosaic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AssemblageTagId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AudioFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Becomings")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PdfFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AssemblageTagId");

                    b.ToTable("MolarMosaics");
                });

            modelBuilder.Entity("Hv.Ppb302.DigitalThesis.WebClient.Models.MolecularMosaic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AssemblageTagId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AudioFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Becomings")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PdfFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AssemblageTagId");

                    b.ToTable("MolecularMosaics");
                });

            modelBuilder.Entity("Hv.Ppb302.DigitalThesis.WebClient.Models.Page", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("Hv.Ppb302.DigitalThesis.WebClient.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KaleidoscopeTagMolarMosaic", b =>
                {
                    b.Property<Guid>("KaleidoscopeTagsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MolarMosaicsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("KaleidoscopeTagsId", "MolarMosaicsId");

                    b.HasIndex("MolarMosaicsId");

                    b.ToTable("KaleidoscopeTagMolarMosaic");
                });

            modelBuilder.Entity("KaleidoscopeTagMolecularMosaic", b =>
                {
                    b.Property<Guid>("KaleidoscopeTagsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MolecularMosaicsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("KaleidoscopeTagsId", "MolecularMosaicsId");

                    b.HasIndex("MolecularMosaicsId");

                    b.ToTable("KaleidoscopeTagMolecularMosaic");
                });

            modelBuilder.Entity("ConnectorTagMolarMosaic", b =>
                {
                    b.HasOne("Hv.Ppb302.DigitalThesis.WebClient.Models.ConnectorTag", null)
                        .WithMany()
                        .HasForeignKey("ConnectorTagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hv.Ppb302.DigitalThesis.WebClient.Models.MolarMosaic", null)
                        .WithMany()
                        .HasForeignKey("MolarMosaicsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ConnectorTagMolecularMosaic", b =>
                {
                    b.HasOne("Hv.Ppb302.DigitalThesis.WebClient.Models.ConnectorTag", null)
                        .WithMany()
                        .HasForeignKey("ConnectorTagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hv.Ppb302.DigitalThesis.WebClient.Models.MolecularMosaic", null)
                        .WithMany()
                        .HasForeignKey("MolecularMosaicsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Hv.Ppb302.DigitalThesis.WebClient.Models.GeoTag", b =>
                {
                    b.HasOne("Hv.Ppb302.DigitalThesis.WebClient.Models.ConnectorTag", null)
                        .WithMany("GeoTags")
                        .HasForeignKey("ConnectorTagId");
                });

            modelBuilder.Entity("Hv.Ppb302.DigitalThesis.WebClient.Models.MolarMosaic", b =>
                {
                    b.HasOne("Hv.Ppb302.DigitalThesis.WebClient.Models.AssemblageTag", "AssemblageTag")
                        .WithMany("MolarMosaics")
                        .HasForeignKey("AssemblageTagId");

                    b.Navigation("AssemblageTag");
                });

            modelBuilder.Entity("Hv.Ppb302.DigitalThesis.WebClient.Models.MolecularMosaic", b =>
                {
                    b.HasOne("Hv.Ppb302.DigitalThesis.WebClient.Models.AssemblageTag", "AssemblageTag")
                        .WithMany("MolecularMosaics")
                        .HasForeignKey("AssemblageTagId");

                    b.Navigation("AssemblageTag");
                });

            modelBuilder.Entity("KaleidoscopeTagMolarMosaic", b =>
                {
                    b.HasOne("Hv.Ppb302.DigitalThesis.WebClient.Models.KaleidoscopeTag", null)
                        .WithMany()
                        .HasForeignKey("KaleidoscopeTagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hv.Ppb302.DigitalThesis.WebClient.Models.MolarMosaic", null)
                        .WithMany()
                        .HasForeignKey("MolarMosaicsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KaleidoscopeTagMolecularMosaic", b =>
                {
                    b.HasOne("Hv.Ppb302.DigitalThesis.WebClient.Models.KaleidoscopeTag", null)
                        .WithMany()
                        .HasForeignKey("KaleidoscopeTagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hv.Ppb302.DigitalThesis.WebClient.Models.MolecularMosaic", null)
                        .WithMany()
                        .HasForeignKey("MolecularMosaicsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Hv.Ppb302.DigitalThesis.WebClient.Models.AssemblageTag", b =>
                {
                    b.Navigation("MolarMosaics");

                    b.Navigation("MolecularMosaics");
                });

            modelBuilder.Entity("Hv.Ppb302.DigitalThesis.WebClient.Models.ConnectorTag", b =>
                {
                    b.Navigation("GeoTags");
                });
#pragma warning restore 612, 618
        }
    }
}
