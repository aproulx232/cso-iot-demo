namespace cso_iot_demo
{
    public class RepairItem
    {
        public int RepairOrderId { get; set; }
        public int RepairItemId { get; set; }
        public string CompleteFlag { get; set; }
        public string PsiSource { get; set; }
        public int InspActionId { get; set; }
        public string PsiResult { get; set; }
        public int ShopTypeId { get; set; }
    }
}
