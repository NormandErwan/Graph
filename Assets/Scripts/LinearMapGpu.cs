namespace Graph
{
    public struct LinearMapGpu
    {
        public LinearMapGpu(Map map)
        {
            Coefficient = map.Image.Distance / map.Domain.Distance;
            DomainMin = map.Domain.Min;
            ImageMin = map.Image.Min;
        }

        public float Coefficient { get; }

        public float DomainMin { get; }

        public float ImageMin { get; }

        public static implicit operator LinearMapGpu(Map map)
        {
            return new LinearMapGpu(map);
        }
    }
}
