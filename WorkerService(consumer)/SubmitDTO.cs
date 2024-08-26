namespace streamer
{
    public partial class Worker
    {
        public class SubmitDTO
        {
            public Guid Clordid { get; set; }
            public string Username { get; set; }
            public int Qty { get; set; }
            public decimal Px { get; set; }
            public string Dir { get; set; }
        }
    }
}


