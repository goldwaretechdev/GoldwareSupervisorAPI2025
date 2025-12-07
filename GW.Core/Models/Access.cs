
namespace GW.Core.Models
{
    public class Access
    {
        public int Id { get; set; }
        
        public User User { get; set; }
        public Guid FkUserId {  get; set; }

        public Unit Unit { get; set; }
        public int FkUnitId { get; set; }
    }
}
