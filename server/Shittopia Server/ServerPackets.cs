
namespace Shittopia_Server
{
    public enum ServerPackets
    {
        welcome = 1,
        spawnPlayer = 2,
        playerPosition = 3,
        playerSize = 4,
        disconnectUser = 5,
        sendItemData = 6,
        sendWorldData = 7,
        editWorldData = 8,
        sendInventoryData = 9,
        editInventoryData = 10, // 0x0000000A
        createDroppedItem = 11, // 0x0000000B
        removeDroppedItem = 12, // 0x0000000C
        sendChatMessage = 13, // 0x0000000D
        sendChatBubble = 14, // 0x0000000E
        authUser = 15, // 0x0000000F
        reply = 16, // 0x00000010
        sendPackData = 17, // 0x00000011
        setPosition = 18, // 0x00000012
        createTrade = 19, // 0x00000013
        addItemToTrade = 20, // 0x00000014
        tradeAction = 21, // 0x00000015
        removeTrade = 22, // 0x00000016
        sendNotification = 23, // 0x00000017
        joinWorld = 24, // 0x00000018
        signData = 25, // 0x00000019
        editCostumeData = 26, // 0x0000001A
        changeAnimation = 27, // 0x0000001B
        setHealth = 28, // 0x0000001C
        setStatus = 29, // 0x0000001D
        tempWorldData = 30, // 0x0000001E
        sendActiveWorlds = 31, // 0x0000001F
        createDialog = 32, // 0x00000020
        sendNews = 33, // 0x00000021
        sendCubixCount = 34, // 0x00000022
        updateAccountData = 35, // 0x00000023
        updateClientData = 36, // 0x00000024
    }
}
