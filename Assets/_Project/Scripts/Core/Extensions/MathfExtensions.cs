namespace GameCore.Extensions
{
    public static class MathfExtensions
    {
        public static float MapRange(this float value, float inRangeA, float inRangeB, float outRangeA, float outRangeB)
        {
            if (value <= inRangeA)
                return outRangeA;
            else if (value >= inRangeB)
                return outRangeB;
            return (value - inRangeA) * (outRangeB - outRangeA) / (inRangeB - inRangeA) + outRangeA;
        }
    }
}