﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Persistence.Framework;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    [DbContext(typeof(ShopwayDbContext))]
    [Migration("20240520065227_SetSparseColumns")]
    partial class SetSparseColumns
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Shopway.Domain.Orders.OrderHeader", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("Char(26)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<bool>("SoftDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("Bit")
                        .HasDefaultValue(false);

                    b.Property<DateTimeOffset?>("SoftDeletedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("VarChar(10)");

                    b.Property<decimal>("TotalDiscount")
                        .HasPrecision(3, 2)
                        .HasColumnType("decimal(3,2)")
                        .HasColumnName("Discount");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .IsConcurrencyToken()
                        .HasColumnType("DateTimeOffset(7)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("Char(26)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("OrderHeader", "Shopway");
                });

            modelBuilder.Entity("Shopway.Domain.Orders.OrderLine", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("Char(26)");

                    b.Property<int>("Amount")
                        .HasColumnType("int")
                        .HasColumnName("Amount");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<decimal>("LineDiscount")
                        .HasPrecision(3, 2)
                        .HasColumnType("decimal(3,2)")
                        .HasColumnName("Discount");

                    b.Property<string>("OrderHeaderId")
                        .IsRequired()
                        .HasColumnType("Char(26)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .IsConcurrencyToken()
                        .HasColumnType("DateTimeOffset(7)");

                    b.HasKey("Id");

                    b.HasIndex("OrderHeaderId");

                    b.ToTable("OrderLine", "Shopway");
                });

            modelBuilder.Entity("Shopway.Domain.Orders.Payment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("Char(26)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<bool>("IsRefunded")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("Bit")
                        .HasDefaultValue(false);

                    b.Property<string>("OrderHeaderId")
                        .IsRequired()
                        .HasColumnType("Char(26)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("VarChar(11)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .IsConcurrencyToken()
                        .HasColumnType("DateTimeOffset(7)");

                    b.HasKey("Id");

                    b.HasIndex("OrderHeaderId");

                    b.ToTable("Payment", "Shopway");
                });

            modelBuilder.Entity("Shopway.Domain.Products.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("Char(26)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<decimal>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("Price");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("ProductName");

                    b.Property<string>("Revision")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("Revision");

                    b.Property<string>("UomCode")
                        .IsRequired()
                        .HasColumnType("VarChar(8)")
                        .HasColumnName("UomCode");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .IsConcurrencyToken()
                        .HasColumnType("DateTimeOffset(7)");

                    b.HasKey("Id");

                    b.HasIndex("ProductName", "Revision")
                        .IsUnique()
                        .HasDatabaseName("UX_Product_ProductName_Revision");

                    b.ToTable("Product", "Shopway");
                });

            modelBuilder.Entity("Shopway.Domain.Products.Review", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("Char(26)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)")
                        .HasColumnName("Description");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("Char(26)");

                    b.Property<decimal>("Stars")
                        .HasColumnType("TinyInt")
                        .HasColumnName("Stars");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("nvarchar(45)")
                        .HasColumnName("Title");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .IsConcurrencyToken()
                        .HasColumnType("DateTimeOffset(7)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("Username");

                    b.HasKey("Id");

                    b.HasIndex("ProductId", "Title")
                        .IsUnique()
                        .HasDatabaseName("UX_Review_ProductId_Title");

                    b.ToTable("Review", "Shopway");
                });

            modelBuilder.Entity("Shopway.Domain.Users.Customer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("Char(26)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<DateTimeOffset?>("DateOfBirth")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("FirstName");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("VarChar(6)")
                        .HasColumnName("Gender");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("LastName");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)")
                        .HasColumnName("PhoneNumber");

                    b.Property<string>("Rank")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("VarChar(8)")
                        .HasDefaultValue("Standard")
                        .HasColumnName("Rank");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .IsConcurrencyToken()
                        .HasColumnType("DateTimeOffset(7)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("Char(26)");

                    b.HasKey("Id");

                    b.ToTable("Customer", "Master");
                });

            modelBuilder.Entity("Shopway.Domain.Users.Enumerations.Permission", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TinyInt");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<byte>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VarChar(128)");

                    b.HasKey("Id");

                    b.ToTable("Permission", "Master");

                    b.HasData(
                        new
                        {
                            Id = (byte)1,
                            Name = "Review_Read"
                        },
                        new
                        {
                            Id = (byte)2,
                            Name = "Review_Add"
                        },
                        new
                        {
                            Id = (byte)3,
                            Name = "Review_Update"
                        },
                        new
                        {
                            Id = (byte)4,
                            Name = "Review_Remove"
                        },
                        new
                        {
                            Id = (byte)5,
                            Name = "INVALID_PERMISSION"
                        });
                });

            modelBuilder.Entity("Shopway.Domain.Users.Enumerations.Role", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TinyInt");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<byte>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VarChar(128)");

                    b.HasKey("Id");

                    b.ToTable("Role", "Master");

                    b.HasData(
                        new
                        {
                            Id = (byte)1,
                            Name = "Customer"
                        },
                        new
                        {
                            Id = (byte)2,
                            Name = "Employee"
                        },
                        new
                        {
                            Id = (byte)3,
                            Name = "Manager"
                        },
                        new
                        {
                            Id = (byte)4,
                            Name = "Administrator"
                        });
                });

            modelBuilder.Entity("Shopway.Domain.Users.Enumerations.RolePermission", b =>
                {
                    b.Property<byte>("RoleId")
                        .HasColumnType("TinyInt");

                    b.Property<byte>("PermissionId")
                        .HasColumnType("TinyInt");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermission", "Master");

                    b.HasData(
                        new
                        {
                            RoleId = (byte)4,
                            PermissionId = (byte)1
                        },
                        new
                        {
                            RoleId = (byte)4,
                            PermissionId = (byte)2
                        },
                        new
                        {
                            RoleId = (byte)4,
                            PermissionId = (byte)3
                        },
                        new
                        {
                            RoleId = (byte)4,
                            PermissionId = (byte)4
                        },
                        new
                        {
                            RoleId = (byte)3,
                            PermissionId = (byte)1
                        },
                        new
                        {
                            RoleId = (byte)3,
                            PermissionId = (byte)2
                        },
                        new
                        {
                            RoleId = (byte)3,
                            PermissionId = (byte)3
                        },
                        new
                        {
                            RoleId = (byte)2,
                            PermissionId = (byte)1
                        },
                        new
                        {
                            RoleId = (byte)1,
                            PermissionId = (byte)1
                        },
                        new
                        {
                            RoleId = (byte)1,
                            PermissionId = (byte)2
                        },
                        new
                        {
                            RoleId = (byte)1,
                            PermissionId = (byte)3
                        });
                });

            modelBuilder.Entity("Shopway.Domain.Users.RoleUser", b =>
                {
                    b.Property<byte>("RoleId")
                        .HasColumnType("TinyInt");

                    b.Property<string>("UserId")
                        .HasColumnType("Char(26)");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("RoleUser", "Master");
                });

            modelBuilder.Entity("Shopway.Domain.Users.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("Char(26)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<string>("CustomerId")
                        .HasColumnType("Char(26)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasColumnName("Email");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("NChar(514)")
                        .HasColumnName("PasswordHash");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("VarChar(32)")
                        .HasColumnName("RefreshToken");

                    b.Property<DateTimeOffset?>("TwoFactorTokenCreatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<string>("TwoFactorTokenHash")
                        .HasColumnType("NChar(514)")
                        .HasColumnName("TwoFactorTokenHash");

                    b.Property<string>("TwoFactorToptSecret")
                        .HasColumnType("Char(32)")
                        .HasColumnName("TwoFactorToptSecret");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .IsConcurrencyToken()
                        .HasColumnType("DateTimeOffset(7)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("Username");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique()
                        .HasFilter("[CustomerId] IS NOT NULL");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("UX_User_Email");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasDatabaseName("UX_Username_Email");

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex("Username"), new[] { "Email" });

                    b.ToTable("User", "Master");
                });

            modelBuilder.Entity("Shopway.Infrastructure.Outbox.OutboxMessage", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("Char(26)");

                    b.Property<byte>("AttemptCount")
                        .HasColumnType("TinyInt");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("VarChar(5000)");

                    b.Property<string>("Error")
                        .HasColumnType("VarChar(8000)");

                    SqlServerPropertyBuilderExtensions.IsSparse(b.Property<string>("Error"));

                    b.Property<string>("ExecutionStatus")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("VarChar(10)")
                        .HasDefaultValue("InProgress");

                    b.Property<DateTimeOffset?>("NextProcessAttempt")
                        .HasColumnType("DateTimeOffset(2)");

                    SqlServerPropertyBuilderExtensions.IsSparse(b.Property<DateTimeOffset?>("NextProcessAttempt"));

                    b.Property<DateTimeOffset>("OccurredOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<DateTimeOffset?>("ProcessedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("VarChar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ExecutionStatus")
                        .HasDatabaseName("IX_OutboxMessage_ExecutionStatus")
                        .HasFilter("[ExecutionStatus] = 'InProgress'");

                    b.ToTable("OutboxMessage", "Outbox");
                });

            modelBuilder.Entity("Shopway.Infrastructure.Outbox.OutboxMessageConsumer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("Char(26)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id", "Name");

                    b.ToTable("OutboxMessageConsumer", "Outbox");
                });

            modelBuilder.Entity("Shopway.Domain.Orders.OrderHeader", b =>
                {
                    b.HasOne("Shopway.Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shopway.Domain.Orders.OrderLine", b =>
                {
                    b.HasOne("Shopway.Domain.Orders.OrderHeader", null)
                        .WithMany("OrderLines")
                        .HasForeignKey("OrderHeaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Shopway.Domain.Orders.ValueObjects.ProductSummary", "ProductSummary", b1 =>
                        {
                            b1.Property<string>("OrderLineId")
                                .HasColumnType("Char(26)");

                            b1.Property<decimal>("Price")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<string>("ProductId")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("ProductName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Revision")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("UomCode")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("OrderLineId");

                            b1.ToTable("OrderLine", "Shopway");

                            b1.ToJson("ProductSummary");

                            b1.WithOwner()
                                .HasForeignKey("OrderLineId");
                        });

                    b.Navigation("ProductSummary")
                        .IsRequired();
                });

            modelBuilder.Entity("Shopway.Domain.Orders.Payment", b =>
                {
                    b.HasOne("Shopway.Domain.Orders.OrderHeader", null)
                        .WithMany("Payments")
                        .HasForeignKey("OrderHeaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Shopway.Domain.Orders.ValueObjects.Session", "Session", b1 =>
                        {
                            b1.Property<string>("PaymentId")
                                .HasColumnType("Char(26)");

                            b1.Property<string>("Id")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PaymentIntentId")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Secret")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("PaymentId");

                            b1.ToTable("Payment", "Shopway");

                            b1.ToJson("Session");

                            b1.WithOwner()
                                .HasForeignKey("PaymentId");
                        });

                    b.Navigation("Session");
                });

            modelBuilder.Entity("Shopway.Domain.Products.Review", b =>
                {
                    b.HasOne("Shopway.Domain.Products.Product", null)
                        .WithMany("Reviews")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shopway.Domain.Users.Customer", b =>
                {
                    b.OwnsOne("Shopway.Domain.Users.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<string>("Id")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<int>("Building")
                                .HasMaxLength(1000)
                                .HasColumnType("int");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("CustomerId")
                                .IsRequired()
                                .HasColumnType("Char(26)");

                            b1.Property<int?>("Flat")
                                .HasMaxLength(1000)
                                .HasColumnType("int");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasMaxLength(5)
                                .HasColumnType("nvarchar(5)");

                            b1.HasKey("Id");

                            b1.HasIndex("CustomerId")
                                .IsUnique();

                            b1.ToTable("Address", "Master");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Shopway.Domain.Users.Enumerations.RolePermission", b =>
                {
                    b.HasOne("Shopway.Domain.Users.Enumerations.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shopway.Domain.Users.Enumerations.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shopway.Domain.Users.RoleUser", b =>
                {
                    b.HasOne("Shopway.Domain.Users.Enumerations.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shopway.Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shopway.Domain.Users.User", b =>
                {
                    b.HasOne("Shopway.Domain.Users.Customer", "Customer")
                        .WithOne("User")
                        .HasForeignKey("Shopway.Domain.Users.User", "CustomerId");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Shopway.Domain.Orders.OrderHeader", b =>
                {
                    b.Navigation("OrderLines");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("Shopway.Domain.Products.Product", b =>
                {
                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("Shopway.Domain.Users.Customer", b =>
                {
                    b.Navigation("User")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
