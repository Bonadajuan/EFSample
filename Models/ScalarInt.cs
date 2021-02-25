namespace EFSample.Models 
{
    public partial class ScalarInt 
    {
        public int Value { get; set; }
        
        public override string ToString() 
        {
            return Value.ToString();
        }
    }
}