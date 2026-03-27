using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod
{
    public static class Bar
    {
        public const string BarRoomPrefabName = "TrueZeal_Bar_Zone01_Room";
        private static GameObject BarRoom;

        public static void Init()
        {
            var room = Bundle.LoadAsset<GameObject>(BarRoomPrefabName);
            var barHandler = room.AddComponent<BarRoomHandler>();
            LoadedAssetsHandler.LoadedRoomPrefabs[barHandler.name] = barHandler;

            BarRoom = room;

            PortalSignAdder.AddSign(SignTypeE.Bar, LoadSprite("BarIcon", new(0.5f, 0f)));
            CustomCardHandler.AddCardGenerator(CardTypeE.EventBar, GenerateBarCard);
            AddCard();
        }

        public static void AddCard()
        {
            var zone1 = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_01");

            if (zone1 == null || zone1 is not ZoneBGDataBaseSO zone1BG)
                return;

            var barCard = new CardTypeInfo()
            {
                _cardInfo = new()
                {
                    cardType = CardTypeE.EventBar,
                    pilePosition = PilePositionType.End,
                    specialID = 0
                },
                _minimumAmount = 1,
                _maximumAmount = 1,
                _usePercentage = true,
                _percentage = 100
            };

            var deck = zone1BG._deckInfo;
            deck._possibleCards = deck._possibleCards.AddToArray(barCard);
        }

        private static void GenerateBarCard(ZoneBGDataBaseSO zone, CardInfo info)
        {
            var card = new Card(zone._zoneData.CardCount, 0, info.cardType, info.pilePosition, SignTypeE.Bar, BarRoomPrefabName);
            zone._zoneData.AddCard(card);
        }
    }
}
