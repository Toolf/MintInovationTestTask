﻿namespace Dissolve.Shared.Entities
{
    public class ImageEditJob 
    {
        protected bool Equals(ImageEditJob other)
        {
            return ImageCount == other.ImageCount;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ImageEditJob) obj);
        }

        public override int GetHashCode()
        {
            return ImageCount;
        }

        public int ImageCount { get; }

        public ImageEditJob(int imageCount)
        {
            ImageCount = imageCount;
        }
    }
}