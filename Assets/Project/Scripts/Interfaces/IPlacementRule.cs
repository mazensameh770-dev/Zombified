public interface IPlacementRule
{
    bool IsValid(GridTile tile);

    string FailureReason { get; }
}