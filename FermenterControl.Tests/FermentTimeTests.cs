using FermenterControl;
using Xunit;

public class FermentTimeTests
{
    [Fact]
    public void Zero_or_negative_minutes_keep_vanilla()
    {
        Assert.Equal(2400f, FermentTime.ResolveSeconds(0f, 2400f));
        Assert.Equal(2400f, FermentTime.ResolveSeconds(-5f, 2400f));
    }

    [Fact]
    public void Minutes_become_seconds()
    {
        Assert.Equal(600f, FermentTime.ResolveSeconds(10f, 2400f));
    }

    [Theory]
    [InlineData(0f, "0m")]
    [InlineData(45f, "1m")]
    [InlineData(90f, "2m")]
    [InlineData(3600f, "1h 0m")]
    [InlineData(3661f, "1h 2m")]
    [InlineData(-4f, "0m")]
    public void Remaining_time_reads_in_hours_and_minutes(float seconds, string expected)
    {
        Assert.Equal(expected, FermentTime.FormatRemaining(seconds));
    }

    [Fact]
    public void Hover_appends_the_time_in_color()
    {
        string hover = FermentTime.WithRemaining("Fermenter ( Fermenting )", true, 100f, 700f, "orange");
        Assert.Equal("Fermenter ( Fermenting )\n<color=orange>10m</color>", hover);
    }

    [Fact]
    public void Finished_and_hidden_batches_keep_the_original_hover()
    {
        Assert.Equal("Ready", FermentTime.WithRemaining("Ready", true, 800f, 600f, "orange"));
        Assert.Equal("Empty", FermentTime.WithRemaining("Empty", false, 10f, 600f, "orange"));
        Assert.Equal("", FermentTime.WithRemaining("", true, 10f, 600f, "orange"));
    }

    [Fact]
    public void Bad_colors_fall_back_to_orange()
    {
        Assert.Equal("orange", FermentTime.SanitizeColor(null));
        Assert.Equal("orange", FermentTime.SanitizeColor("  "));
        Assert.Equal("orange", FermentTime.SanitizeColor("<color=red>"));
        Assert.Equal("#FFCC33", FermentTime.SanitizeColor(" #FFCC33 "));
    }
}
