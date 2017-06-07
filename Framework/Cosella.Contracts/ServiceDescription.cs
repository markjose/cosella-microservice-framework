namespace Cosella.Contracts
{
    public class ServiceDescription
    {
        public string ServiceName { get; set; }
        public int Version { get; set; }
        public InstanceDetail[] Instances { get; set; }
        public dynamic Descriptor { get; set; }

        public class InstanceDetail
        {
            public string Health { get; set; }
            public string InstanceName { get; set; }
            public string NodeId { get; set; }
        }
    }
}