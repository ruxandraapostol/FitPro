namespace FitPro.Entities
{
    public enum PossibleFriendStatus : int
    {
        Nothing = 0,
        Friend = 1,
        RequestedFriendship = 2,
        AcceptFriendship = 3,
        InWaiting = 3,
        BlockedByUser = 4,
        BlockedByMe = 5
    }
}
