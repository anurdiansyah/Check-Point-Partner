namespace RD.Lib.Models
{
    public partial class SysParam
    {
        public string Code { get; set; }
        public int IndexOrder { get; set; }
        public int SysParamGroupId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool IsEncrypted { get; set; }
        public bool IsVisible { get; set; }
        public bool IsNeedApproval { get; set; }
    }
}
