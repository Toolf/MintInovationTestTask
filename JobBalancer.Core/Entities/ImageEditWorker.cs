using System;

namespace JobBalancer.Core.Entities
{
    public class ImageEditWorker
    {
        public int TimeProcessing { get; set; }
        public int Id { get; set; }

        public ImageEditWorker()
        {
            TimeProcessing = 1;
            Id = 0;
        }

        public ImageEditWorker(int id)
        {
            TimeProcessing = 1;
            Id = id;
        }

        public ImageEditWorker(int id, int timeProcessing)
        {
            Id = id;
            TimeProcessing = timeProcessing;
        }

        public double JobPerformance()
        {
            // ReSharper disable once PossibleLossOfFraction
            return (double) 1 / TimeProcessing;
        }

        public int JobExecutingSpeed()
        {
            return TimeProcessing;
        }

        protected bool Equals(ImageEditWorker other)
        {
            return TimeProcessing == other.TimeProcessing && Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ImageEditWorker) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TimeProcessing, Id);
        }
    }
}