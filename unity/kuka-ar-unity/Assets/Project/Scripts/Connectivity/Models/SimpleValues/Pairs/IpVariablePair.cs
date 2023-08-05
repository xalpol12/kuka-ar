namespace Connectivity.Models.SimpleValues.Pairs
{
    public struct IpVariablePair
    {
        public string Host { get; set; }
        public string Var { get; set; }

        public IpVariablePair(string host, string var)
        {
            Host = host;
            Var = var;
        }
    }
}
