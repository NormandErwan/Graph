using System;
using UnityEngine;

namespace Graph
{
    [Serializable]
    public sealed class Map
    {
        [SerializeField]
        private Interval domain = Interval.Default;

        [SerializeField]
        private Interval image = Interval.Default;

        public Map(Interval domain, Interval image, Func<float, float> function)
        {
            Domain = domain;
            Function = function;
            Image = image;
        }

        public Interval Domain { get => domain; set => domain = value; }

        public Func<float, float> Function { get; }

        public Interval Image { get => image; set => image = value; }

        public float this[float value] => Function(Image.Min + (value - Domain.Min) * Image.Distance / Domain.Distance);

        public static Map Linear(Interval domain, Interval image)
        {
            return new Map(domain, image, x => x);
        }
    }
}
