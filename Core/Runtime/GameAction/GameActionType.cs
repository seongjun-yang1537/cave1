namespace Core
{
    public enum GameActionType
    {
        PlayerKilledEnemy,    // 플레이어가 몬스터를 죽임
        PlayerAcquiredItem,   // 플레이어가 아이템을 획득함
        PlayerDiggingOre,  // 플레이어가 자원을 채굴함
        PlayerReachedDepth    // 플레이어가 특정 깊이에 도달함
    }
}