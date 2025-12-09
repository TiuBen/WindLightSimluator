public interface IStateAware
{
    bool IsActive { get; set; }
    string Theme { get; set; }   // "Day" / "Night"
    string Mode { get; set; }    // "Normal" / "Warning" / "Alarm"
}
