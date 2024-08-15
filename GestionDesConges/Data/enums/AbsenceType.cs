using System.Runtime.Serialization;

namespace GestionDesConges.Data.enums
{
    public enum AbsenceType
    {
        [EnumMember(Value = "Annual Leave")]
        AnnualLeave,
        [EnumMember(Value = "Sick Leave")]
        SickLeave,
        [EnumMember(Value = "Maternity Leave")]
        MaternityLeave,
        [EnumMember(Value = "Paternity Leave")]
        PaternityLeave,
        [EnumMember(Value = "Bereavement Leave")]
        BereavementLeave,
        [EnumMember(Value = "Unpaid Leave")]
        UnpaidLeave,
        [EnumMember(Value = "Compassionate Leave")]
        CompassionateLeave,
        [EnumMember(Value = "Study Leave")]
        StudyLeave,
        [EnumMember(Value = "Sabbatical")]
        Sabbatical,
        [EnumMember(Value = "Personal Leave")]
        PersonalLeave,
        [EnumMember(Value = "Jury Duty")]
        JuryDuty
    }

}
