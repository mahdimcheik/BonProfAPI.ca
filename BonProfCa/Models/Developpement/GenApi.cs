using BonProfCa.Models.Files;
using BonProfCa.Utilities;

namespace BonProfCa.Models.Developpement
{
    public class GenApi
    {
        public StatusReservationCode StatusReservationCode { get; set; }
        public RoleEnum RoleEnum { get; set; }
        public GenderEnum GenderEnum { get; set; }
        public AccountStatusEnum AccountStatusEnum { get; set; }
        public AddressTypeEnum AddressTypeEnum { get; set; }
        public TransactionStatusEnum TransactionStatusEnum { get; set; }
        public TeacherTransactionTypeEnum TeacherTransactionTypeEnum { get; set; }
        public OrderStatusEnum OrderStatusEnum { get; set; }
        public CursusLevelEnum CursusLevelEnum { get; set; }
        public LanguageEnum LanguageEnum { get; set; }
        public CursusCategoryEnum CursusCategoryEnum { get; set; }
        public SlotTypeEnum SlotTypeEnum { get; set; }
        public DocumentTypeEnum DocumentTypeEnum { get; set; }
        public SignalRNotificationTypeEnum SignalRNotificationTypeEnum { get; set; }
        public NotificationTypeEnum NotificationTypeEnum { get; set; }
        public ConversationCreate ConversationCreate { get; set; }

    }
}
