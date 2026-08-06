public class TileNotBlockedRule : IPlacementRule
{
    public string FailureReason => "This tile is permanently blocked.";

    public bool IsValid(GridTileState tile)
    {
        return !tile.isBlocked;
    }
}