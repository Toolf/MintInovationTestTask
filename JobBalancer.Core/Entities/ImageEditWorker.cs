using System;

namespace JobBalancer.Core.Entities
{
    public class ImageEditWorker
    {
        public int TimeProcessing { get; }
        public int Id { get; }

        public ImageEditWorker(int id, int timeProcessing)
        {
            Id = id;
            TimeProcessing = timeProcessing;
        }

        protected bool Equals(ImageEditWorker other)
        {
            return TimeProcessing == other.TimeProcessing && Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ImageEditWorker) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TimeProcessing, Id);
        }
    }
}