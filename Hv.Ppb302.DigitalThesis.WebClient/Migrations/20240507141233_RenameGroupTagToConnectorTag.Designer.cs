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
    [Migration("20240507141233_RenameGroupTagToConnectorTag")]
    partial class RenameGroupTagToConnectorTag
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ConnectorTagGeoTag", b =>
                {
                    b.Property<Guid>("ConnectorTagsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GeoTagsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ConnectorTagsId", "GeoTagsId");

                    b.HasIndex("GeoTagsId");

                    b.ToTable("ConnectorTagGeoTag");
                });

            modelBuilder.Entity("ConnectorTagKaleidoscopeMosaic", b =>
                {
                    b.Property<Guid>("ConnectorTagsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("KaleidoscopeMosaicsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ConnectorTagsId", "KaleidoscopeMosaicsId");

                    b.HasIndex("KaleidoscopeMosaicsId");

                    b.ToTable("ConnectorTagKaleidoscopeMosaic");
                });

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

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("HasAudio")
                        .HasColumnType("bit");

                    b.Property<string>("PdfFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GeoTags");
                });

            modelBuilder.Entity("Hv.Ppb302.DigitalThesis.WebClient.Models.KaleidoscopeMosaic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AudioFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("HasAudio")
                        .HasColumnType("bit");

                    b.Property<string>("PdfFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("KaleidoscopeMosaics");
                });

            modelBuilder.Entity("Hv.Ppb302.DigitalThesis.WebClient.Models.MolarMosaic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AudioFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("HasAudio")
                        .HasColumnType("bit");

                    b.Property<string>("PdfFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MolarMosaics");
                });

            modelBuilder.Entity("Hv.Ppb302.DigitalThesis.WebClient.Models.MolecularMosaic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AudioFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("HasAudio")
                        .HasColumnType("bit");

                    b.Property<string>("PdfFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MolecularMosaics");
                });

            modelBuilder.Entity("ConnectorTagGeoTag", b =>
                {
                    b.HasOne("Hv.Ppb302.DigitalThesis.WebClient.Models.ConnectorTag", null)
                        .WithMany()
                        .HasForeignKey("ConnectorTagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hv.Ppb302.DigitalThesis.WebClient.Models.GeoTag", null)
                        .WithMany()
                        .HasForeignKey("GeoTagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ConnectorTagKaleidoscopeMosaic", b =>
                {
                    b.HasOne("Hv.Ppb302.DigitalThesis.WebClient.Models.ConnectorTag", null)
                        .WithMany()
                        .HasForeignKey("ConnectorTagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hv.Ppb302.DigitalThesis.WebClient.Models.KaleidoscopeMosaic", null)
                        .WithMany()
                        .HasForeignKey("KaleidoscopeMosaicsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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
#pragma warning restore 612, 618
        }
    }
}