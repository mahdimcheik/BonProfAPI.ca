using System.Diagnostics.Contracts;
using System.Reflection.Emit;
using BonProfCa.Models;
using BonProfCa.Models;
using BonProfCa.Models.Files;
using BonProfCa.Utilities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BonProfCa.Contexts;

public class MainContext : IdentityDbContext<UserApp, RoleApp, Guid>
{
    public DbSet<UserApp> Users { get; set; }
    public DbSet<RoleApp> Roles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Gender> Genders { get; set; }

    // Profile entities
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Student> Students { get; set; }

    // public DbSet<Profile> Profiles { get; set; }

    // Related entities
    public DbSet<Address> Addresses { get; set; }
    public DbSet<TypeAddress> TypeAddresses { get; set; }
    public DbSet<Cursus> Cursuses { get; set; }
    public DbSet<Experience> Experiences { get; set; }
    public DbSet<Formation> Formations { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<CategoryCursus> CategoryCursuses { get; set; }
    public DbSet<LevelCursus> LevelCursuses { get; set; }

    // reservations
    public DbSet<Slot> Slots { get; set; }
    public DbSet<TypeSlot> TypeSlots { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<StatusOrder> StatusOrders { get; set; }
    public DbSet<StatusReservation> StatusReservations { get; set; }
    
    // conversations
    public DbSet<Conversation> Conversations { get; set; }

    // payments and transactions
    public DbSet<Payment> Payments { get; set; } // paiement d'une commande
    public DbSet<TeacherPayout> TeacherPayouts { get; set; } // une ligne par professeur (les comptes des profs)
    public DbSet<TeacherWalletTransaction> TeacherWalletTransactions { get; set; } // historique des transactions du portefeuille, acomptes, retraits, remboursements
    public DbSet<TypeTeacherTransaction> TypeTransactions { get; set; } // types de transactions (acompte, retrait, remboursement)
    public DbSet<StatusTransaction> StatusTransactions { get; set; } // statuts des transactions (en attente, complété, échoué)
    public DbSet<PaymentMethod> PaymentMethods { get; set; } // méthodes de paiement (carte bancaire, PayPal, etc.)

    // document & type
    public DbSet<Document> Documents { get; set; }
    public DbSet<PrivacyDocument> PrivacyDocuments { get; set; }
    public DbSet<PrivacyDocumentType> PrivacyDocumentTypes { get; set; }

    // notifications
    public DbSet<Notification> Notifications { get; set; }

    public MainContext(DbContextOptions options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserApp>().ToTable("Users");
        builder.Entity<RoleApp>().ToTable("Roles");

        builder.Entity<Document>().ToTable("Documents");
        builder.Entity<PrivacyDocument>().ToTable("PrivacyDocuments");

        // Global query filters (must stay in Fluent API)
        builder.Entity<Cursus>().HasQueryFilter(d => d.ArchivedAt == null);
        builder.Entity<Formation>().HasQueryFilter(d => d.ArchivedAt == null);
        builder.Entity<Address>().HasQueryFilter(d => d.ArchivedAt == null);
        builder.Entity<Product>().HasQueryFilter(d => d.ArchivedAt == null);
        builder.Entity<Slot>().HasQueryFilter(d => d.ArchivedAt == null);

        // Seed Roles
        List<RoleApp> roles = new()
        {
            new RoleApp
            {
                Id = HardCode.ROLE_SUPER_ADMIN,
                Name = nameof(RoleEnum.SuperAdmin),
                NormalizedName = nameof(RoleEnum.SuperAdmin).ToUpper(),
                Color = "#3d82f2",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ConcurrencyStamp = "SUPERADMIN-STAMP-2025",
            },
            new RoleApp
            {
                Id = HardCode.ROLE_ADMIN,
                Name = nameof(RoleEnum.Admin),
                Color = "#31bdd6",
                NormalizedName = nameof(RoleEnum.Admin).ToUpper(),
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ConcurrencyStamp = "ADMIN-STAMP-2025",
            },
            new RoleApp
            {
                Id = HardCode.ROLE_TEACHER,
                Name = nameof(RoleEnum.Teacher),
                Color = "#31d68f",
                NormalizedName = nameof(Teacher).ToUpper(),
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ConcurrencyStamp = "TEACHER-STAMP-2025",
            },
            new RoleApp
            {
                Id = HardCode.ROLE_STUDENT,
                Name = nameof(RoleEnum.Student),
                NormalizedName = nameof(RoleEnum.Student).ToUpper(),
                Color = "#f57ad0",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ConcurrencyStamp = "STUDENT-STAMP-2025",
            },
        };
        builder.Entity<RoleApp>().HasData(roles);
        // Seed Genders
        List<Gender> genders = new()
        {
            new Gender
            {
                Id = HardCode.GENDER_FEMALE,
                Name = nameof(GenderEnum.Female),
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new Gender
            {
                Id = HardCode.GENDER_MALE,
                Name = nameof(GenderEnum.Male),
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new Gender
            {
                Id = HardCode.GENDER_OTHER,
                Name = nameof(GenderEnum.Other),
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };

        builder.Entity<Gender>().HasData(genders);

        // Seed Account status
        List<StatusAccount> accountStatuses = new()
        {
            new StatusAccount
            {
                Id = HardCode.ACCOUNT_ACTIVE,
                Name = nameof(AccountStatusEnum.Active),
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new StatusAccount
            {
                Id = HardCode.ACCOUNT_PENDING,
                Name = nameof(AccountStatusEnum.Pending),
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new StatusAccount
            {
                Id = HardCode.ACCOUNT_BANNED,
                Name = nameof(AccountStatusEnum.Banned),
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };
        builder.Entity<StatusAccount>().HasData(accountStatuses);

        // Seed type address
        List<TypeAddress> typeAddresses = new()
        {
            new TypeAddress
            {
                Id = HardCode.TYPE_ADDRESS_MAIN,
                Name = nameof(AddressTypeEnum.Main),
                DisplayName = "Principale",
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new TypeAddress
            {
                Id = HardCode.TYPE_ADDRESS_BILLING,
                Name = nameof(AddressTypeEnum.Billing),
                DisplayName = "Facturation",
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };
        builder.Entity<TypeAddress>().HasData(typeAddresses);

        // Seed Transaction Status
        List<StatusTransaction> statusTransactions = new()
        {
            new StatusTransaction
            {
                Id = HardCode.STATUS_TRANSACTION_PENDING,
                Name = nameof(TransactionStatusEnum.Pending),
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new StatusTransaction
            {
                Id = HardCode.STATUS_TRANSACTION_PAID,
                Name = nameof(TransactionStatusEnum.Paid),
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new StatusTransaction
            {
                Id = HardCode.STATUS_TRANSACTION_FAILED,
                Name = nameof(TransactionStatusEnum.Failed),
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };

        builder.Entity<StatusTransaction>().HasData(statusTransactions);

        // Seed Transaction Status
        List<TypeTeacherTransaction> typeTeacherTransactions = new()
        {
            new TypeTeacherTransaction
            {
                Id = HardCode.TYPE_TEACHER_TRANSACTION_PAYMENT,
                Name = nameof(TeacherTransactionTypeEnum.Payment),
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new TypeTeacherTransaction
            {
                Id = HardCode.TYPE_TEACHER_TRANSACTION_REFUND,
                Name = nameof(TeacherTransactionTypeEnum.Refund),
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };

        builder.Entity<TypeTeacherTransaction>().HasData(typeTeacherTransactions);

        // seed reservations status
        List<StatusReservation> statusReservations = new()
        {
            new StatusReservation
            {
                Id = HardCode.RESERVATION_PENDING,
                Name = nameof(StatusReservationCode.Pending),
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Code = StatusReservationCode.Pending,
            },
            new StatusReservation
            {
                Id = HardCode.RESERVATION_ACCEPTED,
                Name = nameof(StatusReservationCode.Accepted),
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Code = StatusReservationCode.Accepted,
            },
            new StatusReservation
            {
                Id = HardCode.RESERVATION_DONE,
                Name = nameof(StatusReservationCode.Done),
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Code = StatusReservationCode.Done,
            },
            new StatusReservation
            {
                Id = HardCode.RESERVATION_REJECTED,
                Name = nameof(StatusReservationCode.Rejected),
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Code = StatusReservationCode.Rejected,
            },
            new StatusReservation
            {
                Id = HardCode.RESERVATION_CANCELLED,
                Name = nameof(StatusReservationCode.Cancelled),
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Code = StatusReservationCode.Cancelled,
            },
        };
        builder.Entity<StatusReservation>().HasData(statusReservations);

        // seed reservations status
        List<StatusOrder> statusOrders = new()
        {
            new StatusOrder
            {
                Id = HardCode.STATUS_ORDER_PENDING,
                Name = nameof(OrderStatusEnum.Pending),
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new StatusOrder
            {
                Id = HardCode.STATUS_ORDER_PAID,
                Name = nameof(OrderStatusEnum.Paid),
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new StatusOrder
            {
                Id = HardCode.STATUS_ORDER_REFUNDED,
                Name = nameof(OrderStatusEnum.FullRefund),
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new StatusOrder
            {
                Id = HardCode.STATUS_ORDER_REFUNDED_PARTIALY,
                Name = nameof(OrderStatusEnum.PartialRefund),
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };
        builder.Entity<StatusOrder>().HasData(statusOrders);

        // seed languages
        List<Language> languages = new()
        {
            new Language
            {
                Id = HardCode.LANGUAGE_ARAB,
                Name = nameof(LanguageEnum.Arab),
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new Language
            {
                Id = HardCode.LANGUAGE_FRENCH,
                Name = nameof(LanguageEnum.French),
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new Language
            {
                Id = HardCode.LANGUAGE_ENGLISH,
                Name = nameof(LanguageEnum.English),
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };

        builder.Entity<Language>().HasData(languages);

        // courses type + category + level seeding can be added here similarly

        List<LevelCursus> levelCursuses = new()
        {
            new LevelCursus
            {
                Id = HardCode.LEVEL_ALL,
                Name = nameof(CursusLevelEnum.All),
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new LevelCursus
            {
                Id = HardCode.LEVEL_BEGINNER,
                Name = nameof(CursusLevelEnum.Beginner),
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new LevelCursus
            {
                Id = HardCode.LEVEL_INTERMEDIATE,
                Name = nameof(CursusLevelEnum.Intermediate),
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new LevelCursus
            {
                Id = HardCode.LEVEL_ADVANCED,
                Name = nameof(CursusLevelEnum.Advanced),
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };
        builder.Entity<LevelCursus>().HasData(levelCursuses);

        List<CategoryCursus> categoryCursuses = new()
        {
            new CategoryCursus
            {
                Id = HardCode.CATEGORY_BACK,
                Name = nameof(CursusCategoryEnum.Back),
                Color = "#ff69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new CategoryCursus
            {
                Id = HardCode.CATEGORY_FRONT,
                Name = nameof(CursusCategoryEnum.Front),
                Color = "#fa69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new CategoryCursus
            {
                Id = HardCode.CATEGORY_TECHNICS,
                Name = nameof(CursusCategoryEnum.Technics),
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new CategoryCursus
            {
                Id = HardCode.CATEGORY_SOFT,
                Name = nameof(CursusCategoryEnum.Soft),
                Color = "#ab69b4",
                Icon = "",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };
        builder.Entity<CategoryCursus>().HasData(categoryCursuses);

        // type slots
        List<TypeSlot> typeSlots = new()
        {
            new TypeSlot
            {
                Id = HardCode.TYPE_SLOT_PRESENTIAL,
                Name = nameof(SlotTypeEnum.Presential),
                Color = "#ff69b4",
                Icon = "pi pi-arrow-down-left-and-arrow-up-right-to-center",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new TypeSlot
            {
                Id = HardCode.TYPE_SLOT_VISIO,
                Name = nameof(SlotTypeEnum.Visio),
                Color = "#aa69b4",
                Icon = "pi pi-desktop",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new TypeSlot
            {
                Id = HardCode.TYPE_SLOT_ALL,
                Name = nameof(SlotTypeEnum.All),
                Color = "#1169b4",
                Icon = "pi pi-crown",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };
        builder.Entity<TypeSlot>().HasData(typeSlots);

        // type slots
        List<PrivacyDocumentType> documentTypes = new()
        {
            new PrivacyDocumentType
            {
                Id = HardCode.DOCUMENT_SIRET,
                Name = nameof(DocumentTypeEnum.CompanyRegistration),
                DisplayName = "K-Bis",
                Color = "#ff69b4",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new PrivacyDocumentType
            {
                Id = HardCode.DOCUMENT_DIPLOME,
                Name = nameof(DocumentTypeEnum.Diploma),
                DisplayName = "Diplôme",
                Color = "#aa69b4",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new PrivacyDocumentType
            {
                Id = HardCode.DOCUMENT_ID,
                Name = nameof(DocumentTypeEnum.Identification),
                DisplayName = "Pièce d'identité",
                Color = "#1169b4",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new PrivacyDocumentType
            {
                Id = HardCode.DOCUMENT_OTHER,
                Name = nameof(DocumentTypeEnum.Other),
                DisplayName = "Autre",
                Color = "#1169b4",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
        };
        builder.Entity<PrivacyDocumentType>().HasData(documentTypes);

        // global filtrer
        builder.Entity<Cursus>().HasQueryFilter(d => d.ArchivedAt == null);
        builder.Entity<Formation>().HasQueryFilter(d => d.ArchivedAt == null);
        builder.Entity<Address>().HasQueryFilter(d => d.ArchivedAt == null);
        builder.Entity<Product>().HasQueryFilter(d => d.ArchivedAt == null);
        builder.Entity<Slot>().HasQueryFilter(d => d.ArchivedAt == null);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<CustomDateTimeConversion>();
        base.ConfigureConventions(configurationBuilder);
    }
}
