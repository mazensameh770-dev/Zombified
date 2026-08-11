public class TileNotOccupiedRule : IPlacementRule
{
    public string FailureReason => "This tile already has a trap on it.";

    public bool IsValid(GridTile tile)
    {
        return tile.GetCurrentObject() == null;
    }
}