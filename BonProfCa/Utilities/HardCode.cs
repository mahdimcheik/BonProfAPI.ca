namespace BonProfCa.Utilities;

public static class HardCode
{
    // Roles
    public static Guid ROLE_SUPER_ADMIN => Guid.Parse("bde5556b-562d-431f-9ff9-d31a5f5cb8c5");
    public static Guid ROLE_ADMIN => Guid.Parse("4a5eaf2f-0496-4035-a4b7-9210da39501c");
    public static Guid ROLE_TEACHER => Guid.Parse("87a0a5ed-c7bb-4394-a163-7ed7560b3703");
    public static Guid ROLE_STUDENT => Guid.Parse("87a0a5ed-c7bb-4394-a163-7ed7560b4a01");

    // Genders
    public static Guid GENDER_MALE => Guid.Parse("DCB8B01B-205A-4EA2-A281-004A7B1BB972");
    public static Guid GENDER_FEMALE => Guid.Parse("B68C151B-DB34-462D-A65C-90989CC96E5E");
    public static Guid GENDER_OTHER => Guid.Parse("B07B2445-F39C-4B26-8FFE-E40FE561D8BC");

    // AccountStatuses
    public static Guid ACCOUNT_ACTIVE => Guid.Parse("12e65875-bec4-422a-aad4-c28de0e06fff");
    public static Guid ACCOUNT_PENDING => Guid.Parse("acc9ddb6-824b-42cb-8276-5d91b6af2003");
    public static Guid ACCOUNT_BANNED => Guid.Parse("fd2621ef-85f0-46a6-acdd-c77fb111630f");

    // Reservation Status
    public static Guid RESERVATION_PENDING => Guid.Parse("2EC60A91-AAB8-4753-A5D8-B131B9441E77");
    public static Guid RESERVATION_ACCEPTED => Guid.Parse("32D854B6-6D4E-445A-9209-31A492970F2D");
    public static Guid RESERVATION_REJECTED => Guid.Parse("CAAB85F5-D37B-4EA0-B035-5BA3CA8DD49F");
    public static Guid RESERVATION_DONE => Guid.Parse("6D281AEC-D093-4071-8BF4-C8363361B5B4");
    public static Guid RESERVATION_CANCELLED => Guid.Parse("C5420623-8B51-4CE9-9EC1-3BC546B9FFFF");

    // Levels
    public static Guid LEVEL_BEGINNER => Guid.Parse("eb4bd576-6855-4123-bb92-e921c8610542");
    public static Guid LEVEL_INTERMEDIATE => Guid.Parse("19702cef-0a3b-46f8-932b-cf78634e741d");
    public static Guid LEVEL_ADVANCED => Guid.Parse("a518f61e-8a19-44e6-b365-fb285ad0811e");
    public static Guid LEVEL_ALL => Guid.Parse("b70134eb-060c-4069-a325-1251c75a1ac9");

    // Categories
    public static Guid CATEGORY_SOFT => Guid.Parse("86969bd8-e51f-4558-ac44-fc159ed31c53");
    public static Guid CATEGORY_TECHNICS => Guid.Parse("a7f8b05d-3d2d-43fa-870d-987c21f4e41d");
    public static Guid CATEGORY_FRONT => Guid.Parse("0b6865c4-76fc-4caa-8171-07f449ca6e5c");
    public static Guid CATEGORY_BACK => Guid.Parse("4133e2f8-f0e0-44f8-8cfb-8a8e1aaa86d7");

    // Status Transaction
    public static Guid STATUS_TRANSACTION_PAID =>
        Guid.Parse("73f7fa42-196b-4040-b727-64a7b1e56458");
    public static Guid STATUS_TRANSACTION_FAILED =>
        Guid.Parse("584f233a-bf58-4db9-a24e-90baac3f6d42");
    public static Guid STATUS_TRANSACTION_PENDING =>
        Guid.Parse("ead0ecc1-a58d-4436-a87f-89c2dbc665a8");

    // Type Teacher Transactions
    public static Guid TYPE_TEACHER_TRANSACTION_PAYMENT =>
        Guid.Parse("7305a3d5-bfa9-41ce-be10-174c406cb842");
    public static Guid TYPE_TEACHER_TRANSACTION_PAYOUT =>
        Guid.Parse("27aec3d1-dd41-4728-a29e-473da46779d9");
    public static Guid TYPE_TEACHER_TRANSACTION_REFUND =>
        Guid.Parse("50412518-6c82-40c1-b6bf-b9c7aadf67d1");

