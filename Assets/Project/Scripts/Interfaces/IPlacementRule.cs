public interface IPlacementRule
{
    bool IsValid(GridTileState tile);

    string FailureReason { get; }
}