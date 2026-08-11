public class TileNotBlockedRule : IPlacementRule
{
    public string FailureReason => "This tile is permanently blocked.";

    public bool IsValid(GridTile tile)
    {
        return !tile.isBlocked;
    }
}