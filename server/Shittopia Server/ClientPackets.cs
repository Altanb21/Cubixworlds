
namespace Shittopia_Server
{
    public enum ClientPackets
    {
        welcomeReceived = 1,
        playerMovement = 2,
        editWorldData = 3,
        takeDroppedItem = 4,
        sendChatMessage = 5,
        authUser = 6,
        itemAction = 7,
        leaveWorld = 8,
        buyPack = 9,
        buttonClick = 10, // 0x0000000A
        addItemToTrade = 11, // 0x0000000B
        tradeAction = 12, // 0x0000000C
        playerSize = 13, // 0x0000000D
        editSignText = 14, // 0x0000000E
        wearCostume = 15, // 0x0000000F
        changeAnimation = 16, // 0x00000010
        changeAccountData = 17, // 0x00000011
        changeBio = 18, // 0x00000012
        changeAuthData = 19, // 0x00000013
    }
}
