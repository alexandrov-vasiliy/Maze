using System;
using _Project.Diamonds;
using _Project.Gameplay.Views;
using UnityEngine;

namespace _Project.Gameplay
{
    [Serializable]
    public class GameContext
    {
        [SerializeField] private RoundService roundService;
        [SerializeField] private DiamondPickedCounter diamondPickedCounter;
        [SerializeField] private DiamondSpawnConfig diamondSpawnConfig;
        [SerializeField] private GameObject exitWall;
        [SerializeField] private WinTrigger winTrigger;
        [SerializeField] private WinView winView;
        [SerializeField] private LoseView loseView;

        public RoundService RoundService => roundService;
        public DiamondPickedCounter DiamondPickedCounter => diamondPickedCounter;
        public DiamondSpawnConfig DiamondSpawnConfig => diamondSpawnConfig;
        public GameObject ExitWall => exitWall;
        public WinTrigger WinTrigger => winTrigger;
        public WinView WinView => winView;
        public LoseView LoseView => loseView;
    }
}
