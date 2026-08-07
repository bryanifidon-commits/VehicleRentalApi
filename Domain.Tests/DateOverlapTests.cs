namespace Domain.Tests;

public class DateOverlapTests
{
    [Theory]
    [InlineData("2026-09-01", "2026-09-05", "2026-09-03", "2026-09-08", true)]  // Overlaps end of range
    [InlineData("2026-09-05", "2026-09-10", "2026-09-01", "2026-09-06", true)]  // Overlaps start of range
    [InlineData("2026-09-02", "2026-09-04", "2026-09-01", "2026-09-05", true)]  // Completely inside existing range
    [InlineData("2026-09-10", "2026-09-15", "2026-09-01", "2026-09-05", false)] // Completely after existing range
    public void HasOverlap_EvaluatesDateRanges_ReturnsExpectedResult(
        string existingStartStr, string existingEndStr,
        string newStartStr, string newEndStr,
        bool expectedOverlap)
    {
        
        var existingStart = DateTime.Parse(existingStartStr);
        var existingEnd = DateTime.Parse(existingEndStr);
        var newStart = DateTime.Parse(newStartStr);
        var newEnd = DateTime.Parse(newEndStr);

        bool actualOverlap = (newStart < existingEnd) && (newEnd > existingStart);

        Assert.Equal(expectedOverlap, actualOverlap);
    }
}