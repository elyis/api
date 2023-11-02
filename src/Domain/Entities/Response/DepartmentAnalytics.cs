namespace api.src.Domain.Entities.Response
{
    public class DepartmentAnalytics
    {
        public List<ActivityInfo> ActivityInfos { get; set; } = new();
        public List<UserInfo> UserInfos { get; set; }
    }

    public class ActivityInfo
    {
        public string ActivityName { get; set; }
        public float AverageCountPoint { get; set; }
        public float PaymentRatio { get; set; }
    }

    public class UserInfo
    {
        public string Fullname { get; set; }
        public List<UserActivityInfo> Activities { get; set; }
    }

    public class UserActivityInfo
    {
        public string ActivityName { get; set; }
        public float AverageCountPoints { get; set; }
        public float PointsByMonth { get; set; }
    }

    public class OrganizationAnalytics
    {
        public Dictionary<string, DepartmentAnalytics> DepartmentAnalytics { get; set; } = new();
        public List<ActivityInfo> ActivityInfos { get; set; } = new();
    }
}
