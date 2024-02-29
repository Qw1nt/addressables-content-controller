﻿using Cysharp.Threading.Tasks;

namespace Qw1nt.Runtime.Shared.AddressablesContentController.SceneManagement
{
    public abstract class SceneManipulatorBase<TPreloader> where TPreloader : IScenePreloader
    {
        protected SceneManipulatorBase(TPreloader preloader)
        {
            Preloader = preloader;
        }

        public TPreloader Preloader { get; }

        public abstract UniTask Load(SceneData sceneData);
    }
}