    // Type addresses
    public static Guid TYPE_ADDRESS_MAIN => Guid.Parse("e1fee3ea-6190-48c3-8e40-c1f053fea79d");
    public static Guid TYPE_ADDRESS_BILLING => Guid.Parse("b8b8a8fc-ca60-440b-815f-1e44b89c9803");

    // Type slots
    public static Guid TYPE_SLOT_PRESENTIAL => Guid.Parse("79f538c3-5f2b-4e45-a5f8-4d7cda8b3df8");
    public static Guid TYPE_SLOT_VISIO => Guid.Parse("4043e32b-4d92-49b5-b885-505155ff2fe9");
    public static Guid TYPE_SLOT_ALL => Guid.Parse("c25c18a2-af88-4132-a27a-0025417edb56");

    // langues
    public static Guid LANGUAGE_FRENCH => Guid.Parse("52b54b82-1f37-4a66-a263-708b53cd685d");
    public static Guid LANGUAGE_ENGLISH => Guid.Parse("3aa916ed-53d2-4f93-80e9-b49171a7ebe1");
    public static Guid LANGUAGE_ARAB => Guid.Parse("ff34f5ba-6201-45bf-9217-dcda019976a3");

    // Status Orders
    public static Guid STATUS_ORDER_PENDING => Guid.Parse("4470502b-fce9-4d5f-913c-3340af86b532");
    public static Guid STATUS_ORDER_PAID => Guid.Parse("36fe7f5e-61f3-4ce1-88bb-8942d3b094a1");
    public static Guid STATUS_ORDER_REFUNDED => Guid.Parse("8b84e43c-d208-4c18-9c23-2b13438eb109");
    public static Guid STATUS_ORDER_REFUNDED_PARTIALY =>
        Guid.Parse("2a6822ed-9116-47b7-a225-be832db4beb8");

    // docummment types
    public static Guid DOCUMENT_ID => Guid.Parse("3817d1eb-594e-40ba-8544-959121aaa179");
    public static Guid DOCUMENT_DIPLOME => Guid.Parse("d918182e-d9d7-452c-9607-ab83f78139d0");
    public static Guid DOCUMENT_SIRET => Guid.Parse("8bcfa1b8-70bb-4e9c-a1d1-e1c64230a579");
    public static Guid DOCUMENT_OTHER => Guid.Parse("f709ecfd-82f3-4d07-851f-419e9016135f");
}

public enum StatusReservationCode
{
    Pending,
    Accepted,
    Rejected,
    Done,
    Cancelled,
}

public enum RoleEnum
{
    Student,
    Teacher,
    Admin,
    SuperAdmin,
}

public enum GenderEnum
{
    Male,
    Female,
    Other,
}

public enum AccountStatusEnum
{
    Active,
    Pending,
    Banned,
}

public enum AddressTypeEnum
{
    Main,
    Billing,
}

public enum TransactionStatusEnum
{
    Pending,
    Paid,
    Failed,
}

public enum TeacherTransactionTypeEnum
{
    Payment,
    Refund,
}
public enum OrderStatusEnum
{
    Pending,
    Paid,
    FullRefund,
    PartialRefund,
}

public enum CursusLevelEnum
{
    All,
    Beginner,
    Intermediate,
    Advanced,
}

public enum LanguageEnum
{
    French,
    English,
    Arab,
}

public enum CursusCategoryEnum
{
    Soft,
    Technics,
    Front,
    Back,
}

public enum SlotTypeEnum
{
    Presential,
    Visio,
    All,
}

public enum DocumentTypeEnum
{
    Identification,
    Diploma,
    CompanyRegistration,
    Other,
}

public enum SignalRNotificationTypeEnum
{
    Message,
    Ping,
    Notification,
    Chat,
    Writing
}

public enum NotificationTypeEnum
{
    Account,
    Reservation,
    Payment,
    Chat,
    Product
}

public enum MessageTypeEnum
{
    Notification,
    Email,
    Chat,
    Ping
}
